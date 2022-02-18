using DockerizeTaskManagementApi.Models.Entities.Membership;
using System.Collections.Generic;

namespace DockerizeTaskManagementApi.Models.Entities
{
    public class Organisation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public virtual ICollection<AppUser> Users { get; set; }
    }
}
