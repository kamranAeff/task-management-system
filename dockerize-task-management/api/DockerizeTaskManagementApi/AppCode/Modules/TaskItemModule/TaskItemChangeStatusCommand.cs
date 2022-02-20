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
    public class TaskItemChangeStatusCommand : IRequest<JsonResponse>
    {
        public int Id { get; set; }
        public TaskItemStatus Status { get; set; }

        public class TaskItemChangeStatusCommandHandler : IRequestHandler<TaskItemChangeStatusCommand, JsonResponse>
        {
            readonly TaskManagementDbContext db;
            readonly IActionContextAccessor ctx;

            public TaskItemChangeStatusCommandHandler(TaskManagementDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<JsonResponse> Handle(TaskItemChangeStatusCommand request, CancellationToken cancellationToken)
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
                        Message = "Yalnız siz yaratdığınız tapşırıqların statusunu dəyişə bilərsiz"
                    };
                }

                if (existedTask.Status == request.Status)
                {
                    return new JsonResponse
                    {
                        Error = true,
                        Message = "Bu tapşırıq hazırda qeyd edilmiş statusdadır"
                    };
                }

                existedTask.Status = request.Status;

                await db.SaveChangesAsync(cancellationToken);


                return new JsonResponse
                {
                    Error = false,
                    Message = "Tapşırığın statusu yeniləndi"
                };
            }
        }
    }
}
