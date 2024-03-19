using System;
using Volo.Abp.Application.Dtos;

namespace Indo.Bookings
{
    public class ResourceLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
