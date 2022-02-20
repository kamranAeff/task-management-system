using DockerizeTaskManagementApi.AppCode.Extensions;
using DockerizeTaskManagementApi.AppCode.Infrastructure;
using DockerizeTaskManagementApi.Models.DataContexts;
using DockerizeTaskManagementApi.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DockerizeTaskManagementApi.AppCode.Modules.TaskBoardModule
{
    public class TaskBoardCreateCommand : IRequest<JsonResponse>
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public class TaskBoardCreateCommandHandler : IRequestHandler<TaskBoardCreateCommand, JsonResponse>
        {
            readonly TaskManagementDbContext db;
            readonly IActionContextAccessor ctx;

            public TaskBoardCreateCommandHandler(TaskManagementDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<JsonResponse> Handle(TaskBoardCreateCommand request, CancellationToken cancellationToken)
            {
                int currentUserId = ctx.GetPrincipalId();
                int? organisationId = ctx.GetOrganisationId();

                var existedBoard = await db.Boards.FirstOrDefaultAsync(b => b.Title.Equals(request.Title) && b.OrganisationId == organisationId, cancellationToken);

                if (existedBoard != null)
                {
                    return new JsonResponse
                    {
                        Error = true,
                        Message = "Bu adda tapşırıq lövhəsi artıq mövcuddur"
                    };
                }

                var taskBoard = new TaskBoard
                {
                    Title = request.Title,
                    Description = request.Description,
                    CreatedByUserId = currentUserId,
                    OrganisationId = organisationId.Value,
                };

                db.Boards.Add(taskBoard);
                await db.SaveChangesAsync(cancellationToken);


                return new JsonResponse
                {
                    Error = false,
                    Message = "Tapşırıq lövhəsi uğurla yaradıldı"
                };
            }
        }
    }
}
