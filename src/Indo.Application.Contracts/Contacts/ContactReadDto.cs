using System;
using Volo.Abp.Application.Dtos;

namespace Indo.Contacts
{
    public class ContactReadDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
    }
}
