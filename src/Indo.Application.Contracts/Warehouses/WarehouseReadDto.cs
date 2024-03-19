using System;
using Volo.Abp.Application.Dtos;

namespace Indo.Warehouses
{
    public class WarehouseReadDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool DefaultConfig { get; set; }
        public bool Virtual { get; set; }
    }
}
