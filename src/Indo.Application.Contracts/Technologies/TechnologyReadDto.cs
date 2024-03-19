using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Indo.Technologies
{
    public class TechnologyReadDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ParentId { get; set; }
        public string ParentStatus { get; set; }
    }
}
