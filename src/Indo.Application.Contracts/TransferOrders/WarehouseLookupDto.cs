using System;
using Volo.Abp.Application.Dtos;

namespace Indo.TransferOrders
{
    public class WarehouseLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
