using System;
using Volo.Abp.Application.Dtos;

namespace Indo.Uoms
{
    public class UomReadDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
