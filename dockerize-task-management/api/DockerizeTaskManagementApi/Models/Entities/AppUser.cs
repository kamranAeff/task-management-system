using Microsoft.AspNetCore.Identity;

namespace DockerizeTaskManagementApi.Models.Entities.Membership
{
    public class AppUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
    }
}
