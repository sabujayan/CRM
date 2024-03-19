using System;
using System.Linq;
using System.Threading.Tasks;
using Indo.Products;
using Indo.Uoms;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.SalesDeliveryDetails
{
    public class SalesDeliveryDetailManager : DomainService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUomRepository _uomRepository;

        public SalesDeliveryDetailManager(
            IProductRepository productRepository,
            IUomRepository uomRepository
            )
        {
            _productRepository = productRepository;
            _uomRepository = uomRepository;
        }
        public async Task<SalesDeliveryDetail> CreateAsync(
            [NotNull] Guid salesDeliveryId,
            [NotNull] Guid productId,
            [NotNull] float quantity
            )
        {
            await Task.Yield();

            Check.NotNull<Guid>(salesDeliveryId, nameof(salesDeliveryId));
            Check.NotNull<Guid>(productId, nameof(productId));
            Check.NotNull<float>(quantity, nameof(quantity));

            var salesDeliveryDetail = new SalesDeliveryDetail(
                GuidGenerator.Create(),
                salesDeliveryId,
                productId,
                quantity
            );

            var product = _productRepository.Where(x => x.Id.Equals(productId)).FirstOrDefault();
            var uom = _uomRepository.Where(x => x.Id.Equals(product.UomId)).FirstOrDefault();
            salesDeliveryDetail.UomName = uom.Name;

            return salesDeliveryDetail;
        }
    }
}
