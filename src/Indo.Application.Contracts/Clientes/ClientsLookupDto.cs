using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Indo.Clientes
{
    public class ClientsLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
