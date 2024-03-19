using System;
using Volo.Abp.Application.Dtos;

namespace Indo.Customers
{
    public class LeadRatingLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
