using System;
using Volo.Abp.Application.Dtos;

namespace Indo.PurchaseReceiptDetails
{
    public class PurchaseReceiptLookupDto : EntityDto<Guid>
    {
        public string Number { get; set; }
    }
}
