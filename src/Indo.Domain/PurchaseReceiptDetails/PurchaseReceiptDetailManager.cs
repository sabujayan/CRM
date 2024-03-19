using System;
using System.Linq;
using System.Threading.Tasks;
using Indo.Products;
using Indo.Uoms;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.PurchaseReceiptDetails
{
    public class PurchaseReceiptDetailManager : DomainService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUomRepository _uomRepository;

        public PurchaseReceiptDetailManager(
            IProductRepository productRepository,
            IUomRepository uomRepository
            )
        {
            _productRepository = productRepository;
            _uomRepository = uomRepository;
        }
        public async Task<PurchaseReceiptDetail> CreateAsync(
            [NotNull] Guid purchaseReceiptId,
            [NotNull] Guid productId,
            [NotNull] float quantity
            )
        {
            await Task.Yield();

            Check.NotNull<Guid>(purchaseReceiptId, nameof(purchaseReceiptId));
            Check.NotNull<Guid>(productId, nameof(productId));
            Check.NotNull<float>(quantity, nameof(quantity));

            var purchaseReceiptDetail = new PurchaseReceiptDetail(
                GuidGenerator.Create(),
                purchaseReceiptId,
                productId,
                quantity
            );

            var product = _productRepository.Where(x => x.Id.Equals(productId)).FirstOrDefault();
            var uom = _uomRepository.Where(x => x.Id.Equals(product.UomId)).FirstOrDefault();
            purchaseReceiptDetail.UomName = uom.Name;

            return purchaseReceiptDetail;
        }
    }
}
