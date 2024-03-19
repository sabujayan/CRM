using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.DeliveryOrders;
using Indo.GoodsReceipts;
using Indo.PurchaseReceipts;
using Indo.SalesDeliveries;
using Indo.StockAdjustments;
using Indo.TransferOrders;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Indo.Warehouses
{
    public class WarehouseAppService : IndoAppService, IWarehouseAppService
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly WarehouseManager _warehouseManager;
        private readonly ITransferOrderRepository _transferOrderRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IStockAdjustmentRepository _stockAdjustmentRepository;
        private readonly IDeliveryOrderRepository _deliveryOrderRepository;
        private readonly IGoodsReceiptRepository _goodsReceiptRepository;
        public WarehouseAppService(
            IWarehouseRepository warehouseRepository,
            WarehouseManager warehouseManager,
            ITransferOrderRepository transferOrderRepository,
            ICompanyRepository companyRepository,
            IStockAdjustmentRepository stockAdjustmentRepository,
            IDeliveryOrderRepository deliveryOrderRepository,
            IGoodsReceiptRepository goodsReceiptRepository
            )
        {
            _warehouseRepository = warehouseRepository;
            _warehouseManager = warehouseManager;
            _transferOrderRepository = transferOrderRepository;
            _companyRepository = companyRepository;
            _stockAdjustmentRepository = stockAdjustmentRepository;
            _deliveryOrderRepository = deliveryOrderRepository;
            _goodsReceiptRepository = goodsReceiptRepository;
        }
        public async Task<WarehouseReadDto> GetAsync(Guid id)
        {
            var obj = await _warehouseRepository.GetAsync(id);
            return ObjectMapper.Map<Warehouse, WarehouseReadDto>(obj);
        }
        public async Task<WarehouseReadDto> GetDefaultWarehouseAsync()
        {
            var obj = await _warehouseManager.GetDefaultWarehouseAsync();
            return ObjectMapper.Map<Warehouse, WarehouseReadDto>(obj);
        }
        public async Task<List<WarehouseReadDto>> GetListAsync()
        {
            var queryable = await _warehouseRepository.GetQueryableAsync();
            var query = from warehouse in queryable
                        where warehouse.Virtual.Equals(false)
                        select new { warehouse };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Warehouse, WarehouseReadDto>(x.warehouse);
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<WarehouseReadDto> CreateAsync(WarehouseCreateDto input)
        {
            var obj = await _warehouseManager.CreateAsync(
                input.Name
            );

            obj.Description = input.Description;

            await _warehouseRepository.InsertAsync(obj);

            return ObjectMapper.Map<Warehouse, WarehouseReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, WarehouseUpdateDto input)
        {
            var obj = await _warehouseRepository.GetAsync(id);

            if (obj.Name != input.Name)
            {
                await _warehouseManager.ChangeNameAsync(obj, input.Name);
            }

            obj.Description = input.Description;

            await _warehouseRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            if (_transferOrderRepository.Where(x => x.FromWarehouseId.Equals(id) || x.ToWarehouseId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            if (_deliveryOrderRepository.Where(x => x.FromWarehouseId.Equals(id) || x.ToWarehouseId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            if (_goodsReceiptRepository.Where(x => x.FromWarehouseId.Equals(id) || x.ToWarehouseId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            if (_companyRepository.Where(x => x.DefaultWarehouseId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            if (_stockAdjustmentRepository.Where(x => x.FromWarehouseId.Equals(id) || x.ToWarehouseId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            await _warehouseRepository.DeleteAsync(id);
        }
    }
}
