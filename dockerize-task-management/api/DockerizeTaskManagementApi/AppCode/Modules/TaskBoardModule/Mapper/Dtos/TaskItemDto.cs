using DockerizeTaskManagementApi.AppCode.Modules.AccountModule.Mapper.Dtos;

namespace DockerizeTaskManagementApi.AppCode.Modules.TaskBoardModule.Mapper.Dtos
{
    public class TaskItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Deadline { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public int AuthorId { get; set; }
        public string Author { get; set; }
        public string CreatedDate { get; set; }
        public UserChooseDto[] Users { get; set; }
    }
}
