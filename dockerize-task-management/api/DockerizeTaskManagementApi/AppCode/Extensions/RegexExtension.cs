using System.Text.RegularExpressions;

namespace DockerizeTaskManagementApi.AppCode.Extensions
{
    static public partial class Extension
    {
        static public bool IsEmail(this string value)
        {
            bool success = Regex.IsMatch(value, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

            return success;
        }

        static public bool IsPhone(this string value)
        {
            bool success = Regex.IsMatch(value, @"^(\+994|0)(50|51|55|70|77|99|10)-(\d{3}-\d{2}-\d{2})$");

            return success;
        }
    }
}
