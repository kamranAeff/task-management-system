using AutoMapper;
using DockerizeTaskManagementApi.AppCode.Modules.TaskBoardModule.Mapper.Dtos;
using DockerizeTaskManagementApi.Models.DataContexts;
using DockerizeTaskManagementApi.Models.Entities;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DockerizeTaskManagementApi.AppCode.Modules.TaskBoardModule.Mapper.Resolvers
{
    public class UserChooseResolver : IValueResolver<TaskItem, TaskItemDto, UserChooseDto[]>
    {
        readonly TaskManagementDbContext db;
        readonly IActionContextAccessor ctx;

        public UserChooseResolver(TaskManagementDbContext db, IActionContextAccessor ctx)
        {
            this.db = db;
            this.ctx = ctx;
        }
        public UserChooseDto[] Resolve(TaskItem source, TaskItemDto destination, UserChooseDto[] destMember, ResolutionContext context)
        {
            var users = source?.MappedUsers?.Select(u => u.User)?.ToArray();

            if (users == null)
                return null;

            return context.Mapper.Map<UserChooseDto[]>(users);
        }
    }
}
