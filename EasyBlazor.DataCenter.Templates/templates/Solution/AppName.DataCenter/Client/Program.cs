using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace AppName.DataCenter.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services
                 .AddScoped(_ => new HttpClient
                 {
                     BaseAddress = new Uri(builder.HostEnvironment.BaseAddress),
                     Timeout = TimeSpan.FromSeconds(10)
                 })
                .AddAntDesign()
                .AddBlazoredLocalStorage()
                .AddInternalModule();

            await builder.Build().RunAsync();
        }
    }
}