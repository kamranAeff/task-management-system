using DockerizeTaskManagementApi.Models.Entities.Membership;

namespace DockerizeTaskManagementApi.Models.Entities
{
    public class TaskItemUserCollection
    {
        public int TaskItemId { get; set; }
        public virtual TaskItem TaskItem { get; set; }
        public int UserId { get; set; }
        public virtual AppUser User { get; set; }
    }
}
