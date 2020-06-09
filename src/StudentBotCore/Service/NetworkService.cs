using System;
using System.Net;
using System.Threading.Tasks;

namespace StudentBotCore.Service
{
    public class NetworkService : INetworkService, IDisposable
    {
        private readonly WebClient _client;

        public NetworkService()
        {
            _client = new WebClient();
        }

        public Task<string> LoadStringAsync(string url)
        {
            return _client.DownloadStringTaskAsync(url);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}