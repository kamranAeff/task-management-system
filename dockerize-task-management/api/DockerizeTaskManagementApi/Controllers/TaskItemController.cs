using DockerizeTaskManagementApi.AppCode.Modules.TaskItemModule;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DockerizeTaskManagementApi.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskItemController : ControllerBase
    {
        readonly IMediator mediator;

        public TaskItemController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(Roles = "OrganisationAdmin,User")]//temporary
        [HttpPost("add")]
        public async Task<IActionResult> CreateTask(TaskItemCreateCommand command)
        {
            var response = await mediator.Send(command);

            return Ok(response);
        }

        [Authorize(Roles = "OrganisationAdmin,User")]
        [HttpPost("assign")]
        public async Task<IActionResult> AssignUser(TaskItemAssignUsersCommand command)
        {
            var response = await mediator.Send(command);

            return Ok(response);
        }

        [Authorize(Roles = "OrganisationAdmin,User")]
        [HttpPost("change-status")]
        public async Task<IActionResult> ChangeStatus(TaskItemChangeStatusCommand command)
        {
            var response = await mediator.Send(command);

            return Ok(response);
        }

        [Authorize(Roles = "OrganisationAdmin,User")]
        [HttpPost("change-priority")]
        public async Task<IActionResult> ChangePriority(TaskItemChangePriorityCommand command)
        {
            var response = await mediator.Send(command);

            return Ok(response);
        }


        [HttpGet("{TaskId}/members")]
        public async Task<IActionResult> AllMembers([FromRoute]TaskItemMembersQuery query)
        {
            var response = await mediator.Send(query);

            return Ok(response);
        }

        [Authorize(Roles = "OrganisationAdmin,User")]
        [HttpPost("choose-member")]
        public async Task<IActionResult> ChooseMember(TaskItemChooseMemberCommand command)
        {
            var response = await mediator.Send(command);

            return Ok(response);
        }
    }
}
