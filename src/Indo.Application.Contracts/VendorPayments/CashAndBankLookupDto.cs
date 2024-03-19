using System;
using Volo.Abp.Application.Dtos;

namespace Indo.VendorPayments
{
    public class CashAndBankLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
