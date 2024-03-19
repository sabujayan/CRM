using System;
using Volo.Abp.Application.Dtos;

namespace Indo.VendorBillDetails
{
    public class VendorBillLookupDto : EntityDto<Guid>
    {
        public string Number { get; set; }
    }
}
