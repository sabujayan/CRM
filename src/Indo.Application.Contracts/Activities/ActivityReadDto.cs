using System;
using Volo.Abp.Application.Dtos;

namespace Indo.Activities
{
    public class ActivityReadDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
