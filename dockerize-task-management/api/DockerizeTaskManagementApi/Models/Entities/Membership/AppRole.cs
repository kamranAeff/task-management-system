using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DockerizeTaskManagementApi.Models.Entities.Membership
{
    public class AppRole : IdentityRole<int>
    {
        public byte? Rank { get; set; }
        public virtual ICollection<AppUserRole> UserRoles { get; set; }
    }
}
