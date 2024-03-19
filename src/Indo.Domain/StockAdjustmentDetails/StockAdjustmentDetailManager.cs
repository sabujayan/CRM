using System;
using System.Linq;
using System.Threading.Tasks;
using Indo.Products;
using Indo.Uoms;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.StockAdjustmentDetails
{
    public class StockAdjustmentDetailManager : DomainService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUomRepository _uomRepository;

        public StockAdjustmentDetailManager(
            IProductRepository productRepository,
            IUomRepository uomRepository
            )
        {
            _productRepository = productRepository;
            _uomRepository = uomRepository;
        }
        public async Task<StockAdjustmentDetail> CreateAsync(
            [NotNull] Guid stockAdjustmentId,
            [NotNull] Guid productId,
            [NotNull] float quantity
            )
        {
            await Task.Yield();

            Check.NotNull<Guid>(stockAdjustmentId, nameof(stockAdjustmentId));
            Check.NotNull<Guid>(productId, nameof(productId));
            Check.NotNull<float>(quantity, nameof(quantity));

            var stockAdjustmentDetail = new StockAdjustmentDetail(
                GuidGenerator.Create(),
                stockAdjustmentId,
                productId,
                quantity
            );

            var product = _productRepository.Where(x => x.Id.Equals(productId)).FirstOrDefault();
            var uom = _uomRepository.Where(x => x.Id.Equals(product.UomId)).FirstOrDefault();
            stockAdjustmentDetail.UomName = uom.Name;

            return stockAdjustmentDetail;
        }
    }
}
