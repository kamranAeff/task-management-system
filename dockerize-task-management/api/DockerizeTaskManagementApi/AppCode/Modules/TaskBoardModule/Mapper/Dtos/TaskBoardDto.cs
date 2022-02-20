using DockerizeTaskManagementApi.AppCode.Modules.AccountModule.Mapper.Dtos;

namespace DockerizeTaskManagementApi.AppCode.Modules.TaskBoardModule.Mapper.Dtos
{
    public class TaskBoardDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public string Author { get; set; }
        public string CreatedDate { get; set; }
        public int OrganisationId { get; set; }
        public OrganisationDto Organisation { get; set; }
        public TaskItemDto[] Tasks { get; set; }
    }
}
