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
