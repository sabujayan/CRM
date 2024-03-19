using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Indo.EmailsAttachments
{
    public class EmailAttachmentAlreadyExistsException : BusinessException
    {
        public EmailAttachmentAlreadyExistsException(string name)
          : base("EmailAttachmentAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
