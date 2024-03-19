using System;
using Volo.Abp.Application.Dtos;

namespace Indo.Movements
{
    public class WarehouseLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
