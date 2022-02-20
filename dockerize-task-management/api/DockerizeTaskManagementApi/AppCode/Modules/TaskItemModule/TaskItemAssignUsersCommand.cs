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
    public class TaskItemAssignUsersCommand : IRequest<JsonResponse>
    {
        public int Id { get; set; }
        public int[] MappedUserIds { get; set; }

        public class TaskItemAssignUsersCommandHandler : IRequestHandler<TaskItemAssignUsersCommand, JsonResponse>
        {
            readonly TaskManagementDbContext db;
            readonly IActionContextAccessor ctx;

            public TaskItemAssignUsersCommandHandler(TaskManagementDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<JsonResponse> Handle(TaskItemAssignUsersCommand request, CancellationToken cancellationToken)
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

                if (existedTask.Status == TaskItemStatus.Complated)
                {
                    return new JsonResponse
                    {
                        Error = true,
                        Message = "Tapşırıq artıq bitib!"
                    };
                }


                if (request.MappedUserIds == null || request.MappedUserIds.Length < 1)
                {
                    var item = existedTask.MappedUsers.FirstOrDefault();
                    while (item != null)
                    {
                        existedTask.MappedUsers.Remove(item);
                        item = existedTask.MappedUsers.FirstOrDefault();
                    }

                    await db.SaveChangesAsync(cancellationToken);
                    return new JsonResponse
                    {
                        Error = false,
                        Message = "Yadda saxlandı"
                    };
                }

                var newAvailableUserIds = await (from u in db.Users.AsNoTracking()
                                                 where u.OrganisationId == organisationId && u.EmailConfirmed &&  request.MappedUserIds.Contains(u.Id)
                                                 select u.Id)
                                        .Distinct()
                                        .ToArrayAsync(cancellationToken);

                var forRemoving = existedTask.MappedUsers.Select(u => u.UserId).Except(newAvailableUserIds);

                foreach (var userId in forRemoving)
                {
                    var removingItem = existedTask.MappedUsers.FirstOrDefault(u => u.UserId == userId);

                    if (removingItem == null) 
                        continue;

                    existedTask.MappedUsers.Remove(removingItem);
                }

                var forInserting = newAvailableUserIds.Except(existedTask.MappedUsers.Select(u => u.UserId));

                foreach (var userId in forInserting)
                {
                    existedTask.MappedUsers.Add(new TaskItemUserCollection
                    {
                        UserId = userId
                    });
                }

                db.Tasks.Update(existedTask);
                await db.SaveChangesAsync(cancellationToken);


                return new JsonResponse
                {
                    Error = false,
                    Message = "İcra edildi"
                };
            }
        }
    }
}
