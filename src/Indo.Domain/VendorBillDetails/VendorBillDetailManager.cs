using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.VendorBillDetails
{
    public class VendorBillDetailManager : DomainService
    {

        public VendorBillDetailManager()
        {
        }
        public async Task<VendorBillDetail> CreateAsync(
            [NotNull] Guid vendorBillId,
            [NotNull] string productName,
            [NotNull] Guid uomId,
            [NotNull] float price,
            [NotNull] float taxRate,
            [NotNull] float quantity,
            [NotNull] float discAmt
            )
        {
            await Task.Yield();

            Check.NotNull<Guid>(vendorBillId, nameof(vendorBillId));
            Check.NotNull<Guid>(uomId, nameof(uomId));
            Check.NotNullOrWhiteSpace(productName, nameof(productName));
            Check.NotNull<float>(quantity, nameof(quantity));
            Check.NotNull<float>(price, nameof(price));
            Check.NotNull<float>(taxRate, nameof(taxRate));

            var vendorBillDetail = new VendorBillDetail(
                GuidGenerator.Create(),
                vendorBillId,
                productName,
                uomId,
                price,
                taxRate,
                quantity,
                discAmt
            );

            vendorBillDetail.Recalculate();

            return vendorBillDetail;
        }
    }
}
