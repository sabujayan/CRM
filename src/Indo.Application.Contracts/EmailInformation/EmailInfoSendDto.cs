using System;
using System.Collections.Generic;
using System.Text;

namespace Indo.EmailInformation
{
    public class EmailInfoSendDto
    {
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public Guid TemplateId { get; set; }
    }
}
