using System;
using Volo.Abp.Application.Dtos;

namespace Indo.Tasks
{
    public class ActivityLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
