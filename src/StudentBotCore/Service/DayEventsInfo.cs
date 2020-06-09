using System.Collections.Generic;
using StudentBotCore.Model;

namespace StudentBotCore.Service
{
    public class DayEventsInfo
    {
        public class ScheduleItemWithEvents
        {
            public ScheduleItemWithEvents(EveryDayRegularSchedule schedule, List<Event> events)
            {
                Schedule = schedule;
                Events = events;
            }

            public readonly EveryDayRegularSchedule Schedule;
            public readonly List<Event> Events;
        }

        /// <summary>
        /// Отсортированные события по расписанию времени начала и длительности.
        /// </summary>
        public ScheduleItemWithEvents[] OnScheduledEvents { get; }

        /// <summary>
        /// Отсортированные события, которые не входят в дневное расписание.
        /// </summary>
        public Event[] OffScheduledEvents { get; }

        public DayEventsInfo(ScheduleItemWithEvents[] onScheduledEvents, Event[] offScheduledEvents)
        {
            OffScheduledEvents = offScheduledEvents;
            OnScheduledEvents = onScheduledEvents;
        }
    }
}