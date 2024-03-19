using System;
using Volo.Abp.Application.Dtos;

namespace Indo.SalesQuotations
{
    public class SalesExecutiveLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
