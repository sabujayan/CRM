using System;
using Volo.Abp.Application.Dtos;

namespace Indo.GoodsReceipts
{
    public class DeliveryOrderLookupDto : EntityDto<Guid>
    {
        public string Number { get; set; }
    }
}
