using System;
using System.Collections.Generic;

namespace ScheduleTemplateModel.Period
{
    /// <summary>
    /// Период времени.
    /// </summary>
    public class TimePeriodTemplate
    {
        /// <summary>
        /// Начало.
        /// </summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Длительность.
        /// </summary>
        public TimeSpan Duration { get; set; }
    }

    public class TimePeriodTemplateComparer : IComparer<TimePeriodTemplate>
    {
        public int Compare(TimePeriodTemplate x, TimePeriodTemplate y)
        {
            return x.StartTime != y.StartTime
                ? TimeSpan.Compare(x.StartTime, y.StartTime)
                : TimeSpan.Compare(x.Duration, y.Duration);
        }
    }
}