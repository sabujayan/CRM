using System;
using Volo.Abp.Application.Dtos;

namespace Indo.Notes
{
    public class CustomerLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
