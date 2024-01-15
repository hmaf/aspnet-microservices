using Ordering.Application.Contracts.Infrastracture;
using Ordering.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastracture.Mail
{
    public class MailService : IEmailService
    {
        public async Task<bool> SendEmail(Email email)
        {
            return true;
        }
    }
}
