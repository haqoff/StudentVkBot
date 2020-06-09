using System;
using System.Collections.Generic;
using ScheduleTemplateModel.Period;

namespace ScheduleTemplateModel
{
    public class ScheduleTemplate
    {
        private DateTimeOffset _firstWeekMonday;

        /// <summary>
        /// Дата самого первого понедельника этого расписания.
        /// </summary>
        public DateTimeOffset FirstWeekMonday
        {
            get => _firstWeekMonday;
            set
            {
                if (value.DayOfWeek != DayOfWeek.Monday)
                {
                    throw new ArgumentException(nameof(DayOfWeek));
                }

                _firstWeekMonday = value;
            }
        }

        /// <summary>
        /// Список отрезков дат, которые необходимо пропустить при заполнении расписания.
        /// </summary>
        public List<DatePeriodTemplate> SkipDayPeriods { get; } = new List<DatePeriodTemplate>();

        /// <summary>
        /// Стандартное время расписания.
        /// </summary>
        public List<TimePeriodTemplate> RegularScheduleTime { get; } =
            new List<TimePeriodTemplate>();

        /// <summary>
        /// Все люди - как и организаторы, так и участники.
        /// </summary>
        public List<PersonTemplate> Persons { get; } = new List<PersonTemplate>();

        /// <summary>
        /// Все имена категорий.
        /// </summary>
        public List<string> CategoryNames { get; } = new List<string>();

        public List<EventTemplate> Monday { get; } = new List<EventTemplate>();
        public List<EventTemplate> Tuesday { get; } = new List<EventTemplate>();
        public List<EventTemplate> Wednesday { get; } = new List<EventTemplate>();
        public List<EventTemplate> Thursday { get; } = new List<EventTemplate>();
        public List<EventTemplate> Friday { get; } = new List<EventTemplate>();
        public List<EventTemplate> Saturday { get; } = new List<EventTemplate>();
        public List<EventTemplate> Sunday { get; } = new List<EventTemplate>();
    }
}