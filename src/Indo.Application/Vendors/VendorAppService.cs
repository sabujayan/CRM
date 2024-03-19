using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.PurchaseOrders;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Indo.Vendors
{
    public class VendorAppService : IndoAppService, IVendorAppService
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly VendorManager _vendorManager;
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        public VendorAppService(
            IVendorRepository vendorRepository,
            VendorManager vendorManager,
            IPurchaseOrderRepository purchaseOrderRepository
            )
        {
            _vendorRepository = vendorRepository;
            _vendorManager = vendorManager;
            _purchaseOrderRepository = purchaseOrderRepository;
        }
        public async Task<VendorReadDto> GetAsync(Guid id)
        {
            var obj = await _vendorRepository.GetAsync(id);
            return ObjectMapper.Map<Vendor, VendorReadDto>(obj);
        }
        public async Task<List<VendorReadDto>> GetListAsync()
        {
            var queryable = await _vendorRepository.GetQueryableAsync();
            var query = from vendor in queryable
                        select new { vendor };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Vendor, VendorReadDto>(x.vendor);
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<VendorReadDto> CreateAsync(VendorCreateDto input)
        {
            var obj = await _vendorManager.CreateAsync(
                input.Name
            );

            obj.Phone = input.Phone;
            obj.Email = input.Email;
            obj.Street = input.Street;
            obj.City = input.City;
            obj.State = input.State;
            obj.ZipCode = input.ZipCode;

            await _vendorRepository.InsertAsync(obj);

            return ObjectMapper.Map<Vendor, VendorReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, VendorUpdateDto input)
        {
            var obj = await _vendorRepository.GetAsync(id);

            if (obj.Name != input.Name)
            {
                await _vendorManager.ChangeNameAsync(obj, input.Name);
            }

            obj.Phone = input.Phone;
            obj.Email = input.Email;
            obj.Street = input.Street;
            obj.City = input.City;
            obj.State = input.State;
            obj.ZipCode = input.ZipCode;

            await _vendorRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            if (_purchaseOrderRepository.Where(x => x.VendorId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            await _vendorRepository.DeleteAsync(id);
        }
    }
}
