using System;
using System.Linq;
using System.Threading.Tasks;
using Indo.Products;
using Indo.Uoms;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.SalesQuotationDetails
{
    public class SalesQuotationDetailManager : DomainService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUomRepository _uomRepository;

        public SalesQuotationDetailManager(
            IProductRepository productRepository,
            IUomRepository uomRepository
            )
        {
            _productRepository = productRepository;
            _uomRepository = uomRepository;
        }
        public async Task<SalesQuotationDetail> CreateAsync(
            [NotNull] Guid salesQuotationId,
            [NotNull] Guid productId,
            [NotNull] float quantity,
            [NotNull] float discAmt
            )
        {
            await Task.Yield();

            Check.NotNull<Guid>(salesQuotationId, nameof(salesQuotationId));
            Check.NotNull<Guid>(productId, nameof(productId));
            Check.NotNull<float>(quantity, nameof(quantity));

            var salesQuotationDetail = new SalesQuotationDetail(
                GuidGenerator.Create(),
                salesQuotationId,
                productId,
                quantity,
                discAmt
            );

            var product = _productRepository.Where(x => x.Id.Equals(productId)).FirstOrDefault();

            salesQuotationDetail.Price = product.RetailPrice;
            salesQuotationDetail.TaxRate = product.TaxRate;
            salesQuotationDetail.Recalculate();

            var uom = _uomRepository.Where(x => x.Id.Equals(product.UomId)).FirstOrDefault();
            salesQuotationDetail.UomName = uom.Name;

            return salesQuotationDetail;
        }
    }
}
