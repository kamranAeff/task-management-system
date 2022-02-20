using DockerizeTaskManagementApi.AppCode.Extensions;
using DockerizeTaskManagementApi.Models.DataContexts;
using DockerizeTaskManagementApi.Models.Entities.Membership;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DockerizeTaskManagementApi.AppCode.Modules.AccountModule
{
    public class AccountInfoQuery : IRequest<AppUser>
    {
        public class AccountInfoQueryHandler : IRequestHandler<AccountInfoQuery, AppUser>
        {
            readonly UserManager<AppUser> userManager;
            readonly IActionContextAccessor ctx;
            readonly TaskManagementDbContext db;

            public AccountInfoQueryHandler(UserManager<AppUser> userManager,
                TaskManagementDbContext db,
                IActionContextAccessor ctx)
            {
                this.userManager = userManager;
                this.ctx = ctx;
                this.db = db;
            }

            public async Task<AppUser> Handle(AccountInfoQuery request, CancellationToken cancellationToken)
            {
                var userId = ctx.GetPrincipalId();

                var user = await db.Users
                    .Include(u=>u.Organisation)
                    .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

                return user;
            }
        }
    }
}
