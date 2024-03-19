using System;
using Volo.Abp.Application.Dtos;

namespace Indo.CustomerCreditNotes
{
    public class CustomerInvoiceLookupDto : EntityDto<Guid>
    {
        public string Number { get; set; }
    }
}
