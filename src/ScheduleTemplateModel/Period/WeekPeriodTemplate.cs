using System;
using System.Collections.Generic;
using System.Linq;

namespace ScheduleTemplateModel.Period
{
    /// <summary>
    /// Период недель.
    /// </summary>
    public class WeekPeriodTemplate
    {
        private int _startWeek = 1;
        private int _endWeek = 1;

        /// <summary>
        /// Начало (включительно) периода. Отсчет начинается с 1.
        /// 1 - это первая неделя, заданная в <see cref="ScheduleTemplate.FirstWeekMonday"/>
        /// </summary>
        public int StartWeek
        {
            get => _startWeek;
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException(nameof(StartWeek), "Стартовая неделя не может быть меньше 1");
                _startWeek = value;
            }
        }

        /// <summary>
        /// Конец (включительно) периода. Отсчет начинается с 1.
        /// Должна быть больше или равна <see cref="StartWeek"/>.
        /// </summary>
        public int EndWeek
        {
            get => _endWeek;
            set
            {
                if (value < StartWeek)
                    throw new ArgumentOutOfRangeException(nameof(EndWeek), "Конечная неделя не может быть меньше начальной");

                _endWeek = value;
            }
        }

        public int Length => EndWeek - StartWeek + 1;

        /// <summary>
        /// Правило повторяемости недель.
        /// </summary>
        public Repeatability RuleRepeat { get; set; } = Repeatability.EveryWeek;

        public enum Repeatability
        {
            /// <summary>
            /// Каждую неделю.
            /// </summary>
            EveryWeek = 0,

            /// <summary>
            /// Только нечетные недели.
            /// </summary>
            OnlyOddWeeks = 1,

            /// <summary>
            /// Только четные недели.
            /// </summary>
            OnlyEvenWeeks = 2
        }

        /// <summary>
        /// Получает перечисление недель, входящих в указанный период.
        /// </summary>
        public static IEnumerable<int> GetWeekRange(WeekPeriodTemplate period)
        {
            var allWeeks = Enumerable.Range(period.StartWeek, period.EndWeek);

            return period.RuleRepeat switch
            {
                Repeatability.EveryWeek => allWeeks,
                Repeatability.OnlyOddWeeks => allWeeks.Where(i => i % 2 == 1),
                Repeatability.OnlyEvenWeeks => allWeeks.Where(i => i % 2 == 0),
                _ => throw new ArgumentOutOfRangeException(nameof(RuleRepeat))
            };
        }

        /// <summary>
        /// Получает перечисление недель, входящих в указанные периоды.
        /// </summary>
        public static IEnumerable<int> GetWeekRange(IEnumerable<WeekPeriodTemplate> periods)
        {
            return periods.Select(GetWeekRange).SelectMany(p => p)
                .Distinct().OrderBy(i => i);
        }
    }
}