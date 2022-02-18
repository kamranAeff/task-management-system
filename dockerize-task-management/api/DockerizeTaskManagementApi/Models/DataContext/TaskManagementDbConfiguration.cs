using DockerizeTaskManagementApi.Models.Entities.Membership;
using Microsoft.EntityFrameworkCore;

namespace DockerizeTaskManagementApi.Models.DataContexts
{
    public static class TaskManagementDbConfiguration
    {
        internal static void ConfigureMsSql(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(e => e.ToTable("Users", "Membership"));
            modelBuilder.Entity<AppRole>(e => e.ToTable("Roles", "Membership"));

            modelBuilder.Entity<AppUserRole>(e =>
            {
                e.HasOne(p => p.Role)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(p => p.RoleId);
                e.ToTable("UserRoles", "Membership");
            });

            modelBuilder.Entity<AppUserClaim>(e => e.ToTable("UserClaims", "Membership"));
            modelBuilder.Entity<AppRoleClaim>(e => e.ToTable("RoleClaims", "Membership"));

            modelBuilder.Entity<AppUserToken>(e => e.ToTable("UserTokens", "Membership"));
            modelBuilder.Entity<AppUserLogin>(e => e.ToTable("UserLogins", "Membership"));
        }

        internal static void ConfigurePostgreSql(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(e =>
            {
                e.Property(p => p.Id).HasColumnName("id");
                e.Property(p => p.UserName).HasColumnName("user_name");
                e.Property(p => p.NormalizedUserName).HasColumnName("normalized_user_name");
                e.Property(p => p.Email).HasColumnName("email");
                e.Property(p => p.NormalizedEmail).HasColumnName("normalized_email");
                e.Property(p => p.EmailConfirmed).HasColumnName("email_confirmed");
                e.Property(p => p.PasswordHash).HasColumnName("password_hash");
                e.Property(p => p.SecurityStamp).HasColumnName("security_stamp");
                e.Property(p => p.ConcurrencyStamp).HasColumnName("concurrency_stamp");
                e.Property(p => p.PhoneNumber).HasColumnName("phone_number");
                e.Property(p => p.PhoneNumberConfirmed).HasColumnName("phone_number_confirmed");
                e.Property(p => p.TwoFactorEnabled).HasColumnName("two_factor_enabled");
                e.Property(p => p.LockoutEnd).HasColumnName("lockout_end");
                e.Property(p => p.LockoutEnabled).HasColumnName("lockout_enabled");
                e.Property(p => p.AccessFailedCount).HasColumnName("access_failed_count");
                e.Property(p => p.Name).HasColumnName("name");
                e.Property(p => p.Surname).HasColumnName("surname");
                e.Property(p => p.Patronymic).HasColumnName("patronymic");
                e.ToTable("users", "membership");
            });

            modelBuilder.Entity<AppRole>(e =>
            {
                e.Property(p => p.Id).HasColumnName("id");
                e.Property(p => p.Name).HasColumnName("name");
                e.Property(p => p.NormalizedName).HasColumnName("normalized_name");
                e.Property(p => p.ConcurrencyStamp).HasColumnName("concurrency_stamp");
                e.Property(p => p.Rank).HasColumnName("rank");
                e.ToTable("roles", "membership");
            });

            modelBuilder.Entity<AppUserRole>(e =>
            {
                e.HasOne(p => p.Role)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(p => p.RoleId);

                e.Property(p => p.UserId).HasColumnName("user_id");
                e.Property(p => p.RoleId).HasColumnName("role_id");
                e.ToTable("user_roles", "membership");
            });

            modelBuilder.Entity<AppUserClaim>(e =>
            {
                e.Property(p => p.Id).HasColumnName("id");
                e.Property(p => p.UserId).HasColumnName("user_id");
                e.Property(p => p.ClaimType).HasColumnName("claim_type");
                e.Property(p => p.ClaimValue).HasColumnName("claim_value");
                e.ToTable("user_claims", "membership");
            });

            modelBuilder.Entity<AppRoleClaim>(e =>
            {
                e.Property(p => p.Id).HasColumnName("id");
                e.Property(p => p.RoleId).HasColumnName("role_id");
                e.Property(p => p.ClaimType).HasColumnName("claim_type");
                e.Property(p => p.ClaimValue).HasColumnName("claim_value");
                e.ToTable("role_claims", "membership");
            });

            modelBuilder.Entity<AppUserLogin>(e =>
            {
                e.Property(p => p.LoginProvider).HasColumnName("login_provider");
                e.Property(p => p.ProviderKey).HasColumnName("provider_key");
                e.Property(p => p.ProviderDisplayName).HasColumnName("provider_display_name");
                e.Property(p => p.UserId).HasColumnName("user_id");
                e.ToTable("user_logins", "membership");
            });

            modelBuilder.Entity<AppUserToken>(e =>
            {
                e.Property(p => p.UserId).HasColumnName("user_id");
                e.Property(p => p.LoginProvider).HasColumnName("login_provider");
                e.Property(p => p.Name).HasColumnName("name");
                e.Property(p => p.Value).HasColumnName("value");
                e.ToTable("user_tokens", "membership");
            });

        }

        internal static void ConfigureOtherSql(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(e => e.ToTable("membership_users"));
            modelBuilder.Entity<AppRole>(e => e.ToTable("membership_roles"));
            modelBuilder.Entity<AppUserClaim>(e => e.ToTable("membership_userclaims"));
            modelBuilder.Entity<AppRoleClaim>(e => e.ToTable("membership_roleclaims"));
            modelBuilder.Entity<AppUserRole>(e =>
            {
                e.HasOne(p => p.Role)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(p => p.RoleId);

                e.ToTable("membership_userroles");
            });

            modelBuilder.Entity<AppUserToken>(e => e.ToTable("membership_usertokens"));
            modelBuilder.Entity<AppUserLogin>(e => e.ToTable("membership_userlogins"));
        }
    }
}
