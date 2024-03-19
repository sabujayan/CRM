using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.LeadsAddress
{
    public interface ILeadAddressMatrixRepository : IRepository<LeadsAddressMatrix, Guid>
    {
    }
}
