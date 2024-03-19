using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.SalesQuotations
{
    public interface ISalesQuotationRepository : IRepository<SalesQuotation, Guid>
    {
    }
}
