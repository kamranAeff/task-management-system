using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DockerizeTaskManagementApi.AppCode.Modules.AccountModule;
using DockerizeTaskManagementApi.AppCode.Modules.AccountModule.Mapper.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DockerizeTaskManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        readonly IMediator mediator;
        readonly IMapper mapper;

        public UsersController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Users([FromRoute] UserChooseQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
                return NotFound();

            var dtoResponse = mapper.Map<List<AccountInfoDto>>(response);

            return Ok(dtoResponse);
        }

        [HttpPost("set-role")]
        public async Task<IActionResult> SetRole(UserSetRoleCommand command)
        {
            var response = await mediator.Send(command);
            return Ok(response);
        }
    }
}
