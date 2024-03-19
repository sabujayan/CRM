using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Indo.EmailInformation
{
    public interface IEmailSender
    {
        Task SendAsync(MailMessage mailMessage);
    }
}
