using AutoMapper;
using DockerizeTaskManagementApi.AppCode.Infrastructure;
using DockerizeTaskManagementApi.AppCode.Modules.AccountModule;
using DockerizeTaskManagementApi.AppCode.Modules.AccountModule.Mapper.Dtos;
using DockerizeTaskManagementApi.AppCode.Modules.TaskBoardModule.Mapper.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DockerizeTaskManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        readonly IMediator mediator;
        readonly IMapper mapper;

        public AccountController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(SigninQuery query)
        {
            var response = await mediator.Send(query);

            return Ok(response);
        }

        [HttpGet("check-token")]
        public async Task<IActionResult> Check()
        {
            return Ok();
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
            var response = await mediator.Send(command);

            return Ok(response);
        }

        //[Authorize] set as default from startup
        [HttpGet("account-info")]
        public async Task<IActionResult> GetAccountInfo([FromRoute] AccountInfoQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
                return NotFound();

            var dtoResponse = mapper.Map<AccountInfoDto>(response);

            return Ok(dtoResponse);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAccountInfo([FromRoute] UserChooseQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
                return NotFound();

            var dtoResponse = mapper.Map<List<UserChooseDto>>(response);

            return Ok(dtoResponse);
        }
    }
}
