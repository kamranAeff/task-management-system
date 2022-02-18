using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DockerizeTaskManagementApi.Models.Entities.Membership
{
    public class AppUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public int? OrganisationId { get; set; }
        public virtual Organisation Organisation { get; set; }
        public virtual ICollection<TaskItemUserCollection> MappedTaskItems { get; set; }
    }
}
