using Microsoft.AspNetCore.Identity;

namespace DockerizeTaskManagementApi.AppCode.Providers
{
    public class AppIdentityErrorProvider : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = $"'{email}' bu email artıq artıq mövcuddur."
            };
        }
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = $"'{userName}' bu istifadəçi adı artıq mövcuddur."
            };
        }
    }
}
