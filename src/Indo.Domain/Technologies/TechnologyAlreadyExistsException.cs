using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Indo.Technologies
{
    public class TechnologyAlreadyExistsException : BusinessException
    {
        public TechnologyAlreadyExistsException(string name)
              : base("TechnologyAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
