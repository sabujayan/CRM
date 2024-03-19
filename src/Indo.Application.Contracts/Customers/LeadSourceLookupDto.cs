using System;
using Volo.Abp.Application.Dtos;

namespace Indo.Customers
{
    public class LeadSourceLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
