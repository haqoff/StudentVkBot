using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentBotCore.Model;

namespace StudentBotCore.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly Func<StuDbContext> _contextCreator;

        public EventRepository(Func<StuDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        /// <inheritdoc />
        public async Task<List<Event>> GetEvents(ulong chatId, DateTime? date)
        {
            await using var ctx = _contextCreator();
            return await ctx.Events
                .Where(ev =>
                    ev.StartUtcDateTime == date || (ev.StartUtcDateTime != null && date != null &&
                                                    ev.StartUtcDateTime.Value.Date == date.Value.Date))
                .Include(ev => ev.Category).Where(ev => ev.Category.ChatId == chatId)
                .Include(ev => ev.Organizers).ThenInclude(o => o.Person)
                .Include(ev => ev.Participants).ThenInclude(p => p.Person)
                .Include(ev => ev.Tag)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task AddEvents(IEnumerable<Event> events)
        {
            await using var ctx = _contextCreator();

            ctx.Events.AddRange(events);
            await ctx.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task ClearAllEvents(ulong chatId)
        {
            await using var ctx = _contextCreator();

            var toRemoveCategories = ctx.Categories.Where(c => c.ChatId == chatId);
            var toRemoveEvents = ctx.Events
                .Include(ev => ev.Category)
                .Where(ev => ev.Category.ChatId == chatId);

            var toRemoveOrganizers = ctx.EventOrganizers
                .Include(eo => eo.Event)
                .ThenInclude(e => e.Category)
                .Where(eo => eo.Event.Category.ChatId == chatId);

            var toRemoveParticipants = ctx.EventParticipants
                .Include(ep => ep.Event)
                .ThenInclude(e => e.Category)
                .Where(eo => eo.Event.Category.ChatId == chatId);

            var toRemovePersons = toRemoveOrganizers.Select(o => o.Person).Concat(toRemoveParticipants.Select(p => p.Person));

            ctx.EventOrganizers.RemoveRange(toRemoveOrganizers);
            ctx.EventParticipants.RemoveRange(toRemoveParticipants);
            ctx.Persons.RemoveRange(toRemovePersons);

            ctx.Events.RemoveRange(toRemoveEvents);
            ctx.Categories.RemoveRange(toRemoveCategories);

            await ctx.SaveChangesAsync();
        }
    }
}