using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Contacts;
using Indo.Expenses;
using Indo.ImportantDates;
using Indo.LeadRatings;
using Indo.LeadSources;
using Indo.Notes;
using Indo.NumberSequences;
using Indo.ProjectOrders;
using Indo.SalesOrders;
using Indo.SalesQuotations;
using Indo.ServiceOrders;
using Indo.ServiceQuotations;
using Indo.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Indo.Customers
{
    public class CustomerAppService : IndoAppService, ICustomerAppService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly CustomerManager _customerManager;
        private readonly IProjectOrderRepository _projectOrderRepository;
        private readonly IServiceOrderRepository _serviceOrderRepository;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly INumberSequenceAppService _numberSequenceAppService;
        private readonly IExpenseRepository _expenseRepository;
        private readonly ILeadRatingRepository _leadRatingRepository;
        private readonly ILeadSourceRepository _leadSourceRepository;
        private readonly IServiceQuotationRepository _serviceQuotationRepository;
        private readonly ISalesQuotationRepository _salesQuotationRepository;
        private readonly IContactRepository _contactRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly INoteRepository _noteRepository;
        private readonly IImportantDateRepository _importantDateRepository;

        public CustomerAppService(
            ICustomerRepository customerRepository, 
            CustomerManager customerManager,
            IProjectOrderRepository projectOrderRepository,
            IServiceOrderRepository serviceOrderRepository,
            ISalesOrderRepository salesOrderRepository,
            INumberSequenceAppService numberSequenceAppService,
            IExpenseRepository expenseRepository,
            ILeadRatingRepository leadRatingRepository,
            ILeadSourceRepository leadSourceRepository,
            IServiceQuotationRepository serviceQuotationRepository,
            ISalesQuotationRepository salesQuotationRepository,
            IContactRepository contactRepository,
            ITaskRepository taskRepository,
            INoteRepository noteRepository,
            IImportantDateRepository importantDateRepository
            )
        {
            _customerRepository = customerRepository;
            _customerManager = customerManager;
            _projectOrderRepository = projectOrderRepository;
            _serviceOrderRepository = serviceOrderRepository;
            _salesOrderRepository = salesOrderRepository;
            _numberSequenceAppService = numberSequenceAppService;
            _expenseRepository = expenseRepository;
            _leadRatingRepository = leadRatingRepository;
            _leadSourceRepository = leadSourceRepository;
            _serviceQuotationRepository = serviceQuotationRepository;
            _salesQuotationRepository = salesQuotationRepository;
            _contactRepository = contactRepository;
            _taskRepository = taskRepository;
            _noteRepository = noteRepository;
            _importantDateRepository = importantDateRepository;
        }
        public async Task<CustomerReadDto> GetAsync(Guid id)
        {
            var obj = await _customerRepository.GetAsync(id);
            return ObjectMapper.Map<Customer, CustomerReadDto>(obj);
        }
        public async Task<List<CustomerReadDto>> GetListAsync()
        {
            var queryable = await _customerRepository.GetQueryableAsync();
            var query = from customer in queryable
                        select new { customer };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Customer, CustomerReadDto>(x.customer);
                dto.StageString = L[$"Enum:CustomerStage:{(int)x.customer.Stage}"];
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<List<CustomerReadDto>> GetListLeadAsync()
        {
            var queryable = await _customerRepository.GetQueryableAsync();
            var query = from customer in queryable
                        where customer.Status == CustomerStatus.Lead
                        select new { customer };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Customer, CustomerReadDto>(x.customer);
                dto.StageString = L[$"Enum:CustomerStage:{(int)x.customer.Stage}"];
                dto.Street = String.IsNullOrEmpty(x.customer.Street) ? "" : x.customer.Street;
                dto.Email = String.IsNullOrEmpty(x.customer.Email) ? "" : x.customer.Email;
                dto.Phone = String.IsNullOrEmpty(x.customer.Phone) ? "" : x.customer.Phone;
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<List<CustomerReadDto>> GetListCustomerAsync()
        {
            var queryable = await _customerRepository.GetQueryableAsync();
            var query = from customer in queryable
                        where customer.Status == CustomerStatus.Customer
                        select new { customer };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Customer, CustomerReadDto>(x.customer);
                dto.StageString = L[$"Enum:CustomerStage:{(int)x.customer.Stage}"];
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<List<CustomerReadDto>> GetListByCustomerAsync(Guid customerId)
        {
            var queryable = await _customerRepository.GetQueryableAsync();
            var query = from customer in queryable
                        where customer.Id.Equals(customerId)
                        select new { customer };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Customer, CustomerReadDto>(x.customer);
                dto.StageString = L[$"Enum:CustomerStage:{(int)x.customer.Stage}"];
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<int> GetLeadCountByStage(CustomerStage stage)
        {
            await System.Threading.Tasks.Task.Yield();
            
            return _customerRepository.Where(x => x.Stage == stage).Count();
        }
        public async Task<CustomerReadDto> CreateAsync(CustomerCreateDto input)
        {
            var obj = await _customerManager.CreateAsync(
                input.Name
            );

            obj.Phone = input.Phone;
            obj.Email = input.Email;
            obj.Street = input.Street;
            obj.City = input.City;
            obj.State = input.State;
            obj.ZipCode = input.ZipCode;
            obj.Status = input.Status;
            obj.RootFolder = await _numberSequenceAppService.GetNextNumberAsync(NumberSequenceModules.CustomerRootFolder);
            obj.LeadRatingId = input.LeadRatingId;
            obj.LeadSourceId = input.LeadSourceId;
            obj.Stage = input.Stage;

            await _customerRepository.InsertAsync(obj);

            return ObjectMapper.Map<Customer, CustomerReadDto>(obj);
        }
        public async System.Threading.Tasks.Task UpdateAsync(Guid id, CustomerUpdateDto input)
        {
            var obj = await _customerRepository.GetAsync(id);

            if (obj.Name != input.Name)
            {
                await _customerManager.ChangeNameAsync(obj, input.Name);
            }

            obj.Phone = input.Phone;
            obj.Email = input.Email;
            obj.Street = input.Street;
            obj.City = input.City;
            obj.State = input.State;
            obj.ZipCode = input.ZipCode;
            obj.LeadRatingId = input.LeadRatingId;
            obj.LeadSourceId = input.LeadSourceId;
            obj.Stage = input.Stage;

            await _customerRepository.UpdateAsync(obj);
        }
        public async System.Threading.Tasks.Task DeleteAsync(Guid id)
        {
            if (_projectOrderRepository.Where(x => x.CustomerId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            if (_serviceOrderRepository.Where(x => x.CustomerId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            if (_salesOrderRepository.Where(x => x.CustomerId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            if (_expenseRepository.Where(x => x.CustomerId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            if (_serviceQuotationRepository.Where(x => x.CustomerId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            if (_salesQuotationRepository.Where(x => x.CustomerId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            if (_contactRepository.Where(x => x.CustomerId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            if (_importantDateRepository.Where(x => x.CustomerId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            if (_taskRepository.Where(x => x.CustomerId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            if (_noteRepository.Where(x => x.CustomerId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            await _customerRepository.DeleteAsync(id);
        }

        public async Task<CustomerReadDto> ConvertLeadToCustomerAsync(Guid customerId)
        {
            var obj = await _customerManager.ConvertLeadToCustomerAsync(customerId);
            return ObjectMapper.Map<Customer, CustomerReadDto>(obj);
        }

        public async Task<ListResultDto<LeadRatingLookupDto>> GetLeadRatingLookupAsync()
        {
            var list = await _leadRatingRepository.GetListAsync();
            return new ListResultDto<LeadRatingLookupDto>(
                ObjectMapper.Map<List<LeadRating>, List<LeadRatingLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<LeadSourceLookupDto>> GetLeadSourceLookupAsync()
        {
            var list = await _leadSourceRepository.GetListAsync();
            return new ListResultDto<LeadSourceLookupDto>(
                ObjectMapper.Map<List<LeadSource>, List<LeadSourceLookupDto>>(list)
            );
        }

    }
}
