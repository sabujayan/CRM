using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.SalesQuotationDetails
{
    public interface ISalesQuotationDetailRepository : IRepository<SalesQuotationDetail, Guid>
    {
    }
}
