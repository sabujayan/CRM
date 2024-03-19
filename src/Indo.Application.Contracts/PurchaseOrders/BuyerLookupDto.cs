using System;
using Volo.Abp.Application.Dtos;

namespace Indo.PurchaseOrders
{
    public class BuyerLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
