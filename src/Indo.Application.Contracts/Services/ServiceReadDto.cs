using System;
using Volo.Abp.Application.Dtos;

namespace Indo.Services
{
    public class ServiceReadDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public string CurrencyName { get; set; }
        public float TaxRate { get; set; }
        public Guid UomId { get; set; }
        public string UomName { get; set; }
    }
}
