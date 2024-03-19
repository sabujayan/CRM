using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.CustomerInvoiceDetails
{
    public class CustomerInvoiceDetailManager : DomainService
    {

        public CustomerInvoiceDetailManager()
        {
        }
        public async Task<CustomerInvoiceDetail> CreateAsync(
            [NotNull] Guid customerInvoiceId,
            [NotNull] string productName,
            [NotNull] Guid uomId,
            [NotNull] float price,
            [NotNull] float taxRate,
            [NotNull] float quantity,
            [NotNull] float discAmt
            )
        {
            await Task.Yield();

            Check.NotNull<Guid>(customerInvoiceId, nameof(customerInvoiceId));
            Check.NotNull<Guid>(uomId, nameof(uomId));
            Check.NotNullOrWhiteSpace(productName, nameof(productName));
            Check.NotNull<float>(quantity, nameof(quantity));
            Check.NotNull<float>(price, nameof(price));
            Check.NotNull<float>(taxRate, nameof(taxRate));

            var customerInvoiceDetail = new CustomerInvoiceDetail(
                GuidGenerator.Create(),
                customerInvoiceId,
                productName,
                uomId,
                price,
                taxRate,
                quantity,
                discAmt
            );

            customerInvoiceDetail.Recalculate();

            return customerInvoiceDetail;
        }
    }
}
