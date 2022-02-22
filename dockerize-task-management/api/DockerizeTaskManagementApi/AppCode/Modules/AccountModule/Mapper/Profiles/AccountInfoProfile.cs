using DockerizeTaskManagementApi.AppCode.Infrastructure;
using DockerizeTaskManagementApi.AppCode.Modules.AccountModule.Mapper.Converters;
using DockerizeTaskManagementApi.AppCode.Modules.AccountModule.Mapper.Dtos;
using DockerizeTaskManagementApi.AppCode.Modules.AccountModule.Mapper.Resolvers;
using DockerizeTaskManagementApi.AppCode.Modules.TaskBoardModule.Mapper.Dtos;
using DockerizeTaskManagementApi.Models.DataContexts;
using DockerizeTaskManagementApi.Models.Entities;
using DockerizeTaskManagementApi.Models.Entities.Membership;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DockerizeTaskManagementApi.AppCode.Modules.AccountModule.Mapper.Profiles
{
    public class AccountInfoProfile : MapperProfile
    {
        public AccountInfoProfile(IHostEnvironment hostEnvironment, IActionContextAccessor ctx, IConfiguration configuration, TaskManagementDbContext db)
            : base(hostEnvironment, ctx, configuration, db)
        {
            CreateMap<AppUser, AccountInfoDto>()
                .ForMember(dest => dest.VisibleName, opt => opt.ConvertUsing(new VisibleNameValueConverter(), src => src))
                .ForMember(dest => dest.Organisations, m => m.MapFrom(new UserOrganisationsResolver(db, ctx)));


            CreateMap<Organisation, OrganisationDto>();

            CreateMap<AppUser, UserChooseDto>()
                .ForMember(dest => dest.Name, opt => opt.ConvertUsing(new VisibleNameValueConverter(), m => m));
        }
    }
}
