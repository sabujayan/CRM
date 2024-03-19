using System;
using Volo.Abp.Application.Dtos;

namespace Indo.Companies
{
    public class CompanyReadDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public Guid CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public Guid DefaultWarehouseId { get; set; }
        public string DefaultWarehouseName { get; set; }
    }
}
