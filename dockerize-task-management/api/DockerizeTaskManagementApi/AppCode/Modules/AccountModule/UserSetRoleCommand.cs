using DockerizeTaskManagementApi.AppCode.Extensions;
using DockerizeTaskManagementApi.AppCode.Infrastructure;
using DockerizeTaskManagementApi.Models.DataContexts;
using DockerizeTaskManagementApi.Models.Entities.Membership;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DockerizeTaskManagementApi.AppCode.Modules.AccountModule
{
    public class UserSetRoleCommand : IRequest<JsonResponse>
    {
        public int Id { get; set; }
        public string Role { get; set; }


        public class UserSetRoleCommandHandler : IRequestHandler<UserSetRoleCommand, JsonResponse>
        {
            readonly IActionContextAccessor ctx;
            readonly TaskManagementDbContext db;
            readonly UserManager<AppUser> userManager;
            readonly RoleManager<AppRole> roleManager;

            public UserSetRoleCommandHandler(TaskManagementDbContext db, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IActionContextAccessor ctx)
            {
                this.ctx = ctx;
                this.db = db;
                this.userManager = userManager;
                this.roleManager = roleManager;
            }

            public async Task<JsonResponse> Handle(UserSetRoleCommand request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByIdAsync(request.Id.ToString());

                if (user == null)
                {
                    return new JsonResponse
                    {
                        Error = true,
                        Message = "İstifadəçi mövcud deyil"
                    };
                }

                var role = await roleManager.FindByNameAsync(request.Role);


                if (role == null)
                {
                    return new JsonResponse
                    {
                        Error = true,
                        Message = "Rol mövcud deyil"
                    };
                }

                var result = await userManager.AddToRoleAsync(user, role.Name);

                if (!result.Succeeded)
                {
                    return new JsonResponse
                    {
                        Error = true,
                        Message = result.Errors.FirstOrDefault()?.Description
                    };
                }

                var roleNames = db.UserRoles.Include(ur => ur.Role).Where(ur => ur.RoleId != role.Id && ur.UserId == user.Id).Select(ur=>ur.Role.Name).ToArray();

                await userManager.RemoveFromRolesAsync(user, roleNames);

                return new JsonResponse
                {
                    Error = false,
                    Message = "Icra edildi"
                };
            }
        }
    }
}
