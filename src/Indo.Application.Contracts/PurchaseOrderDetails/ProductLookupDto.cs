using System;
using Volo.Abp.Application.Dtos;

namespace Indo.PurchaseOrderDetails
{
    public class ProductLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public float Price { get; set; }
    }
}
