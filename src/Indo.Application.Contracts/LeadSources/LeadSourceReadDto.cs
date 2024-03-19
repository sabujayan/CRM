using System;
using Volo.Abp.Application.Dtos;

namespace Indo.LeadSources
{
    public class LeadSourceReadDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
