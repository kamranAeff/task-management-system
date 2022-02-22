using AutoMapper;
using DockerizeTaskManagementApi.AppCode.Modules.AccountModule.Mapper.Dtos;
using DockerizeTaskManagementApi.Models.DataContexts;
using DockerizeTaskManagementApi.Models.Entities.Membership;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DockerizeTaskManagementApi.AppCode.Modules.AccountModule.Mapper.Resolvers
{
    public class UserOrganisationsResolver : IValueResolver<AppUser, AccountInfoDto, OrganisationDto[]>
    {
        readonly TaskManagementDbContext db;
        readonly IActionContextAccessor ctx;

        public UserOrganisationsResolver(TaskManagementDbContext db, IActionContextAccessor ctx)
        {
            this.db = db;
            this.ctx = ctx;
        }
        public OrganisationDto[] Resolve(AppUser source, AccountInfoDto destination, OrganisationDto[] destMember, ResolutionContext context)
        {
            var availableRoles = (from r in db.Roles
                         join ur in db.UserRoles on r.Id equals ur.RoleId
                         where ur.UserId == source.Id
                         select r.Name).ToArray();


            destination.IsSuperAdmin = availableRoles.Contains("SuperAdmin");
            destination.IsOrganisationAdmin = availableRoles.Contains("OrganisationAdmin");
            destination.IsUser = availableRoles.Contains("User");


            if (source.Organisation != null)
            {
                return new[] { context.Mapper.Map<OrganisationDto>(source.Organisation) };
            }
            else if(destination.IsSuperAdmin)
            {
                return context.Mapper.Map<OrganisationDto[]>(db.Organisations.AsNoTracking().ToArray());
            }

            return null;
        }
    }
}
