using System;
using Volo.Abp.Application.Dtos;

namespace Indo.PurchaseOrderDetails
{
    public class PurchaseOrderLookupDto : EntityDto<Guid>
    {
        public string Number { get; set; }
    }
}
