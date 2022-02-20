using AutoMapper;
using DockerizeTaskManagementApi.AppCode.Configuration;
using DockerizeTaskManagementApi.AppCode.Infrastructure;
using DockerizeTaskManagementApi.Models.DataContext;
using DockerizeTaskManagementApi.Models.DataContexts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Linq;
using System.Reflection;

namespace DockerizeTaskManagementApi
{
    public class Startup
    {
        readonly IConfiguration configuration;
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;

        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(cfg =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                cfg.Filters.Add(new AuthorizeFilter(policy));
            }).AddNewtonsoftJson(cfg =>
            {
                cfg.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddRouting(cfg => cfg.LowercaseUrls = true);

            services.AddDbContext<TaskManagementDbContext>(cfg =>
            {
                string cString = configuration["Database:MsSql:cString"];

                if (!string.IsNullOrWhiteSpace(cString))
                {
                    cfg.UseSqlServer(cString);
                    return;
                }

                cString = configuration["Database:PostgreSql:cString"];

                if (!string.IsNullOrWhiteSpace(cString))
                {
                    cfg.UseNpgsql(cString);
                    return;
                }
            });


            CorsConfiguration.AddCors(services);
            SwaggerConfiguration.AddSwagger(services);
            JwtConfiguration.AddJwt(services, configuration);

            services.AddMediatR(typeof(Program).Assembly);
            services.AddAutoMapper(cfg =>
            {
                var provider = services.BuildServiceProvider();

                var webHostEnvironment = provider.GetRequiredService<IWebHostEnvironment>();
                var ctx = provider.GetRequiredService<IActionContextAccessor>();
                var db = provider.GetRequiredService<TaskManagementDbContext>();

                foreach (var profile in Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(Profile).IsAssignableFrom(t)))
                {
                    if (typeof(MapperProfile).IsAssignableFrom(profile))
                    {
                        var profileInstance = profile.GetConstructor(new[] { typeof(IWebHostEnvironment), typeof(IActionContextAccessor), typeof(IConfiguration), typeof(TaskManagementDbContext) })
                        .Invoke(new object[] { webHostEnvironment, ctx, configuration, db });

                        cfg.AddProfile(profileInstance as Profile);
                        continue;
                    }

                    cfg.AddProfile(profile);
                }
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.SeedMembership();

            CorsConfiguration.UseCors(app);
            SwaggerConfiguration.UseSwagger(app);
            JwtConfiguration.UseJwt(app);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(cfg =>
            {
                cfg.MapControllers();
            });
        }
    }
}
