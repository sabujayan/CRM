using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.CustomerCreditNoteDetails
{
    public class CustomerCreditNoteDetailManager : DomainService
    {

        public CustomerCreditNoteDetailManager()
        {
        }
        public async Task<CustomerCreditNoteDetail> CreateAsync(
            [NotNull] Guid customerCreditNoteId,
            [NotNull] string productName,
            [NotNull] Guid uomId,
            [NotNull] float price,
            [NotNull] float taxRate,
            [NotNull] float quantity,
            [NotNull] float discAmt
            )
        {
            await Task.Yield();

            Check.NotNull<Guid>(customerCreditNoteId, nameof(customerCreditNoteId));
            Check.NotNull<Guid>(uomId, nameof(uomId));
            Check.NotNullOrWhiteSpace(productName, nameof(productName));
            Check.NotNull<float>(quantity, nameof(quantity));
            Check.NotNull<float>(price, nameof(price));
            Check.NotNull<float>(taxRate, nameof(taxRate));

            var customerCreditNoteDetail = new CustomerCreditNoteDetail(
                GuidGenerator.Create(),
                customerCreditNoteId,
                productName,
                uomId,
                price,
                taxRate,
                quantity,
                discAmt
            );

            customerCreditNoteDetail.Recalculate();

            return customerCreditNoteDetail;
        }
    }
}
