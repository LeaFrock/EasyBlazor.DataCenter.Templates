using AppName.DataCenter.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInternalModule(this IServiceCollection services)
        {
            ConfigureAuth(services);

            services
                .AddScoped<IUserInterfaceDataService, UserInterfaceDataService>()
                .AddScoped<BackendDataService>();
            return services;
        }

        private static void ConfigureAuth(IServiceCollection services)
        {
            services
                .AddAuthorizationCore()
                .AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>()
                .AddScoped<IAuthService, AuthService>();
        }
    }
}