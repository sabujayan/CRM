using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.NumberSequences
{
    public interface INumberSequenceRepository : IRepository<NumberSequence, Guid>
    {
    }
}
