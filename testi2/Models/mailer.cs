using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace testi2.Models
{
    public class mailer
    {
        public bool sendEmail(mailerDetail obj) {

            string your_id = "@gmail.com";
            string your_password = "";
            bool answer = false;
            try
            {
                SmtpClient client = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(your_id, your_password),
                    Timeout = 10000,
                };
                MailMessage mm = new MailMessage(obj.mailerName, obj.mailerEmail, obj.mailerSubject, obj.mailerBody);
                client.Send(mm);
                //Console.WriteLine("Email Sent");
                answer = true;
            }
            catch (Exception e)
            {
                //Console.WriteLine("Could not end email\n\n" + e.ToString());
                answer = true;
            }

            return answer;
        }
    }

    public class mailerDetail
    {
        public string mailerName { get; set; }
        public string mailerEmail { get; set; }
        public string mailerBody { get; set; }
        public string mailerSubject { get; set; }
    }
}