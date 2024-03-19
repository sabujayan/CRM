using System;
using Volo.Abp.Application.Dtos;

namespace Indo.Resources
{
    public class ResourceReadDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
