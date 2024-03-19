using System;
using System.Linq;
using System.Threading.Tasks;
using Indo.Products;
using Indo.Uoms;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.DeliveryOrderDetails
{
    public class DeliveryOrderDetailManager : DomainService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUomRepository _uomRepository;

        public DeliveryOrderDetailManager(
            IProductRepository productRepository,
            IUomRepository uomRepository
            )
        {
            _productRepository = productRepository;
            _uomRepository = uomRepository;
        }
        public async Task<DeliveryOrderDetail> CreateAsync(
            [NotNull] Guid deliveryOrderId,
            [NotNull] Guid productId,
            [NotNull] float quantity
            )
        {
            await Task.Yield();

            Check.NotNull<Guid>(deliveryOrderId, nameof(deliveryOrderId));
            Check.NotNull<Guid>(productId, nameof(productId));
            Check.NotNull<float>(quantity, nameof(quantity));

            var deliveryOrderDetail = new DeliveryOrderDetail(
                GuidGenerator.Create(),
                deliveryOrderId,
                productId,
                quantity
            );

            var product = _productRepository.Where(x => x.Id.Equals(productId)).FirstOrDefault();
            var uom = _uomRepository.Where(x => x.Id.Equals(product.UomId)).FirstOrDefault();
            deliveryOrderDetail.UomName = uom.Name;

            return deliveryOrderDetail;
        }
    }
}
