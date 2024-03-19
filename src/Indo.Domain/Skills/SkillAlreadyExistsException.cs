using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.Skills
{
    public class SkillAlreadyExistsException: BusinessException
    {
        public SkillAlreadyExistsException(string name)
            : base("SkillAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
