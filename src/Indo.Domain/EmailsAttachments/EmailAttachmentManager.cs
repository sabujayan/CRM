using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Indo.EmailsAttachments
{
    public class EmailAttachmentManager : DomainService
    {
        private readonly IEmailAttachmentRepository _emailattachmentRepository;

        public EmailAttachmentManager(IEmailAttachmentRepository emailattachmentRepository)
        {
            _emailattachmentRepository = emailattachmentRepository;
        }
    }
}
