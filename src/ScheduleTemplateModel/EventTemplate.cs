using System;
using System.Collections.Generic;
using ScheduleTemplateModel.Period;

namespace ScheduleTemplateModel
{
    /// <summary>
    /// Шаблон повторяемого события.
    /// </summary>
    public class EventTemplate
    {
        private byte _order = 1;

        /// <summary>
        /// Номер события в дне по расписанию времени. Начинается с 1.
        /// Ссылается на <see cref="ScheduleTemplate.RegularScheduleTime"/>.
        /// </summary>
        public byte Order
        {
            get => _order;
            set
            {
                if (Order < 1) throw new ArgumentOutOfRangeException(nameof(Order));
                _order = value;
            }
        }

        /// <summary>
        /// Индекс категории.
        /// Ссылается на <see cref="ScheduleTemplate.CategoryNames"/>.
        /// </summary>
        public int CategoryIndex { get; set; }

        /// <summary>
        /// Отрезки недель, в которые повторяется данное событие.
        /// </summary>
        public List<WeekPeriodTemplate> WeekPeriods { get; } = new List<WeekPeriodTemplate>();

        /// <summary>
        /// Индексы организаторов этого события, ссылаются на <see cref="ScheduleTemplate.Persons"/>.
        /// </summary>
        public HashSet<int> OrganizerIndexes { get; } = new HashSet<int>();


        /// <summary>
        /// Индексы участников этого события, ссылаются на <see cref="ScheduleTemplate.Persons"/>.
        /// </summary>
        public HashSet<int> ParticipantsIndexes { get; } = new HashSet<int>();

        /// <summary>
        /// Место проведения.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Тэг события.
        /// </summary>
        public TagTemplate Tag { get; set; }
    }
}