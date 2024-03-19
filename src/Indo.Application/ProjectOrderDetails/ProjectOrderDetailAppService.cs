using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Customers;
using Indo.ProjectOrders;
using Indo.Uoms;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.ProjectOrderDetails
{
    public class ProjectOrderDetailAppService : IndoAppService, IProjectOrderDetailAppService
    {
        private readonly CompanyAppService _companyAppService;
        private readonly IProjectOrderRepository _projectOrderRepository;
        private readonly IProjectOrderDetailRepository _projectOrderDetailRepository;
        private readonly ProjectOrderDetailManager _projectOrderDetailManager;
        private readonly ICustomerRepository _customerRepository;
        public ProjectOrderDetailAppService(
            CompanyAppService companyAppService,
            IProjectOrderDetailRepository projectOrderDetailRepository,
            ProjectOrderDetailManager projectOrderDetailManager,
            ICustomerRepository customerRepository,
            IProjectOrderRepository projectOrderRepository)
        {
            _projectOrderDetailRepository = projectOrderDetailRepository;
            _projectOrderDetailManager = projectOrderDetailManager;
            _projectOrderRepository = projectOrderRepository;
            _companyAppService = companyAppService;
            _customerRepository = customerRepository;
        }
        public async Task<ProjectOrderDetailReadDto> GetAsync(Guid id)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _projectOrderDetailRepository.GetQueryableAsync();
            var query = from projectOrderDetail in queryable
                        join projectOrder in _projectOrderRepository on projectOrderDetail.ProjectOrderId equals projectOrder.Id
                        where projectOrderDetail.Id == id
                        select new { projectOrderDetail, projectOrder };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(ProjectOrderDetail), id);
            }
            var dto = ObjectMapper.Map<ProjectOrderDetail, ProjectOrderDetailReadDto>(queryResult.projectOrderDetail);
            dto.CurrencyName = company.CurrencyName;
            dto.Status = queryResult.projectOrder.Status;
            dto.StatusString = L[$"Enum:ProjectOrderStatus:{(int)queryResult.projectOrder.Status}"];
            dto.PriceString = queryResult.projectOrderDetail.Price.ToString("##,##.00");
            dto.TotalString = queryResult.projectOrderDetail.Total.ToString("##,##.00");
            return dto;
        }
        public async Task<PagedResultDto<ProjectOrderDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _projectOrderDetailRepository.GetQueryableAsync();
            var query = from projectOrderDetail in queryable
                        join projectOrder in _projectOrderRepository on projectOrderDetail.ProjectOrderId equals projectOrder.Id
                        select new { projectOrderDetail, projectOrder };
            query = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<ProjectOrderDetail, ProjectOrderDetailReadDto>(x.projectOrderDetail);
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.projectOrder.Status;
                dto.StatusString = L[$"Enum:ProjectOrderStatus:{(int)x.projectOrder.Status}"];
                dto.PriceString = x.projectOrderDetail.Price.ToString("##,##.00");
                dto.TotalString = x.projectOrderDetail.Total.ToString("##,##.00");
                return dto;
            }).ToList();

            var totalCount = await _projectOrderDetailRepository.GetCountAsync();

            return new PagedResultDto<ProjectOrderDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<List<ProjectOrderDetailReadDto>> GetListDetailAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _projectOrderDetailRepository.GetQueryableAsync();
            var query = from projectOrderDetail in queryable
                        join projectOrder in _projectOrderRepository on projectOrderDetail.ProjectOrderId equals projectOrder.Id
                        join customer in _customerRepository on projectOrder.CustomerId equals customer.Id
                        select new { projectOrderDetail, projectOrder, customer };
            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<ProjectOrderDetail, ProjectOrderDetailReadDto>(x.projectOrderDetail);
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.projectOrder.Status;
                dto.StatusString = L[$"Enum:ProjectOrderStatus:{(int)x.projectOrder.Status}"];
                dto.ProjectOrderNumber = x.projectOrder.Number;
                dto.OrderDate = x.projectOrder.OrderDate;
                dto.CustomerName = x.customer.Name;
                dto.PriceString = x.projectOrderDetail.Price.ToString("##,##.00");
                dto.TotalString = x.projectOrderDetail.Total.ToString("##,##.00");
                return dto;
            })
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return dtos;
        }
        public async Task<PagedResultDto<ProjectOrderDetailReadDto>> GetListByProjectOrderAsync(Guid projectOrderId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _projectOrderDetailRepository.GetQueryableAsync();
            var query = from projectOrderDetail in queryable
                        join projectOrder in _projectOrderRepository on projectOrderDetail.ProjectOrderId equals projectOrder.Id
                        where projectOrderDetail.ProjectOrderId.Equals(projectOrderId)
                        select new { projectOrderDetail, projectOrder };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<ProjectOrderDetail, ProjectOrderDetailReadDto>(x.projectOrderDetail);
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.projectOrder.Status;
                dto.StatusString = L[$"Enum:ProjectOrderStatus:{(int)x.projectOrder.Status}"];
                dto.PriceString = x.projectOrderDetail.Price.ToString("##,##.00");
                dto.TotalString = x.projectOrderDetail.Total.ToString("##,##.00");
                return dto;
            }).ToList();

            var totalCount = dtos.Count;

            return new PagedResultDto<ProjectOrderDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<ListResultDto<ProjectOrderLookupDto>> GetProjectOrderLookupAsync()
        {
            var list = await _projectOrderRepository.GetListAsync();
            return new ListResultDto<ProjectOrderLookupDto>(
                ObjectMapper.Map<List<ProjectOrder>, List<ProjectOrderLookupDto>>(list)
            );
        }
        public async Task<ProjectOrderDetailReadDto> CreateAsync(ProjectOrderDetailCreateDto input)
        {
            var obj = await _projectOrderDetailManager.CreateAsync(
                input.ProjectOrderId,
                input.ProjectTask,
                input.Quantity,
                input.Price
            );

            await _projectOrderDetailRepository.InsertAsync(obj);

            return ObjectMapper.Map<ProjectOrderDetail, ProjectOrderDetailReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, ProjectOrderDetailUpdateDto input)
        {
            var obj = await _projectOrderDetailRepository.GetAsync(id);

            obj.ProjectTask = input.ProjectTask;
            obj.Quantity = input.Quantity;
            obj.Price = input.Price;
            obj.Recalculate();

            await _projectOrderDetailRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _projectOrderDetailRepository.DeleteAsync(id);
        }
    }
}
