using System;

namespace ScheduleTemplateModel.Period
{
    /// <summary>
    /// Период даты.
    /// </summary>
    public class DatePeriodTemplate
    {
        /// <summary>
        /// Начало.
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// Конец.
        /// </summary>
        public DateTimeOffset EndDate { get; set; }
    }
}
