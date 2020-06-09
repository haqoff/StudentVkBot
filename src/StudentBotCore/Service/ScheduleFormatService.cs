using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using StudentBotCore.Helpers;
using StudentBotCore.Model;

namespace StudentBotCore.Service
{
    public class ScheduleFormatService : IScheduleFormatService
    {
        public string FormatScheduleAndEvents(DayEventsInfo info, DateTime? requestedDate)
        {
            var sb = new StringBuilder();
            sb.Append("События на ");

            var displayDay = requestedDate != null
                ? $"{requestedDate.Value.ToShortDateString()} ({CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(requestedDate.Value.DayOfWeek)})"
                : "отложенный день (од)";

            sb.AppendLine(displayDay);

            if (info.OnScheduledEvents.Length > 0)
            {
                sb.AppendLine("По расписанию:");

                for (var scheduleIndex = 0; scheduleIndex < info.OnScheduledEvents.Length; scheduleIndex++)
                {
                    var item = info.OnScheduledEvents[scheduleIndex];

                    if (item.Events.Count > 0)
                    {
                        for (var eventIndex = 0; eventIndex < item.Events.Count; eventIndex++)
                        {
                            var additionalOrder = item.Events.Count > 1
                                ? eventIndex + 1
                                : (int?) null;

                            AppendEventInfo(sb, scheduleIndex + 1, additionalOrder, item.Events[eventIndex]);
                        }
                    }
                    else
                    {
                        sb.Append(VkStr.Tab)
                            .AppendLine(
                                $"{scheduleIndex + 1}. [{item.Schedule.StartUtcTime:hh\\:mm}] - Нет");
                    }
                }
            }

            if (info.OffScheduledEvents.Length > 0)
            {
                sb.AppendLine("Не по расписанию:");

                for (int i = 0; i < info.OffScheduledEvents.Length; i++)
                {
                    var mainOrder = info.OnScheduledEvents.Length + i + 1;
                    AppendEventInfo(sb, mainOrder, null, info.OffScheduledEvents[i]);
                }
            }

            return sb.ToString();
        }

        private void AppendEventInfo(StringBuilder sb, int mainOrder, int? additionalOrder, Event e)
        {
            /*
                 *  1. [8:30] - [Заголовок эвента] [Категория] [Тэг] [Место] [Организаторы]
                 *      ?Участники:
                 *
                 *      ?Описание:
                 */

            #region Порядок

            sb.Append(VkStr.Tab).Append(mainOrder).Append('.');
            if (additionalOrder.HasValue) sb.Append(additionalOrder.Value);
            sb.Append(' ');

            #endregion

            #region Время начала

            if (e.StartUtcDateTime.HasValue)
                sb.Append('[')
                    .Append(e.StartUtcDateTime.Value.TimeOfDay.ToString("hh\\:mm"))
                    .Append("] - ");

            #endregion

            #region Категория

            sb.Append('[').Append(e.Category.Name).Append("] ");

            #endregion

            #region Тэг

            if (e.Tag != null) sb.Append('[').Append(e.Tag.Name).Append("] ");

            #endregion

            #region Место

            if (!string.IsNullOrEmpty(e.Location)) sb.Append('[').Append(e.Location).Append("] ");

            #endregion

            #region Организаторы

            if (e.Organizers != null && e.Organizers.Count > 0)
            {
                var organizersFullName = e.Organizers.Select(o => o.Person)
                    .Select(ToFullName);
                var organizersStr = string.Join("; ", organizersFullName);

                sb.Append('[').Append(organizersStr).Append("] ");
            }

            #endregion

            #region Участники и Описание

            var hasParticipantSec = e.Participants != null && e.Participants.Count > 0;
            var hasDescriptionSec = !string.IsNullOrEmpty(e.Description);
            if (hasParticipantSec || hasDescriptionSec)
            {
                sb.AppendLine().Append(VkStr.Tab.Repeat(2));

                if (hasParticipantSec)
                {
                    var participantsFullName = e.Participants.Select(o => o.Person)
                        .Select(ToFullName);
                    var participantsStr = string.Join("\n" + VkStr.Tab.Repeat(2), participantsFullName);

                    sb.Append('[').Append(participantsStr).Append("] ");
                }

                if (hasParticipantSec && hasDescriptionSec) sb.AppendLine().Append(VkStr.Tab.Repeat(2));

                if (hasDescriptionSec)
                {
                    sb.Append(e.Description);
                }
            }

            #endregion

            sb.AppendLine();
        }

        //TODO:
        private static string ToFullName(Person p)
        {
            var s = p.LastName;
            if (!string.IsNullOrEmpty(p.FirstName)) s += $" {p.FirstName[0]}.";
            if (!string.IsNullOrEmpty(p.Patronymic)) s += $" {p.Patronymic[0]}.";

            return s;
        }

        public Event GetEventByOrder(DayEventsInfo info, int main, int additional)
        {
            if (main < 1) return null;

            if (main <= info.OnScheduledEvents.Length)
            {
                var item = info.OnScheduledEvents[main - 1];
                if (additional < 1 || additional > item.Events.Count) return null;

                return item.Events[additional];
            }

            var offScheduledIndex = main - info.OnScheduledEvents.Length;
            if (offScheduledIndex > 0 && offScheduledIndex <= info.OffScheduledEvents.Length)
            {
                return info.OffScheduledEvents[offScheduledIndex - 1];
            }

            return null;
        }

        public DayEventsInfo GetDayEventsInfo(IEnumerable<Event> events, IList<EveryDayRegularSchedule> schedule)
        {
            // TODO: perf optimization

            var onScheduledEvents = schedule
                .InsertionSortInPlace(CompareSchedule)
                .Select(s => new DayEventsInfo.ScheduleItemWithEvents(s, new List<Event>(4)))
                .ToArray();

            var offScheduledEvents = new List<Event>();
            foreach (var ev in events)
            {
                if (ev.StartUtcDateTime.HasValue && ev.Duration.HasValue)
                {
                    if (onScheduledEvents.Find(item => item.Schedule.StartUtcTime == ev.StartUtcDateTime.Value.TimeOfDay &&
                                                       item.Schedule.Duration == ev.Duration.Value, out var foundItem))
                    {
                        foundItem.Events.Add(ev);
                        foundItem.Events.InsertionSortInPlace(CompareEvents);

                        continue;
                    }
                }

                offScheduledEvents.Add(ev);
            }

            offScheduledEvents.InsertionSortInPlace(CompareEvents);
            return new DayEventsInfo(onScheduledEvents, offScheduledEvents.ToArray());
        }

        private static int CompareEvents(Event e1, Event e2)
        {
            if (e1.StartUtcDateTime == null || e2.StartUtcDateTime == null) return (int) (e1.Id - e2.Id);
            var byStart = DateTime.Compare(e1.StartUtcDateTime.Value, e2.StartUtcDateTime.Value);
            if (byStart != 0) return byStart;

            if (e1.Duration == null || e2.Duration == null) return (int) (e1.Id - e2.Id);
            return TimeSpan.Compare(e1.Duration.Value, e2.Duration.Value);
        }

        private static int CompareSchedule(EveryDayRegularSchedule s1, EveryDayRegularSchedule s2)
        {
            var byStart = TimeSpan.Compare(s1.StartUtcTime, s2.StartUtcTime);
            return byStart != 0
                ? byStart
                : TimeSpan.Compare(s1.Duration, s2.Duration);
        }
    }
}