using System;
using System.Linq;
using System.Threading.Tasks;
using Indo.Products;
using Indo.Uoms;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.GoodsReceiptDetails
{
    public class GoodsReceiptDetailManager : DomainService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUomRepository _uomRepository;

        public GoodsReceiptDetailManager(
            IProductRepository productRepository,
            IUomRepository uomRepository
            )
        {
            _productRepository = productRepository;
            _uomRepository = uomRepository;
        }
        public async Task<GoodsReceiptDetail> CreateAsync(
            [NotNull] Guid goodsReceiptId,
            [NotNull] Guid productId,
            [NotNull] float quantity
            )
        {
            await Task.Yield();

            Check.NotNull<Guid>(goodsReceiptId, nameof(goodsReceiptId));
            Check.NotNull<Guid>(productId, nameof(productId));
            Check.NotNull<float>(quantity, nameof(quantity));

            var goodsReceiptDetail = new GoodsReceiptDetail(
                GuidGenerator.Create(),
                goodsReceiptId,
                productId,
                quantity
            );

            var product = _productRepository.Where(x => x.Id.Equals(productId)).FirstOrDefault();
            var uom = _uomRepository.Where(x => x.Id.Equals(product.UomId)).FirstOrDefault();
            goodsReceiptDetail.UomName = uom.Name;

            return goodsReceiptDetail;
        }
    }
}
