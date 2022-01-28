using System.Text;
using LinqToDB.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.IdentityModel.Tokens;
using AppName.DataCenter.Server.Data;
using AppName.DataCenter.Server.Options;
using AppName.DataCenter.Server.Profiles;
using AppName.DataCenter.Server.Services;
using AppName.DataCenter.Server.Services.Abstractions;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ServiceCollectionExtension
    {
        private const int DatabaseTimeoutSeconds = 10;

#if DEBUG
        private static readonly ILoggerFactory DebugLoggerFactory = new LoggerFactory(new[] { new DebugLoggerProvider() });
#endif

        public static void AddAuthModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = configuration.GetValue<string>("JWT:Issuer"),
                            ValidAudience = configuration.GetValue<string>("JWT:Audience"),
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWT:Key"))),
                            RequireExpirationTime = true
                        };
                    });

            services.Configure<JwtOptions>(configuration.GetSection("JWT"));
        }

        public static void AddEntityFrameworkMySqlModule(this IServiceCollection services, string connStr)
        {
            services.AddDbContext<AppNameDbContext>(opt =>
            {
#if DEBUG
                opt.UseLoggerFactory(DebugLoggerFactory);
#endif
                opt.UseSqlServer(connStr, builder =>
                {
                    builder.CommandTimeout(DatabaseTimeoutSeconds);
                });
            });

            LinqToDBForEFTools.Initialize();
        }

        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));
        }

        public static void AddInternalModule(this IServiceCollection services)
        {
            services.AddSingleton<IPasswordCryptor, DefaultPasswordCryptor>();
        }
    }
}