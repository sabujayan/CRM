using System;
using Volo.Abp.Application.Dtos;

namespace Indo.CustomerInvoiceDetails
{
    public class CustomerInvoiceLookupDto : EntityDto<Guid>
    {
        public string Number { get; set; }
    }
}
