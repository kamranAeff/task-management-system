using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DockerizeTaskManagementApi.AppCode.Configuration
{
    public static class CorsConfiguration
    {
        internal static IServiceCollection AddCors(this IServiceCollection services)
        {
            services.AddCors(cfg =>
            {
                cfg.AddPolicy("allowAny", p =>
                {
                    p.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });


            return services;
        }

        internal static IApplicationBuilder UseCors(this IApplicationBuilder app)
        {

            app.UseCors("allowAny");

            return app;
        }
    }
}
