using DockerizeTaskManagementApi.Models.DataContext;
using DockerizeTaskManagementApi.Models.DataContexts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

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

            services.AddMediatR(typeof(Program).Assembly);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.SeedMembership();

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
