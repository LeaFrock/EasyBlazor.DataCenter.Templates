using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using NLog.Web;

namespace AppName.DataCenter.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                builder.Services.AddControllersWithViews();
                builder.Services.AddRazorPages();
                builder.Services.AddDataProtection()
                    .PersistKeysToFileSystem(new(Path.Combine(builder.Environment.ContentRootPath, "DataProtectionKeys")));

                string connStr = builder.Configuration.GetConnectionString("SqlServer");
                builder.Services.AddAuthModule(builder.Configuration);
                builder.Services.AddEntityFrameworkMySqlModule(connStr);
                builder.Services.AddAutoMapper();
                builder.Services.AddInternalModule();

#if RELEASE
                builder.Logging.ClearProviders();
#endif
                builder.Host.UseNLog();

                var app = builder.Build();
                if (app.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseWebAssemblyDebugging();
                }
                else
                {
                    app.UseExceptionHandler("/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }

                app.UseForwardedHeaders(new()
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
                string pathBase = app.Configuration.GetSection("Urls")?.GetValue<string>("PathBase");
                if (!string.IsNullOrEmpty(pathBase))
                {
                    app.UsePathBase(pathBase);
                }

                //app.UseHttpsRedirection();
                app.UseBlazorFrameworkFiles();
                app.UseStaticFiles();

                app.UseRouting();

                app.UseAuthentication()
                    .UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapRazorPages();
                    endpoints.MapControllers();
                    endpoints.MapFallbackToFile("index.html");
                });

                app.Run();
            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }
        }
    }
}