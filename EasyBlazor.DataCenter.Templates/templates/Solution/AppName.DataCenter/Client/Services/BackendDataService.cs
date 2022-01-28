using System.Net.Http.Json;
using AppName.DataCenter.Client.Models;
using AppName.DataCenter.Shared.ViewModels;

namespace AppName.DataCenter.Client.Services
{
    internal partial class BackendDataService
    {
        private readonly HttpClient _client;

        public BackendDataService(HttpClient client)
        {
            _client = client;
        }
    }
}
