using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.Products
{
    public interface IProductAppService : IApplicationService
    {
        Task<ProductReadDto> GetAsync(Guid id);

        Task<List<ProductReadDto>> GetListAsync();

        Task<ProductReadDto> CreateAsync(ProductCreateDto input);

        Task UpdateAsync(Guid id, ProductUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<UomLookupDto>> GetUomLookupAsync();
    }
}
