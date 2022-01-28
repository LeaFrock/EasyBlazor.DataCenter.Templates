using System.Net.Http.Json;
using AppName.DataCenter.Client.Models;

namespace AppName.DataCenter.Client.Services
{
    internal class UserInterfaceDataService : IUserInterfaceDataService
    {
        private readonly HttpClient _client;

        public UserInterfaceDataService(HttpClient client)
        {
            _client = client;
        }

        public Task<HomePageContent[]> LoadHomePageContentsAsync() => _client.GetFromJsonAsync<HomePageContent[]>("uidata/home/naviLinks.json?t=" + DateTime.Now.ToString("yyyyMMddHHmm"));

        public Task<UIBaseContent> GetBaseContentAsync() => _client.GetFromJsonAsync<UIBaseContent>("uidata/base.json?t=" + DateTime.Now.ToString("yyyyMMddHHmm"));
    }

    public interface IUserInterfaceDataService
    {
        Task<UIBaseContent> GetBaseContentAsync();

        Task<HomePageContent[]> LoadHomePageContentsAsync();
    }
}