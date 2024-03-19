using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Indo.Skills
{
    public class SkillReadDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
