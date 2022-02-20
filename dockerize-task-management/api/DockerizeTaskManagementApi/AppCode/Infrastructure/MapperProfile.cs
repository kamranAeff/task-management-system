using AutoMapper;
using DockerizeTaskManagementApi.Models.DataContexts;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DockerizeTaskManagementApi.AppCode.Infrastructure
{
    public class MapperProfile : Profile
    {
        protected readonly IHostEnvironment hostEnvironment;
        protected readonly IActionContextAccessor ctx;
        protected readonly IConfiguration configuration;
        protected readonly TaskManagementDbContext db;

        public MapperProfile(IHostEnvironment hostEnvironment, IActionContextAccessor ctx, IConfiguration configuration, TaskManagementDbContext db)
        {
            this.hostEnvironment = hostEnvironment;
            this.configuration = configuration;
            this.ctx = ctx;
            this.db = db;
        }
    }
}
