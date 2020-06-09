using System;
using System.Threading.Tasks;
using StudentBotCore.Model;

namespace StudentBotCore.Repository
{
    public class ChatRepository : IChatRepository
    {
        private readonly Func<StuDbContext> _contextCreator;

        public ChatRepository(Func<StuDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task AddChat(VkChat chat)
        {
            await using var ctx = _contextCreator();
            ctx.VkChats.Add(chat);

            await ctx.SaveChangesAsync();
        }

        public async Task<bool> Exists(ulong chatId)
        {
            var chat = await GetChat(chatId);
            return chat != null;
        }

        public async Task<VkChat> GetChat(ulong chatId)
        {
            await using var ctx = _contextCreator();
            return await ctx.VkChats.FindAsync(chatId);
        }
    }
}