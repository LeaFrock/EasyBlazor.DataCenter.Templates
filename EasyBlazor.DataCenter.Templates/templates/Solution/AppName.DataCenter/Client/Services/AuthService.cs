using System.Net.Http.Headers;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using AppName.DataCenter.Client.Helpers;
using AppName.DataCenter.Shared.Requests;
using AppName.DataCenter.Shared.ViewModels;

namespace AppName.DataCenter.Client.Services
{
    internal class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider,
                           ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task<bool> Login(AccountLoginRequest rqtDto)
        {
            var content = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
            {
                new(nameof(AccountLoginRequest.Account), rqtDto.Account),
                new(nameof(AccountLoginRequest.Password), rqtDto.Password),
            });
            using var rsp = await _httpClient.PostAsync($"{UrlHelper.AccountAPI}/login", content);
            if (!rsp.IsSuccessStatusCode)
            {
                return false;
            }
            var authToken = await rsp.Content.ReadAsStringAsync();
            await _localStorage.SetItemAsync("authToken", authToken);
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkModNameAsAuthenticated(rqtDto.Account);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            return true;
        }

        public async Task Logout()
        {
            using var rsp = await _httpClient.PostAsync($"{UrlHelper.AccountAPI}/logout", default);
            if (rsp.IsSuccessStatusCode)
            {
                await _localStorage.RemoveItemAsync("authToken");
                ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkModNameAsLoggedOut();
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }

        public async Task<AccountShortInfoViewModel> GetAccountShortInfoAsync()
        {
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            if (state?.User is null)
            {
                return default;
            }
            var user = state.User;
            if (!int.TryParse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id))
            {
                return default;
            }
            return new()
            {
                Id = id,
                ModNameName = user.FindFirst(ClaimTypes.Name)?.Value,
                RealName = user.FindFirst(ClaimTypes.GivenName)?.Value,
                Role = user.FindFirst(ClaimTypes.Role)?.Value
            };
        }
    }

    public interface IAuthService
    {
        Task<bool> Login(AccountLoginRequest rqtDto);

        Task Logout();

        Task<AccountShortInfoViewModel> GetAccountShortInfoAsync();
    }
}