using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentBotCore.Model;

namespace StudentBotCore.Repository
{
    public class RegularScheduleRepository : IRegularScheduleRepository
    {
        private readonly Func<StuDbContext> _contextCreator;

        public RegularScheduleRepository(Func<StuDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        /// <inheritdoc />
        public async Task<List<EveryDayRegularSchedule>> GetSchedule(ulong chatId)
        {
            await using var ctx = _contextCreator();
            return await ctx.EveryDayRegularSchedules.Where(s => s.ChatId == chatId).ToListAsync();
        }

        public async Task ClearSchedule(ulong chatId)
        {
            await using var ctx = _contextCreator();

            var toRemoveSchedules = ctx.EveryDayRegularSchedules.Where(s => s.ChatId == chatId);
            ctx.EveryDayRegularSchedules.RemoveRange(toRemoveSchedules);

            await ctx.SaveChangesAsync();
        }

        public async Task AddSchedule(IEnumerable<EveryDayRegularSchedule> schedules)
        {
            await using var ctx = _contextCreator();

            ctx.EveryDayRegularSchedules.AddRange(schedules);
            await ctx.SaveChangesAsync();
        }
    }
}