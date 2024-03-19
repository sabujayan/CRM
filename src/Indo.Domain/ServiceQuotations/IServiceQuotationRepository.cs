using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.ServiceQuotations
{
    public interface IServiceQuotationRepository : IRepository<ServiceQuotation, Guid>
    {
    }
}
