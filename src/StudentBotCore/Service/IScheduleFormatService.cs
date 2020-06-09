using System;
using System.Collections.Generic;
using StudentBotCore.Model;

namespace StudentBotCore.Service
{
    public interface IScheduleFormatService
    {
        /// <summary>
        /// Формирует строковое представление вывода событий вместе с расписанием.
        /// </summary>
        /// <param name="info">Информация о событиях и расписании дня.</param>
        /// <returns>Сначала идут все события по расписанию, потом оставшиеся.</returns>
        string FormatScheduleAndEvents(DayEventsInfo info, DateTime? requestedDate);

        /// <summary>
        /// Получает событие по указанному порядку.
        /// </summary>
        Event GetEventByOrder(DayEventsInfo info, int main, int additional);
        
        /// <summary>
        /// Формирует информацию о расписании и событиях.
        /// </summary>
        DayEventsInfo GetDayEventsInfo(IEnumerable<Event> events, IList<EveryDayRegularSchedule> schedule);
    }
}
