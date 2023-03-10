using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Common.Mail
{
    public class Email
    {
        public static string Address = "SenderFSVN@gmail.com"; //Địa chỉ email của bạn
        public static string Password = "kcpcubvavoyljcsg"; //Mật khẩu ứng dụng /
        public void SendEmail(string sendTo, string subject, string message, string Pathfile)
        {
            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential(Address, Password);
                MailMessage mm = new MailMessage(Address, sendTo, subject, message);
                mm.Attachments.Add(new Attachment(Pathfile));
                smtp.Send(mm);
            }
        }
    }
}