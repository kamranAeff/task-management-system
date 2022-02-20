using AutoMapper;
using DockerizeTaskManagementApi.AppCode.Extensions;
using DockerizeTaskManagementApi.AppCode.Modules.TaskBoardModule;
using DockerizeTaskManagementApi.AppCode.Modules.TaskBoardModule.Mapper.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DockerizeTaskManagementApi.Controllers
{
    [Route("api/boards")]
    [ApiController]
    public class TaskBoardController : ControllerBase
    {
        readonly IMediator mediator;
        readonly IMapper mapper;

        public TaskBoardController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [Authorize(Roles = "OrganisationAdmin,User")]//temporary
        [HttpPost("add")]
        public async Task<IActionResult> CreateBoard(TaskBoardCreateCommand command)
        {
            var response = await mediator.Send(command);

            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromRoute]TaskBoardAllQuery query)
        {
            var response = await mediator.Send(query);

            var dtoResponse = mapper.Map<List<TaskBoardDto>>(response,cfg=> {

                Request.GetHeaderValue("dateTimeOutFormat", cfg.Items);
            });

            return Ok(dtoResponse);
        }
    }
}
