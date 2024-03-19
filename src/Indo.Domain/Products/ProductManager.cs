using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.Products
{
    public class ProductManager : DomainService
    {
        private readonly IProductRepository _productRepository;

        public ProductManager(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<Product> CreateAsync(
            [NotNull] string name,
            [NotNull] Guid uomId)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNull<Guid>(uomId, nameof(uomId));

            var existing = await _productRepository.FindAsync(x => x.Name.Equals(name));
            if (existing != null)
            {
                throw new ProductAlreadyExistsException(name);
            }

            return new Product(
                GuidGenerator.Create(),
                name,
                uomId
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] Product product,
           [NotNull] string newName)
        {
            Check.NotNull(product, nameof(product));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _productRepository.FindAsync(x => x.Name.Equals(newName));
            if (existing != null && existing.Id != product.Id)
            {
                throw new ProductAlreadyExistsException(newName);
            }

            product.ChangeName(newName);
        }
    }
}
