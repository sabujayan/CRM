using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Indo.EmailsTemplates
{
    public class EmailsTemplatesLookUpDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}