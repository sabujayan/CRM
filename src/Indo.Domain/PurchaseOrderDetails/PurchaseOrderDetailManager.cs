using System;
using System.Linq;
using System.Threading.Tasks;
using Indo.Products;
using Indo.Uoms;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.PurchaseOrderDetails
{
    public class PurchaseOrderDetailManager : DomainService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUomRepository _uomRepository;

        public PurchaseOrderDetailManager(
            IProductRepository productRepository,
            IUomRepository uomRepository
            )
        {
            _productRepository = productRepository;
            _uomRepository = uomRepository;
        }
        public async Task<PurchaseOrderDetail> CreateAsync(
            [NotNull] Guid purchaseOrderId,
            [NotNull] Guid productId,
            [NotNull] float quantity,
            [NotNull] float discAmt
            )
        {
            await Task.Yield();

            Check.NotNull<Guid>(purchaseOrderId, nameof(purchaseOrderId));
            Check.NotNull<Guid>(productId, nameof(productId));
            Check.NotNull<float>(quantity, nameof(quantity));

            var purchaseOrderDetail = new PurchaseOrderDetail(
                GuidGenerator.Create(),
                purchaseOrderId,
                productId,
                quantity,
                discAmt
            );

            var product = _productRepository.Where(x => x.Id.Equals(productId)).FirstOrDefault();

            purchaseOrderDetail.Price = product.Price;
            purchaseOrderDetail.TaxRate = product.TaxRate;
            purchaseOrderDetail.Recalculate();

            var uom = _uomRepository.Where(x => x.Id.Equals(product.UomId)).FirstOrDefault();
            purchaseOrderDetail.UomName = uom.Name;

            return purchaseOrderDetail;
        }
    }
}
