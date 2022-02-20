using System.Collections.Generic;

namespace DockerizeTaskManagementApi.AppCode.Modules.AccountModule.Mapper.Dtos
{
    public class AccountInfoDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string VisibleName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        public OrganisationDto[] Organisations { get; set; }
        public bool IsSuperAdmin { get; set; }
        public bool IsOrganisationAdmin { get; set; }
        public bool IsUser { get; set; }
    }
}
