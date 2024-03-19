using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.VendorDebitNoteDetails
{
    public class VendorDebitNoteDetailManager : DomainService
    {

        public VendorDebitNoteDetailManager()
        {
        }
        public async Task<VendorDebitNoteDetail> CreateAsync(
            [NotNull] Guid vendorDebitNoteId,
            [NotNull] string productName,
            [NotNull] Guid uomId,
            [NotNull] float price,
            [NotNull] float taxRate,
            [NotNull] float quantity,
            [NotNull] float discAmt
            )
        {
            await Task.Yield();

            Check.NotNull<Guid>(vendorDebitNoteId, nameof(vendorDebitNoteId));
            Check.NotNull<Guid>(uomId, nameof(uomId));
            Check.NotNullOrWhiteSpace(productName, nameof(productName));
            Check.NotNull<float>(quantity, nameof(quantity));
            Check.NotNull<float>(price, nameof(price));
            Check.NotNull<float>(taxRate, nameof(taxRate));

            var vendorDebitNoteDetail = new VendorDebitNoteDetail(
                GuidGenerator.Create(),
                vendorDebitNoteId,
                productName,
                uomId,
                price,
                taxRate,
                quantity,
                discAmt
            );

            vendorDebitNoteDetail.Recalculate();

            return vendorDebitNoteDetail;
        }
    }
}
