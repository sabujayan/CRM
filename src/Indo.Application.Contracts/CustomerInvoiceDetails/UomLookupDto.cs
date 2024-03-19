using System;
using Volo.Abp.Application.Dtos;

namespace Indo.CustomerInvoiceDetails
{
    public class UomLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
