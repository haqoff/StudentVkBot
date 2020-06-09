using System.Collections.Generic;
using System.Threading.Tasks;
using StudentBotCore.Model;

namespace StudentBotCore.Repository
{
    public interface IRegularScheduleRepository
    {
        /// <summary>
        /// Получает стандартное расписание в чате.
        /// </summary>
        Task<List<EveryDayRegularSchedule>> GetSchedule(ulong chatId);

        /// <summary>
        /// Удаляет стандартное расписание в чате.
        /// </summary>
        Task ClearSchedule(ulong chatId);

        /// <summary>
        /// Устанавливает стандартное расписание в чате.
        /// </summary>
        Task AddSchedule(IEnumerable<EveryDayRegularSchedule> schedules);
    }
}