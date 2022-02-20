using DockerizeTaskManagementApi.AppCode.Extensions;
using DockerizeTaskManagementApi.AppCode.Infrastructure;
using DockerizeTaskManagementApi.Models.DataContexts;
using DockerizeTaskManagementApi.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DockerizeTaskManagementApi.AppCode.Modules.TaskItemModule
{
    public class TaskItemChangePriorityCommand : IRequest<JsonResponse>
    {
        public int Id { get; set; }
        public TaskItemPriority Priority { get; set; }

        public class TaskItemChangePriorityCommandHandler : IRequestHandler<TaskItemChangePriorityCommand, JsonResponse>
        {
            readonly TaskManagementDbContext db;
            readonly IActionContextAccessor ctx;

            public TaskItemChangePriorityCommandHandler(TaskManagementDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<JsonResponse> Handle(TaskItemChangePriorityCommand request, CancellationToken cancellationToken)
            {
                int currentUserId = ctx.GetPrincipalId();
                int organisationId = ctx.GetOrganisationId().Value;

                var existedTask = await db.Tasks
                    .Include(t => t.TaskBoard)
                    .Include(t => t.MappedUsers)
                    .FirstOrDefaultAsync(b => b.Id == request.Id && b.TaskBoard.OrganisationId == organisationId, cancellationToken);

                if (existedTask == null)
                {
                    return new JsonResponse
                    {
                        Error = true,
                        Message = "Tapşırıq mövcud deyil"
                    };
                }

                if (existedTask.CreatedByUserId != currentUserId)
                {
                    return new JsonResponse
                    {
                        Error = true,
                        Message = "Yalnız siz yaratdığınız tapşırıqların önəmlilik statusunu dəyişə bilərsiz"
                    };
                }

                if (existedTask.Priority == request.Priority)
                {
                    return new JsonResponse
                    {
                        Error = true,
                        Message = "Bu tapşırıq hazırda qeyd edilmiş önəmlilik statusundadır"
                    };
                }

                existedTask.Priority = request.Priority;

                await db.SaveChangesAsync(cancellationToken);


                return new JsonResponse
                {
                    Error = false,
                    Message = "Tapşırığın önəmlilik statusu yeniləndi"
                };
            }
        }
    }
}
