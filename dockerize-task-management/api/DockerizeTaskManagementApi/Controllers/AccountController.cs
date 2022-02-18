using DockerizeTaskManagementApi.AppCode.Modules.AccountModule;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DockerizeTaskManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        readonly IMediator mediator;

        public AccountController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(SigninQuery query)
        {
            var response = await mediator.Send(query);

            return Ok(response);
        }

        [Authorize(Roles = "SuperAdmin,OrganisationAdmin")]//temporary
        [HttpPost("create-sigup-ticket")]
        public async Task<IActionResult> SigupTicket(AccountCreateTicketCommand command)
        {
            var response = await mediator.Send(command);

            return Ok(response);
        }

        [AllowAnonymous]//temporary
        [HttpPost("complate-signup")]
        public async Task<IActionResult> ComplateSignup(AccountComplateSignupCommand command)
        {
            //http://localhost:4200/complate-signup?token=Op4ie8%2F%2FodyibLbxY8LYwm9Zlz1Pe87PMJoB9JZfyQDkAxHPb%2Bg7wourreW%2BEEpFUoKQ0pFtr0M%3D
            var response = await mediator.Send(command);

            return Ok(response);
        }
    }
}
