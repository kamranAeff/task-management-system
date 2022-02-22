using DockerizeTaskManagementApi.AppCode.Extensions;
using DockerizeTaskManagementApi.AppCode.Infrastructure;
using DockerizeTaskManagementApi.Models.DataContexts;
using DockerizeTaskManagementApi.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DockerizeTaskManagementApi.AppCode.Modules.TaskItemModule
{
    public class TaskItemCreateCommand : IRequest<JsonResponse>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public TaskItemPriority Priority { get; set; }
        public int BoardId { get; set; }
        public int[] MappedUserIds { get; set; }

        public class TaskItemCreateCommandHandler : IRequestHandler<TaskItemCreateCommand, JsonResponse>
        {
            readonly TaskManagementDbContext db;
            readonly IActionContextAccessor ctx;

            public TaskItemCreateCommandHandler(TaskManagementDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<JsonResponse> Handle(TaskItemCreateCommand request, CancellationToken cancellationToken)
            {
                int currentUserId = ctx.GetPrincipalId();
                int organisationId = ctx.GetOrganisationId().Value;

                var board = await db.Boards.FirstOrDefaultAsync(b => b.Id == request.BoardId && b.OrganisationId == organisationId, cancellationToken);

                if (board == null)
                {
                    return new JsonResponse
                    {
                        Error = true,
                        Message = "Tapşırıq lövhəsi mövcud deyil"
                    };
                }

                var existedTask = await db.Tasks.FirstOrDefaultAsync(b => b.Title.Equals(request.Title) && b.TaskBoardId == board.Id, cancellationToken);

                if (existedTask != null)
                {
                    return new JsonResponse
                    {
                        Error = true,
                        Message = "Bu adda tapşırıq artıq mövcuddur"
                    };
                }

                var task = new TaskItem
                {
                    Title = request.Title,
                    TaskBoardId = board.Id,
                    Deadline = request.Deadline,
                    Description = request.Description,
                    Status = TaskItemStatus.New,
                    Priority = request.Priority,
                    CreatedByUserId = currentUserId,
                };


                if (request.MappedUserIds == null)
                    goto withoutAssign;

                var availableUserIds = await (from u in db.Users.AsNoTracking()
                                              where u.OrganisationId == organisationId && u.EmailConfirmed && request.MappedUserIds.Contains(u.Id)
                                              select u.Id)
                                        .Distinct()
                                        .ToArrayAsync(cancellationToken);

                if (availableUserIds.Any())
                {
                    task.MappedUsers = new List<TaskItemUserCollection>();

                    foreach (var userId in availableUserIds)
                    {
                        //i want to use RabbitMq for Email Notification but is bad choice for demo version
                        task.MappedUsers.Add(new TaskItemUserCollection
                        {
                            UserId = userId
                        });
                    }
                }


            withoutAssign:  //without user assign
                db.Tasks.Add(task);
                await db.SaveChangesAsync(cancellationToken);


                return new JsonResponse
                {
                    Error = false,
                    Message = "Tapşırıq uğurla yaradıldı"
                };
            }
        }
    }
}
