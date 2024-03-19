using System;
using Volo.Abp.Application.Dtos;

namespace Indo.Companies
{
    public class CurrencyLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
