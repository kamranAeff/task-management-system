using DockerizeTaskManagementApi.AppCode.Modules.AccountModule.Mapper.Converters;
using DockerizeTaskManagementApi.AppCode.Modules.TaskBoardModule.Mapper.Dtos;
using DockerizeTaskManagementApi.Models.DataContexts;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DockerizeTaskManagementApi.AppCode.Modules.TaskItemModule
{
    public class TaskItemChooseMemberCommand : IRequest<UserChooseDto[]>
    {
        public int TaskId { get; set; }
        public int Id { get; set; }
        public bool Selected { get; set; }

        public class TaskItemChooseMemberCommandHandler : IRequestHandler<TaskItemChooseMemberCommand, UserChooseDto[]>
        {
            readonly TaskManagementDbContext db;
            readonly IActionContextAccessor ctx;

            public TaskItemChooseMemberCommandHandler(TaskManagementDbContext db,
                IActionContextAccessor ctx
                )
            {
                this.db = db;
                this.ctx = ctx;
            }

            public async Task<UserChooseDto[]> Handle(TaskItemChooseMemberCommand request, CancellationToken cancellationToken)
            {
                var selected = await db.TaskItemUserCollection.FirstOrDefaultAsync(tu => tu.UserId == request.Id && tu.TaskItemId == request.TaskId, cancellationToken);
                if (selected != null && request.Selected == false)
                {
                    db.TaskItemUserCollection.Remove(selected);
                    await db.SaveChangesAsync(cancellationToken);

                    goto resp;
                }

                selected = await db.TaskItemUserCollection.FirstOrDefaultAsync(tu => tu.UserId == request.Id && tu.TaskItemId == request.TaskId, cancellationToken);
                if (selected == null && request.Selected)
                {
                    db.TaskItemUserCollection.Add(new Models.Entities.TaskItemUserCollection
                    {
                        UserId = request.Id,
                        TaskItemId = request.TaskId
                    });
                    await db.SaveChangesAsync(cancellationToken);

                    goto resp;
                }



            resp:
                var data = (await (from u in db.Users
                                   join tu in db.TaskItemUserCollection on new { UserId = u.Id, TaskItemId = request.TaskId } equals new { tu.UserId, tu.TaskItemId } into lj_u_tu
                                   from ljUtU in lj_u_tu.DefaultIfEmpty()
                                   select new
                                   {
                                       User = u,
                                       Selected = ljUtU != null
                                   }).Distinct()
                                   .ToListAsync(cancellationToken))
                                   .Select(u => new UserChooseDto
                                   {
                                       Id = u.User.Id,
                                       Name = VisibleNameValueConverter.GetVisibleName(u.User),
                                       Selected = u.Selected
                                   }).ToArray();

                return data;
            }
        }
    }
}
