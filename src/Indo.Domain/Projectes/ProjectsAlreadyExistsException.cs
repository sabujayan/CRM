using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Indo.Projectes
{
    public class ProjectsAlreadyExistsException : BusinessException
    {
        public ProjectsAlreadyExistsException(string name)
           : base("ProjectAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
