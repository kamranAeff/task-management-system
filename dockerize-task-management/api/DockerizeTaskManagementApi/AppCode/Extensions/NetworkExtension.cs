using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DockerizeTaskManagementApi.AppCode.Extensions
{
    static public partial class Extension
    {
        static async public Task<Tuple<bool, string>> SendEmailAsync(this IConfiguration configration, string subject, string body,  params string[] to)
        {
            try
            {

                if (!int.TryParse(configration["emailAccount:smtpPort"], out int port))
                    port = 25;

                using (var client = new SmtpClient(configration["emailAccount:smtpServer"], port)
                {
                    Credentials = new NetworkCredential(configration["emailAccount:userName"], configration["emailAccount:password"]),
                    EnableSsl = true
                })
                using (var message = new MailMessage(new MailAddress(configration["emailAccount:userName"], configration["emailAccount:displayName"]), new MailAddress(to[0]))
                {
                    Subject = subject,
                    IsBodyHtml = true,
                    Body = body
                })
                {
                    for (int i = 1; i < to.Length; i++)
                        message.To.Add(to[i]);

                    client.Send(message);                }
            }
            catch (Exception ex)
            {
                return Tuple.Create(true, "Xəta baş verdi zəhmət olmasa sistem inzibatçılarına bildirin");
            }

            return Tuple.Create(false, "Email uğurla göndərildi");
        }
    }
}
