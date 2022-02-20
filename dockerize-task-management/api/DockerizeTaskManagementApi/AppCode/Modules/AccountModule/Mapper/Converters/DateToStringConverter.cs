using AutoMapper;
using System;

namespace DockerizeTaskManagementApi.AppCode.Modules.AccountModule.Mapper.Converters
{
    public class DateToStringConverter : IValueConverter<DateTime?, string>
    {
        string format;
        public DateToStringConverter(string format = null)
        {
            this.format = format;
        }

        public string Convert(DateTime? sourceMember, ResolutionContext context)
        {
            if (sourceMember.HasValue)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(this.format))
                        return sourceMember?.ToString(this.format);

                    var format = context.Items["dateTimeOutFormat"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(format))
                        return sourceMember?.ToString(format);
                }
                catch { }

                return sourceMember?.ToString("yyyy-MM-ddTHH:mm:dd.fffff");
            }

            return null;
        }
    }
}
