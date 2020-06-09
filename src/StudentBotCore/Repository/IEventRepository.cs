using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudentBotCore.Model;

namespace StudentBotCore.Repository
{
    public interface IEventRepository
    {
        /// <summary>
        /// Получает все события, которые содержат всю информацию (категория, тэг, участники, организаторы и тд), на указанный день.
        /// </summary>
        /// <param name="chatId">Идентификатор чата.</param>
        /// <param name="date">День.</param>
        /// <returns>Список событий, содержащие всю информацию, на указанный день.</returns>
        Task<List<Event>> GetEvents(ulong chatId, DateTime? date);

        /// <summary>
        /// Удаляет все категории с событиями (со всей информацией - организаторами, участниками) для указанного чата.
        /// </summary>
        /// <param name="chatId">Идентификатор чата.</param>
        Task ClearAllEvents(ulong chatId);

        /// <summary>
        /// Добавляет категории, вместе с событиями внутри.
        /// Должны быть заполнены: <see cref="Category.ChatId"/>,
        /// <see cref="Category.Name"/>,
        /// <see cref="Category.Events"/> 
        /// </summary>
        Task AddEvents(IEnumerable<Event> events);
    }
}