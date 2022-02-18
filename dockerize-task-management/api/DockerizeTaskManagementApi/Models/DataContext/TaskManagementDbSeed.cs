using DockerizeTaskManagementApi.Models.DataContexts;
using DockerizeTaskManagementApi.Models.Entities.Membership;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DockerizeTaskManagementApi.Models.DataContext
{
    static public class TaskManagementDbSeed
    {
        static public IApplicationBuilder SeedMembership(this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<TaskManagementDbContext>();

                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                string adminEmail = configuration["jwt:adminEmail"];
                string adminPassword = configuration["jwt:adminPassword"];
                string superAdminRoleName = configuration["jwt:superAdminRoleName"];

                //db.Database.Migrate();// invoke update-database

                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

                var role = roleManager.FindByNameAsync(superAdminRoleName).Result;

                if (role == null)
                {
                    role = new AppRole
                    {
                        Rank = 1,
                        Name = superAdminRoleName
                    };

                    roleManager.CreateAsync(role).Wait();
                }

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                var adminUser = userManager.FindByEmailAsync(adminEmail).Result;

                if (adminUser == null)
                {
                    adminUser = new AppUser
                    {
                        Email = adminEmail,
                        UserName = adminEmail,
                        EmailConfirmed = true
                    };

                    var userResult = userManager.CreateAsync(adminUser, adminPassword).Result;

                    if (userResult.Succeeded)
                    {
                        userManager.AddToRoleAsync(adminUser, superAdminRoleName).Wait();
                    }
                }

                role = roleManager.FindByNameAsync("OrganisationAdmin").Result;

                if (role == null)
                {
                    role = new AppRole
                    {
                        Rank = 2,
                        Name = "OrganisationAdmin"
                    };

                    roleManager.CreateAsync(role).Wait();
                }

                role = roleManager.FindByNameAsync("User").Result;

                if (role == null)
                {
                    role = new AppRole
                    {
                        Rank = 3,
                        Name = "User"
                    };

                    roleManager.CreateAsync(role).Wait();
                }
            }


            return builder;
        }
    }
}
