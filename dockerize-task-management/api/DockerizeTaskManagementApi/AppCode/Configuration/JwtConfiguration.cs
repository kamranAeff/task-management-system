using DockerizeTaskManagementApi.AppCode.Providers;
using DockerizeTaskManagementApi.Models.DataContexts;
using DockerizeTaskManagementApi.Models.Entities.Membership;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace DockerizeTaskManagementApi.AppCode.Configuration
{
    public static class JwtConfiguration
    {
        static internal IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<AppUser, AppRole>()
                .AddErrorDescriber<AppIdentityErrorProvider>()
                .AddEntityFrameworkStores<TaskManagementDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<SignInManager<AppUser>>();
            services.AddScoped<UserManager<AppUser>>();
            services.AddScoped<RoleManager<AppRole>>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IClaimsTransformation, AppClaimProvider>();

            services.AddAuthentication(cfg =>
            {
                cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["jwt:issuer"],
                    ValidAudience = configuration["jwt:audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:secret"])),
                    LifetimeValidator = (notBefore, expires, tokenToValidate, @param) => expires != null && expires > DateTime.UtcNow
                };
            });

            services.AddAuthorization();

            services.Configure<IdentityOptions>(cfg =>
            {

                //cfg.User.AllowedUserNameCharacters = "abcde";
                cfg.User.RequireUniqueEmail = true;

                cfg.Password.RequireDigit = false;
                cfg.Password.RequiredLength = 3;
                cfg.Password.RequiredUniqueChars = 1;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequireNonAlphanumeric = false;

                cfg.Lockout.MaxFailedAccessAttempts = 3;
                cfg.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
            });

            return services;
        }

        static internal IApplicationBuilder UseJwt(this IApplicationBuilder builder)
        {
            builder.UseAuthentication();

            builder.UseAuthorization();

            return builder;

        }
    }
}
