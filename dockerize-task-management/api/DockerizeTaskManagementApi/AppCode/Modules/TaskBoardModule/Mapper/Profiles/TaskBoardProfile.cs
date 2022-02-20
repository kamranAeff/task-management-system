using AutoMapper;
using DockerizeTaskManagementApi.AppCode.Modules.AccountModule.Mapper.Converters;
using DockerizeTaskManagementApi.AppCode.Modules.TaskBoardModule.Mapper.Dtos;
using DockerizeTaskManagementApi.Models.Entities;

namespace DockerizeTaskManagementApi.AppCode.Modules.TaskBoardModule.Mapper.Profiles
{
    public class TaskBoardProfile : Profile
    {
        public TaskBoardProfile()
        {
            CreateMap<TaskBoard, TaskBoardDto>()
                .ForMember(dest=>dest.AuthorId,src=>src.MapFrom(m=>m.CreatedByUserId))
                .ForMember(dest=>dest.Author, opt => opt.ConvertUsing(new VisibleNameValueConverter(), src => src.CreatedByUser))
                .ForMember(dest => dest.CreatedDate, src => src.ConvertUsing(new DateToStringConverter(), u => u.CreatedDate));

            CreateMap<TaskItem, TaskItemDto>()
                .ForMember(dest => dest.AuthorId, src => src.MapFrom(m => m.CreatedByUserId))
                .ForMember(dest => dest.Author, opt => opt.ConvertUsing(new VisibleNameValueConverter(), src => src.CreatedByUser))
                .ForMember(dest => dest.CreatedDate, src => src.ConvertUsing(new DateToStringConverter(), u => u.CreatedDate))
                .ForMember(dest => dest.Deadline, src => src.ConvertUsing(new DateToStringConverter(), u => u.Deadline));
        }
    }
}
