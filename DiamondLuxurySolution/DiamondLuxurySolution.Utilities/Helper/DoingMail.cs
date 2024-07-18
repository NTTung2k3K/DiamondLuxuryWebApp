using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using static DiamondLuxurySolution.Utilities.Helper.Settings;
using Microsoft.Extensions.Configuration;

namespace DiamondLuxurySolution.Utilities.Helper
{
    public class DoingMail
    {
        private static readonly string _email;
        private static readonly string _password;

        static DoingMail()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var configPath = Path.Combine(basePath, @"..\..\..\appsettings.json");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(configPath))
                .AddJsonFile(Path.GetFileName(configPath), optional: false, reloadOnChange: true);

            var config = builder.Build();

            _email = config["Settings:EmailSettings:Email"];
            _password = config["Settings:EmailSettings:Password"];
        }



        public static bool SendMail(string name, string subject, string content, string toMail)
        {
            bool rs = false;
            try
            {
                MailMessage message = new MailMessage();
                var smtp = new System.Net.Mail.SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com"; //host name
                    smtp.Port = 587; //port number
                    smtp.EnableSsl = true; //whether your smt server requires SSL smtp.DeliveryMethod = System.Net .Mail.SmtpDeliveryMethod.Network;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential()
                    {
                        UserName = _email,
                        Password = _password
                    };

                }

                MailAddress fromAddress = new MailAddress(_email, name);
                message.From = fromAddress;
                message.To.Add(toMail);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = content;
                smtp.Send(message);
                rs = true;

            }
            catch (Exception e)
            {
                rs = false;
            }
            return rs;
        }

    }
}
