using System;
using System.Collections.Generic;
using System.Text;

namespace Indo.EmailInformation
{
    public class EmailDto
    {
        public List<string> To { get; set; }
        public List<string> Cc { get; set; }
        public List<string> Bcc { get; set; }
        public Guid TemplateId { get; set; }
    }

}
