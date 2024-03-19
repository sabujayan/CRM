using System;
using Volo.Abp.Application.Dtos;

namespace Indo.ProjectOrderDetails
{
    public class ProjectOrderLookupDto : EntityDto<Guid>
    {
        public string Number { get; set; }
    }
}
