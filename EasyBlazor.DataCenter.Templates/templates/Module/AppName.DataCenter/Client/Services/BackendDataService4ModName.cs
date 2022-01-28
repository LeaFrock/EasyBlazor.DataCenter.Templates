using System.Net.Http.Json;
using AppName.DataCenter.Client.Helpers;
using AppName.DataCenter.Client.Models;
using AppName.DataCenter.Shared.ViewModels;

namespace AppName.DataCenter.Client.Services
{
    internal partial class BackendDataService
    {
        public Task<PageListViewModel<ModNameListItem>> GetModNamePageListAsync(IList<KeyValuePair<string, string>> pairs)
        {
            string url = UrlHelper.BuildHttpGetUrl(UrlHelper.ModNameAPI, pairs);
            return _client.GetFromJsonAsync<PageListViewModel<ModNameListItem>>(url);
        }

        public Task<ModNameDetailViewModel> GetModNameDetailAsync(int id)
        {
            return _client.GetFromJsonAsync<ModNameDetailViewModel>($"{UrlHelper.ModNameAPI}/{id}");
        }

        public async Task<ModNameDetailViewModel> UpdateModNameAsync(int id, IList<KeyValuePair<string, string>> pairs)
        {
            var form = new FormUrlEncodedContent(pairs);
            var rsp = await _client.PutAsync($"{UrlHelper.ModNameAPI}/{id}", form);
            rsp.EnsureSuccessStatusCode();
            var model = await rsp.Content.ReadFromJsonAsync<ModNameDetailViewModel>();
            return model;
        }

        public async Task<bool> DeleteModNameAsync(int id) 
        {
            var rsp = await _client.DeleteAsync($"{UrlHelper.ModNameAPI}/{id}");
            return rsp.IsSuccessStatusCode;
        }
    }
}
