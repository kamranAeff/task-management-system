using DockerizeTaskManagementApi.Models.Entities.Membership;
using System;
using System.Collections.Generic;

namespace DockerizeTaskManagementApi.Models.Entities
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public TaskItemStatus Status { get; set; }
        public TaskItemPriority Priority { get; set; }
        public int CreatedByUserId { get; set; }
        public virtual AppUser CreatedByUser { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(4);
        public int TaskBoardId { get; set; }
        public virtual TaskBoard TaskBoard { get; set; }
        public virtual ICollection<TaskItemUserCollection> MappedUsers { get; set; }
    }

    public enum TaskItemStatus
    {
        None = 0,
        New,
        Complated
    }

    public enum TaskItemPriority
    {
        None = 0,
        Low,
        Normal,
        High
    }
}
