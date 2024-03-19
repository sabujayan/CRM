using System;
using Volo.Abp.Application.Dtos;

namespace Indo.LeadRatings
{
    public class LeadRatingReadDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
