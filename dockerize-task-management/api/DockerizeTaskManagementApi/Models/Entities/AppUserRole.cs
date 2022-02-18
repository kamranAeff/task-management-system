using Microsoft.AspNetCore.Identity;

namespace DockerizeTaskManagementApi.Models.Entities.Membership
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public virtual AppRole Role { get; set; }
    }
}
