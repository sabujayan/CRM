using System;
using Volo.Abp.Application.Dtos;

namespace Indo.PurchaseReceipts
{
    public class PurchaseOrderLookupDto : EntityDto<Guid>
    {
        public string Number { get; set; }
    }
}
