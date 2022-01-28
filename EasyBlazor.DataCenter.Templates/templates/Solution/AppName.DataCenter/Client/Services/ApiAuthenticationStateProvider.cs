using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace AppName.DataCenter.Client.Services
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private static readonly ClaimsPrincipal AnonymousModName = new(new ClaimsIdentity());
        private static readonly JwtSecurityTokenHandler JwtSecurityTokenHandler = new();

        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public ApiAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var savedToken = await _localStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrWhiteSpace(savedToken))
            {
                return new AuthenticationState(AnonymousModName);
            }
            var claims = ParseClaimsFromJwt(savedToken);
            var expiredClaim = claims?.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);
            if (expiredClaim is null || !long.TryParse(expiredClaim.Value, out var expiredTime) || DateTimeOffset.Now.ToUnixTimeSeconds() >= expiredTime)
            {
                await _localStorage.RemoveItemAsync("authToken");
                return new AuthenticationState(AnonymousModName);
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
        }

        public void MarkModNameAsAuthenticated(string account)
        {
            var authenticatedModName = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, account) }, "apiauth"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedModName));
            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkModNameAsLoggedOut()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(AnonymousModName)));
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var token = JwtSecurityTokenHandler.ReadJwtToken(jwt);
            return token.Claims;
        }
    }
}