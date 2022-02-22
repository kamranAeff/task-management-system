using DockerizeTaskManagementApi.AppCode.Extensions;
using DockerizeTaskManagementApi.Models.DataContexts;
using DockerizeTaskManagementApi.Models.Entities.Membership;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DockerizeTaskManagementApi.AppCode.Modules.AccountModule
{
    public class UserChooseQuery : IRequest<List<AppUser>>
    {
        public class UserChooseQueryHandler : IRequestHandler<UserChooseQuery, List<AppUser>>
        {
            readonly TaskManagementDbContext db;
            readonly IActionContextAccessor ctx;

            public UserChooseQueryHandler(TaskManagementDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }

            public async Task<List<AppUser>> Handle(UserChooseQuery request, CancellationToken cancellationToken)
            {
                var userId = ctx.GetPrincipalId();
                var organisationId = ctx.GetOrganisationId();

                var query = db.Users
                    .Where(u => u.Id != userId)
                    .AsQueryable();

                if (!ctx.IsInRole("SuperAdmin") && organisationId.HasValue)
                {
                    query = query.Where(u => u.OrganisationId == organisationId.Value);
                }
                else if (!ctx.IsInRole("SuperAdmin"))
                {
                    query = query.Take(0);
                }

                return await query.ToListAsync(cancellationToken);
            }
        }
    }
}
