using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Indo.Skills
{
    public class SkillLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
