using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Indo.Clientes
{
    public class ClientsAlreadyExistsException : BusinessException
    {
        public ClientsAlreadyExistsException(string name)
           : base("ClientsAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
