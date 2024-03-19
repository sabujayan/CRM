using System;
using Volo.Abp.Application.Dtos;

namespace Indo.ServiceQuotationDetails
{
    public class ServiceQuotationLookupDto : EntityDto<Guid>
    {
        public string Number { get; set; }
    }
}
