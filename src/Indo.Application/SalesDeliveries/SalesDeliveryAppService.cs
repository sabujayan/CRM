using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Customers;
using Indo.SalesOrders;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.SalesDeliveries
{
    public class SalesDeliveryAppService : IndoAppService, ISalesDeliveryAppService
    {
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly ISalesDeliveryRepository _salesDeliveryRepository;
        private readonly SalesDeliveryManager _salesDeliveryManager;
        private readonly ICompanyAppService _companyAppService;
        private readonly ICustomerRepository _customerRepository;
        public SalesDeliveryAppService(
            ISalesDeliveryRepository salesDeliveryRepository,
            SalesDeliveryManager salesDeliveryManager,
            ISalesOrderRepository salesOrderRepository,
            ICustomerRepository customerRepository,
            ICompanyAppService companyAppService)
        {
            _salesDeliveryRepository = salesDeliveryRepository;
            _salesDeliveryManager = salesDeliveryManager;
            _salesOrderRepository = salesOrderRepository;
            _companyAppService = companyAppService;
            _customerRepository = customerRepository;
        }
        public async Task<SalesDeliveryReadDto> GetAsync(Guid id)
        {
            var queryable = await _salesDeliveryRepository.GetQueryableAsync();
            var query = from salesDelivery in queryable
                        join salesOrder in _salesOrderRepository on salesDelivery.SalesOrderId equals salesOrder.Id
                        where salesDelivery.Id == id
                        select new { salesDelivery, salesOrder };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(SalesDelivery), id);
            }
            var dto = ObjectMapper.Map<SalesDelivery, SalesDeliveryReadDto>(queryResult.salesDelivery);
            dto.SalesOrderNumber = queryResult.salesOrder.Number;
            dto.CustomerId = queryResult.salesOrder.CustomerId;
            dto.StatusString = L[$"Enum:SalesDeliveryStatus:{(int)queryResult.salesDelivery.Status}"];
            return dto;
        }
        public async Task<List<SalesDeliveryReadDto>> GetListAsync()
        {
            var queryable = await _salesDeliveryRepository.GetQueryableAsync();
            var query = from salesDelivery in queryable
                        join salesOrder in _salesOrderRepository on salesDelivery.SalesOrderId equals salesOrder.Id
                        join customer in _customerRepository on salesOrder.CustomerId equals customer.Id
                        select new { salesDelivery, salesOrder, customer };
           
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<SalesDelivery, SalesDeliveryReadDto>(x.salesDelivery);
                dto.SalesOrderNumber = x.salesOrder.Number;
                dto.CustomerId = x.salesOrder.CustomerId;
                dto.CustomerName = x.customer.Name;
                dto.StatusString = L[$"Enum:SalesDeliveryStatus:{(int)x.salesDelivery.Status}"];
                return dto;
            })
                .OrderByDescending(x => x.DeliveryDate)
                .ToList();
            return dtos;
        }
        public async Task<List<SalesDeliveryReadDto>> GetListBySalesOrderAsync(Guid salesOrderId)
        {
            var queryable = await _salesDeliveryRepository.GetQueryableAsync();
            var query = from salesDelivery in queryable
                        join salesOrder in _salesOrderRepository on salesDelivery.SalesOrderId equals salesOrder.Id
                        where salesDelivery.SalesOrderId.Equals(salesOrderId)
                        select new { salesDelivery, salesOrder };     
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<SalesDelivery, SalesDeliveryReadDto>(x.salesDelivery);
                dto.SalesOrderNumber = x.salesOrder.Number;
                dto.CustomerId = x.salesOrder.CustomerId;
                dto.StatusString = L[$"Enum:SalesDeliveryStatus:{(int)x.salesDelivery.Status}"];
                return dto;
            }).ToList();

            return dtos;

        }
        public async Task<ListResultDto<SalesOrderLookupDto>> GetSalesOrderLookupAsync()
        {
            var list = await _salesOrderRepository.GetListAsync();
            return new ListResultDto<SalesOrderLookupDto>(
                ObjectMapper.Map<List<SalesOrder>, List<SalesOrderLookupDto>>(list)
            );
        }
        public async Task<SalesDeliveryReadDto> CreateAsync(SalesDeliveryCreateDto input)
        {
            var obj = await _salesDeliveryManager.CreateAsync(
                input.Number,
                input.SalesOrderId,
                input.DeliveryDate
            );

            obj.Description = input.Description;

            await _salesDeliveryRepository.InsertAsync(obj);

            return ObjectMapper.Map<SalesDelivery, SalesDeliveryReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, SalesDeliveryUpdateDto input)
        {
            var obj = await _salesDeliveryRepository.GetAsync(id);

            if (obj.Number != input.Number)
            {
                await _salesDeliveryManager.ChangeNameAsync(obj, input.Number);
            }

            obj.SalesOrderId = input.SalesOrderId;
            obj.Description = input.Description;
            obj.DeliveryDate = input.DeliveryDate;

            await _salesDeliveryRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _salesDeliveryRepository.DeleteAsync(id);
        }

        public async Task ConfirmAsync(Guid salesDeliveryId)
        {
            await _salesDeliveryManager.ConfirmSalesDeliveryAsync(salesDeliveryId);
        }

        public async Task ReturnAsync(Guid salesDeliveryId)
        {
            var returned = await _salesDeliveryManager.GenerateSalesDeliveryReturnFromDeliveryAsync(salesDeliveryId);
            await _salesDeliveryManager.ConfirmSalesDeliveryReturnAsync(returned.Id);
        }
    }
}
