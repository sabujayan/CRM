using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Indo.ProjectsTechnologies
{
    public class ProjectsTechnologyMatricesAlreadyExistsException : BusinessException
    {
        public ProjectsTechnologyMatricesAlreadyExistsException(string name)
        : base("TechnologyProjectsAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
