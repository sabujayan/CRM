using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.ServiceQuotationDetails
{
    public interface IServiceQuotationDetailRepository : IRepository<ServiceQuotationDetail, Guid>
    {
    }
}
