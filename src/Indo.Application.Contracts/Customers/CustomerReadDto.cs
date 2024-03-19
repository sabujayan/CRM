using System;
using Volo.Abp.Application.Dtos;

namespace Indo.Customers
{
    public class CustomerReadDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string RootFolder { get; set; }
        public CustomerStatus Status { get; set; }
        public Guid LeadSourceId { get; set; }
        public Guid LeadRatingId { get; set; }
        public CustomerStage Stage { get; set; }
        public string StageString { get; set; }
    }
}
