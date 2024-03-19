using System;
using Volo.Abp.Application.Dtos;

namespace Indo.VendorDebitNotes
{
    public class VendorBillLookupDto : EntityDto<Guid>
    {
        public string Number { get; set; }
    }
}
