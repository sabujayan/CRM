using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Indo.Technologies
{
    public class TechnologyLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
