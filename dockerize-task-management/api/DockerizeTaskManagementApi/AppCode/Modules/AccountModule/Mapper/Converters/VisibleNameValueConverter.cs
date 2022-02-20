using AutoMapper;
using DockerizeTaskManagementApi.Models.Entities.Membership;

namespace DockerizeTaskManagementApi.AppCode.Modules.AccountModule.Mapper.Converters
{
    public class VisibleNameValueConverter : IValueConverter<AppUser, string>
    {
        public string Convert(AppUser user, ResolutionContext context)
        {
            if (user == null)
                return null;

            if (!string.IsNullOrWhiteSpace(user.Name) && !string.IsNullOrWhiteSpace(user.Surname))
                return $"{user.Name} {user.Surname}";
            else if (!string.IsNullOrWhiteSpace(user.Name))
                return user.Name;
            else if (!string.IsNullOrWhiteSpace(user.Email))
                return user.Email;
            else if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
                return $"@{user.PhoneNumber}";
            else if (!string.IsNullOrWhiteSpace(user.UserName))
                return $"@{user.UserName}";

            return null;
        }
    }
}
