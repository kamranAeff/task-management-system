using DockerizeTaskManagementApi.Models.Entities;
using DockerizeTaskManagementApi.Models.Entities.Membership;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DockerizeTaskManagementApi.Models.DataContexts
{
    public class TaskManagementDbContext : IdentityDbContext<AppUser, AppRole, int, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
    {
        public TaskManagementDbContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<TaskBoard> Boards { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<TaskItemUserCollection> TaskItemUserCollection { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Membership

            if (this.Database.IsNpgsql())
            {
                TaskManagementDbConfiguration.ConfigurePostgreSql(modelBuilder);
            }
            else if (this.Database.IsSqlServer())
            {
                TaskManagementDbConfiguration.ConfigureMsSql(modelBuilder);
            }
            else
            {
                TaskManagementDbConfiguration.ConfigureOtherSql(modelBuilder);
            }

            #endregion
        }
    }
}
