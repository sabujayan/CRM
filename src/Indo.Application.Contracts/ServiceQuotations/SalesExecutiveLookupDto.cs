using System;
using Volo.Abp.Application.Dtos;

namespace Indo.ServiceQuotations
{
    public class SalesExecutiveLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
