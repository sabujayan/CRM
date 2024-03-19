using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Indo.ClientsProjects
{
    public class ClientsProjectsMatricesMatricesAlreadyExistsException : BusinessException
    {
        public ClientsProjectsMatricesMatricesAlreadyExistsException(string name)
        : base("ClientsProjectsAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
