using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Indo.Clientes
{
    public class ClientsReadDto : AuditedEntityDto<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid ClientsId { get; set; }
        public Guid AddressId { get; set; }
        public string Projectnameist { get; set; }
        public string ProjectDesclist { get; set; }
        public Guid ContactId { get; set; }
        public List<string> ClientProjectId { get; set; }
        public string AddressDesc { get; set; }
    }
}
