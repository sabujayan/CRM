using System;
using Volo.Abp.Application.Dtos;

namespace Indo.Currencies
{
    public class CurrencyReadDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
    }
}
