using System;
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

        static public string ClearPhone(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            var match = Regex.Match(value, @"^(\+994|0)(?<prefix>12|10|50|51|55|70|77|90)-?((?<part1>\d{3})-?(?<part2>\d{2})-?(?<part3>\d{2}))$");

            if (!match.Success)
                return value;

            return $"+994{match.Groups["prefix"].Value}{match.Groups["part1"].Value}{match.Groups["part2"].Value}{match.Groups["part3"].Value}";
        }

        static public Match ReleaseComplateSignupToken(this string value)
        {
            return Regex.Match(value, @"^(?<id>\d+)#(?<email>[^#\s]*)#(?<pwd>[^#\s]*)#$");
        }

        static public T Read<T>(this Match match ,string key)
        {
            var value = match.Groups[key].Value;

            try
            {
                return (T)Convert.ChangeType(value,typeof(T));
            }
            catch (Exception)
            {
                return default;
            }

        }
    }
}
