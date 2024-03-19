using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.Vendors
{
    public interface IVendorAppService : IApplicationService
    {
        Task<VendorReadDto> GetAsync(Guid id);

        Task<List<VendorReadDto>> GetListAsync();

        Task<VendorReadDto> CreateAsync(VendorCreateDto input);

        Task UpdateAsync(Guid id, VendorUpdateDto input);

        Task DeleteAsync(Guid id);
    }
}
