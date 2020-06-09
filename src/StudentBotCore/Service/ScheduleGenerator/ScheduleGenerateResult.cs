using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using StudentBotCore.Model;

namespace StudentBotCore.Service.ScheduleGenerator
{
    /// <summary>
    /// Представляет собой результат генерации расписания на основе шаблона.
    /// </summary>
    public class ScheduleGenerateResult
    {
        /// <summary>
        /// Ошибки.
        /// </summary>
        public ScheduleGenerateError[] Errors { get; }

        /// <summary>
        /// Признак того, что имеются ошибки.
        /// </summary>
        public bool HasErrors => Errors.Length > 0;

        /// <summary>
        /// Сгенерированные события.
        /// </summary>
        public IEnumerable<Event> Events { get; }

        /// <summary>
        /// Сгенерированное регулярное расписание.
        /// </summary>
        public IEnumerable<EveryDayRegularSchedule> RegularSchedules { get; }

        public ScheduleGenerateResult([NotNull] ScheduleGenerateError[] errors, [NotNull] IEnumerable<Event> events,
            [NotNull] IEnumerable<EveryDayRegularSchedule> regularSchedules
        )
        {
            Events = events;
            RegularSchedules = regularSchedules;
            Errors = errors;
        }
    }
}