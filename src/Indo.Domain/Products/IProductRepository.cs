using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.Products
{
    public interface IProductRepository : IRepository<Product, Guid>
    {
    }
}
