using System.Threading.Tasks;

namespace StudentBotCore.Service
{
    interface INetworkService
    {
        Task<string> LoadStringAsync(string url);
    }
}
