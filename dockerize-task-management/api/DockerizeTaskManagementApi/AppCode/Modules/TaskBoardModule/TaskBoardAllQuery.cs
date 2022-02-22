using DockerizeTaskManagementApi.AppCode.Extensions;
using DockerizeTaskManagementApi.Models.DataContexts;
using DockerizeTaskManagementApi.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DockerizeTaskManagementApi.AppCode.Modules.TaskBoardModule
{
    public class TaskBoardAllQuery : IRequest<List<TaskBoard>>
    {
        public class TaskBoardAllQueryHandler : IRequestHandler<TaskBoardAllQuery, List<TaskBoard>>
        {
            readonly TaskManagementDbContext db;
            readonly IActionContextAccessor ctx;

            public TaskBoardAllQueryHandler(TaskManagementDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<List<TaskBoard>> Handle(TaskBoardAllQuery request, CancellationToken cancellationToken)
            {
                int currentUserId = ctx.GetPrincipalId();
                int? organisationId = ctx.GetOrganisationId();


                var query = db.Boards
                    .Include(b => b.Organisation)
                    .Include(b => b.Tasks)
                    .ThenInclude(t => t.MappedUsers)
                    .ThenInclude(t => t.User)
                    .AsQueryable();


                if (!ctx.IsInRole("SuperAdmin") && organisationId.HasValue)
                {
                    query = query.Where(b => b.OrganisationId == organisationId.Value);
                }
                else if (!ctx.IsInRole("SuperAdmin"))
                {
                    query = query.Take(0);
                }

                var response = await query.ToListAsync(cancellationToken);

                return response;
            }
        }
    }
}
