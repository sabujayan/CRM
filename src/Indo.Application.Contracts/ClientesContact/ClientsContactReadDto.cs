using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Indo.ClientesContact
{
    public class ClientsContactReadDto : AuditedEntityDto<Guid>
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid ClientsId { get; set; }
    }
}
