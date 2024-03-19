using System;
using Volo.Abp.Application.Dtos;

namespace Indo.PurchaseOrders
{
    public class VendorLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
