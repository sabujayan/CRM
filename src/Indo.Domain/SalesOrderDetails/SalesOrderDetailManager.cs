using System;
using System.Linq;
using System.Threading.Tasks;
using Indo.Products;
using Indo.Uoms;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.SalesOrderDetails
{
    public class SalesOrderDetailManager : DomainService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUomRepository _uomRepository;

        public SalesOrderDetailManager(
            IProductRepository productRepository,
            IUomRepository uomRepository
            )
        {
            _productRepository = productRepository;
            _uomRepository = uomRepository;
        }
        public async Task<SalesOrderDetail> CreateAsync(
            [NotNull] Guid salesOrderId,
            [NotNull] Guid productId,
            [NotNull] float quantity,
            [NotNull] float discAmt
            )
        {
            await Task.Yield();

            Check.NotNull<Guid>(salesOrderId, nameof(salesOrderId));
            Check.NotNull<Guid>(productId, nameof(productId));
            Check.NotNull<float>(quantity, nameof(quantity));

            var salesOrderDetail = new SalesOrderDetail(
                GuidGenerator.Create(),
                salesOrderId,
                productId,
                quantity,
                discAmt
            );

            var product = _productRepository.Where(x => x.Id.Equals(productId)).FirstOrDefault();

            salesOrderDetail.Price = product.RetailPrice;
            salesOrderDetail.TaxRate = product.TaxRate;
            salesOrderDetail.Recalculate();

            var uom = _uomRepository.Where(x => x.Id.Equals(product.UomId)).FirstOrDefault();
            salesOrderDetail.UomName = uom.Name;

            return salesOrderDetail;
        }
    }
}
