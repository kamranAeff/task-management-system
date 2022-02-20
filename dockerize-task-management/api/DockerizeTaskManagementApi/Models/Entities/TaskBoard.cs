using DockerizeTaskManagementApi.Models.Entities.Membership;
using System;
using System.Collections.Generic;

namespace DockerizeTaskManagementApi.Models.Entities
{
    public class TaskBoard
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CreatedByUserId { get; set; }
        public virtual AppUser CreatedByUser { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(4);
        public int OrganisationId { get; set; }
        public virtual Organisation Organisation { get; set; }
        public virtual ICollection<TaskItem> Tasks { get; set; }
    }


}
