using AutoMapper;
using DockerizeTaskManagementApi.AppCode.Infrastructure;
using DockerizeTaskManagementApi.AppCode.Modules.AccountModule.Mapper.Converters;
using DockerizeTaskManagementApi.AppCode.Modules.TaskBoardModule.Mapper.Dtos;
using DockerizeTaskManagementApi.AppCode.Modules.TaskBoardModule.Mapper.Resolvers;
using DockerizeTaskManagementApi.Models.DataContexts;
using DockerizeTaskManagementApi.Models.Entities;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DockerizeTaskManagementApi.AppCode.Modules.TaskBoardModule.Mapper.Profiles
{
    public class TaskBoardProfile : MapperProfile
    {
        public TaskBoardProfile(IHostEnvironment hostEnvironment, IActionContextAccessor ctx, IConfiguration configuration, TaskManagementDbContext db)
            : base(hostEnvironment, ctx, configuration, db)
        {
            CreateMap<TaskBoard, TaskBoardDto>()
                .ForMember(dest=>dest.AuthorId,src=>src.MapFrom(m=>m.CreatedByUserId))
                .ForMember(dest=>dest.Author, opt => opt.ConvertUsing(new VisibleNameValueConverter(), src => src.CreatedByUser))
                .ForMember(dest => dest.CreatedDate, src => src.ConvertUsing(new DateToStringConverter(), u => u.CreatedDate));

            CreateMap<TaskItem, TaskItemDto>()
                .ForMember(dest => dest.AuthorId, src => src.MapFrom(m => m.CreatedByUserId))
                .ForMember(dest => dest.Author, opt => opt.ConvertUsing(new VisibleNameValueConverter(), src => src.CreatedByUser))
                .ForMember(dest => dest.CreatedDate, src => src.ConvertUsing(new DateToStringConverter(), u => u.CreatedDate))
                .ForMember(dest => dest.Deadline, src => src.ConvertUsing(new DateToStringConverter(), u => u.Deadline))
                .ForMember(dest => dest.Users, src => src.MapFrom(new UserChooseResolver(db,ctx)));
        }
    }
}
