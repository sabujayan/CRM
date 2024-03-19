using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.LeadRatings
{
    public interface ILeadRatingRepository : IRepository<LeadRating, Guid>
    {
    }
}
