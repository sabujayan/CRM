using System;
using Volo.Abp.Application.Dtos;

namespace Indo.ImportantDates
{
    public class CustomerLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
