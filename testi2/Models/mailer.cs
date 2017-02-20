using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace testi2.Models
{
    public class mailer
    {
        public String sendEmail(mailerDetail obj) {

            string your_id = "@gmail.com";
            string your_password = "";
            string myName = "Andres Mora";
            String answer = "noSend";
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
                //MailMessage mm = new MailMessage(obj.mailerEmail, obj.mailerName, obj.mailerSubject, obj.mailerBody);
                //client.Send(mm);
                ////Console.WriteLine("Email Sent");
                //answer = "yes, sended";

                // add from,to mailaddresses
                MailAddress from = new MailAddress(your_id, myName);
                MailAddress to = new MailAddress(obj.mailerEmail, obj.mailerName);
                MailMessage myMail = new System.Net.Mail.MailMessage(from, to);

                // add ReplyTo
                MailAddress replyTo = new MailAddress(your_id);
                myMail.ReplyToList.Add(replyTo);

                // set subject and encoding
                myMail.Subject = obj.mailerSubject;
                myMail.SubjectEncoding = System.Text.Encoding.UTF8;

                // set body-message and encoding
                myMail.Body = obj.mailerBody;
                myMail.BodyEncoding = System.Text.Encoding.UTF8;
                // text or html
                myMail.IsBodyHtml = true;

                client.Send(myMail);
                answer = "Sended";

            }
            catch (Exception e)
            {
                //Console.WriteLine("Could not end email\n\n" + e.ToString());
                answer = e.ToString();
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