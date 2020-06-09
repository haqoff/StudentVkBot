using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleTemplateModel;
using ScheduleTemplateModel.Period;
using StudentBotCore.Model;

namespace StudentBotCore.Service.ScheduleGenerator
{
    /// <inheritdoc />
    public class ScheduleGenerateService : IScheduleGenerateService
    {
        private const int MaxPersons = 50;
        private const int MaxCategories = 50;
        private const int MaxRegularScheduleCount = 15;
        private const int MaxEventRepeatWeekPeriod = 30;
        private const int MaxEventTemplatesPerDay = 15;

        private const int MaxOrganizersPerEvent = 15;
        private const int MaxParticipantsPerEvent = 50;

        /// <inheritdoc />
        public ScheduleGenerateResult Generate(ulong chatId, ScheduleTemplate template)
        {
            var events = Enumerable.Empty<Event>();
            var regularSchedules = Enumerable.Empty<EveryDayRegularSchedule>();
            var errors = new List<ScheduleGenerateError>();

            ValidateLimitErrors(template, errors);
            ValidateScheduleTemplate(template, errors);

            if (errors.Count == 0)
            {
                events = GenerateEvents(chatId, template);
                regularSchedules = GenerateRegularSchedule(chatId, template);
            }

            return new ScheduleGenerateResult(errors.ToArray(), events, regularSchedules);
        }

        private static IEnumerable<EveryDayRegularSchedule> GenerateRegularSchedule(ulong chatId, ScheduleTemplate template)
        {
            return template.RegularScheduleTime.Select(t => new EveryDayRegularSchedule
                {ChatId = chatId, StartUtcTime = t.StartTime, Duration = t.Duration});
        }

        private static Category[] GenerateCategories(ulong chatId, ScheduleTemplate t)
        {
            return t.CategoryNames.Select(c => new Category {Name = c, ChatId = chatId}).ToArray();
        }

        private static Person[] GeneratePersons(ScheduleTemplate t)
        {
            return t.Persons.Select(p => new Person
            {
                Email = p.Email,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Patronymic = p.Patronymic,
                VkUser = p.VkUserId != null ? new VkUser() {Id = p.VkUserId.Value} : null
            }).ToArray();
        }

        private static IEnumerable<Event> GenerateEvents(ulong chatId, ScheduleTemplate t)
        {
            var persons = GeneratePersons(t);
            var categories = GenerateCategories(chatId, t);

            var events = new List<Event>();
            var days = CollectDays(t);

            foreach (var (dayOfWeek, eventTemplates) in days)
            {
                var startDay = t.FirstWeekMonday + TimeSpan.FromDays((int) dayOfWeek - 1);

                foreach (var eventTemplate in eventTemplates)
                {
                    var eventTimePeriod = t.RegularScheduleTime[eventTemplate.Order];
                    var weeks = WeekPeriodTemplate.GetWeekRange(eventTemplate.WeekPeriods);

                    foreach (var week in weeks)
                    {
                        var weekDay = startDay + TimeSpan.FromDays(7 * (week - 1));
                        var eventStart = weekDay.Date + eventTimePeriod.StartTime;

                        var ev = new Event
                        {
                            StartUtcDateTime = eventStart,
                            Duration = eventTimePeriod.Duration,
                            Category = categories[eventTemplate.CategoryIndex],
                            Description = eventTemplate.Description,
                            Location = eventTemplate.Location,
                            TagId = (ulong) eventTemplate.Tag
                        };

                        ev.Participants = eventTemplate.ParticipantsIndexes.Select(pi => new EventParticipant
                        {
                            Event = ev,
                            Person = persons[pi]
                        }).ToList();

                        ev.Organizers = eventTemplate.OrganizerIndexes.Select(oi => new EventOrganizer
                        {
                            Event = ev,
                            Person = persons[oi]
                        }).ToList();

                        events.Add(ev);
                    }
                }
            }

            return events;
        }

        private static void ValidateLimitErrors(ScheduleTemplate t, ICollection<ScheduleGenerateError> errors)
        {
            if (t.CategoryNames.Count > MaxCategories)
                errors.Add(ScheduleGenerateError.ExceedsLimitCategories);

            if (t.Persons.Count > MaxPersons)
                errors.Add(ScheduleGenerateError.ExceedsLimitPersons);

            if (t.RegularScheduleTime.Count > MaxRegularScheduleCount)
                errors.Add(ScheduleGenerateError.ExceedsLimitRegularSchedule);

            foreach (var (_, events) in CollectDays(t))
            {
                if (events.Count > MaxEventTemplatesPerDay)
                {
                    errors.Add(ScheduleGenerateError.ExceedsLimitEventTemplatesPerDay);
                }
                else
                {
                    foreach (var eventTemplate in events)
                    {
                        if (eventTemplate.OrganizerIndexes.Count > MaxOrganizersPerEvent)
                            errors.Add(ScheduleGenerateError.ExceedsLimitOrganizersPerEvent);

                        if (eventTemplate.ParticipantsIndexes.Count > MaxParticipantsPerEvent)
                            errors.Add(ScheduleGenerateError.ExceedsLimitParticipantsPerEvent);

                        var weeksCount = WeekPeriodTemplate.GetWeekRange(eventTemplate.WeekPeriods).Count();
                        if (weeksCount > MaxEventRepeatWeekPeriod)
                            errors.Add(ScheduleGenerateError.ExceedsLimitWeeksPerEvent);
                    }
                }
            }
        }

        private static void ValidateScheduleTemplate(ScheduleTemplate t, List<ScheduleGenerateError> errors)
        {
            foreach (var (_, events) in CollectDays(t))
            {
                foreach (var eventTemplate in events)
                {
                    ValidateEventTemplate(t, eventTemplate, errors);
                }
            }
        }

        private static void ValidateEventTemplate(ScheduleTemplate s, EventTemplate e, List<ScheduleGenerateError> errors)
        {
            if (e.Order > s.RegularScheduleTime.Count)
                errors.Add(ScheduleGenerateError.NoScheduleForEvent);

            if (e.CategoryIndex < 0 || e.CategoryIndex >= s.CategoryNames.Count)
                errors.Add(ScheduleGenerateError.NoCategoryForEvent);

            errors.AddRange(from organizerIndex in e.OrganizerIndexes
                where organizerIndex < 0 || organizerIndex >= s.Persons.Count
                select ScheduleGenerateError.NoPersonForEventOrganizer);

            errors.AddRange(from participantIndex in e.ParticipantsIndexes
                where participantIndex < 0 || participantIndex >= s.Persons.Count
                select ScheduleGenerateError.NoPersonForEventParticipant);
        }

        private static IEnumerable<(DayOfWeek dayOfWeek, List<EventTemplate> events)> CollectDays(ScheduleTemplate t)
        {
            return new[]
            {
                (DayOfWeek.Monday, t.Monday),
                (DayOfWeek.Tuesday, t.Tuesday),
                (DayOfWeek.Wednesday, t.Wednesday),
                (DayOfWeek.Thursday, t.Thursday),
                (DayOfWeek.Friday, t.Friday),
                (DayOfWeek.Saturday, t.Saturday),
                (DayOfWeek.Sunday, t.Sunday)
            };
        }
    }
}