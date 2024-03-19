using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Indo.ClientesAddress
{
    public class ClientsAddressAlreadyExistsException : BusinessException
    {
        public ClientsAddressAlreadyExistsException(Guid id)
          : base("ClientsAddressAlreadyExists")
        {
            WithData("id", id);
        }
    }
}
