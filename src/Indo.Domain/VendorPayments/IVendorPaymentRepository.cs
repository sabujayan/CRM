using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.VendorPayments
{
    public interface IVendorPaymentRepository : IRepository<VendorPayment, Guid>
    {
    }
}
