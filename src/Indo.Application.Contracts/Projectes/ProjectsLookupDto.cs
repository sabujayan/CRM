using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Indo.Projectes
{
    public class ProjectsLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
