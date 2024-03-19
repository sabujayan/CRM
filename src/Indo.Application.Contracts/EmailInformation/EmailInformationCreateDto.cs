using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;

namespace Indo.EmailInformation
{
    public class EmailInformationCreateDto
    {
        public Guid EmployeeId { get; set; }
        public Guid TemplateId { get; set; }
    }
}
