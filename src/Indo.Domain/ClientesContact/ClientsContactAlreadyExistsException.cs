using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Indo.ClientesContact
{
    public class ClientsContactAlreadyExistsException : BusinessException
    {
        public ClientsContactAlreadyExistsException(string name)
          : base("ClientsContactAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
