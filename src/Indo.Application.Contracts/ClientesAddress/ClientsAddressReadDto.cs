using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Indo.ClientesAddress
{
    public class ClientsAddressReadDto : AuditedEntityDto<Guid>
    {
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public Guid ClientsId { get; set; }
    }
}
