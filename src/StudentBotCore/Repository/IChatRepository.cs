using System.Threading.Tasks;
using StudentBotCore.Model;

namespace StudentBotCore.Repository
{
    public interface IChatRepository
    {
        Task AddChat(VkChat chat);
        Task<bool> Exists(ulong chatId);
        Task<VkChat> GetChat(ulong chatId);
    }
}
