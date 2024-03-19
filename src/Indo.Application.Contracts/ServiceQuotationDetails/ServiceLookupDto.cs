using System;
using Volo.Abp.Application.Dtos;

namespace Indo.ServiceQuotationDetails
{
    public class ServiceLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public float Price { get; set; }
    }
}
