using System;
using Volo.Abp.Application.Dtos;

namespace Indo.CustomerInvoices
{
    public class CustomerLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
