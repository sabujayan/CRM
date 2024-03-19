using System;
using Volo.Abp.Application.Dtos;

namespace Indo.SalesQuotationDetails
{
    public class SalesQuotationLookupDto : EntityDto<Guid>
    {
        public string Number { get; set; }
    }
}
