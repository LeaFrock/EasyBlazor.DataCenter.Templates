using System.Text;
using AppName.DataCenter.Server.Data;
using AppName.DataCenter.Server.Options;
using AppName.DataCenter.Server.Profiles;
using AppName.DataCenter.Server.Services;
using AppName.DataCenter.Server.Services.Abstractions;
using LinqToDB.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ServiceCollectionExtension
    {
        private const int DatabaseTimeoutSeconds = 10;

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
                // See generated sql from EF Core in 'Output/Debug'
                opt.UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider() }));
#endif
                opt.UseSqlServer(connStr, builder =>
                {
                    builder.CommandTimeout(DatabaseTimeoutSeconds);
                });
            });

            LinqToDBForEFTools.Initialize();

#if DEBUG
            // See generated sql from linq2db in 'Output/Debug'
            LinqToDB.Data.DataConnection.TurnTraceSwitchOn();
            LinqToDB.Data.DataConnection.WriteTraceLine = (s1, s2, level) =>
            {
                if (level == System.Diagnostics.TraceLevel.Info && s2 == nameof(LinqToDB.Data.DataConnection))
                {
                    System.Diagnostics.Debug.WriteLine(s1);
                }
            };
#endif
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