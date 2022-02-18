using DockerizeTaskManagementApi.AppCode.Modules.TaskBoardModule;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DockerizeTaskManagementApi.Controllers
{
    [Route("api/boards")]
    [ApiController]
    public class TaskBoardController : ControllerBase
    {
        readonly IMediator mediator;

        public TaskBoardController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(Roles = "OrganisationAdmin,User")]//temporary
        [HttpPost("add")]
        public async Task<IActionResult> CreateBoard(TaskBoardCreateCommand command)
        {
            var response = await mediator.Send(command);

            return Ok(response);
        }
    }
}
