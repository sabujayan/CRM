using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Activities;
using Indo.Customers;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.Tasks
{
    public class TaskAppService : IndoAppService, ITaskAppService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly TaskManager _taskManager;
        private readonly ICustomerRepository _customerRepository;
        private readonly IActivityRepository _activityRepository;
        public TaskAppService(
            ITaskRepository taskRepository,
            TaskManager taskManager,
            ICustomerRepository customerRepository,
            IActivityRepository activityRepository
            )
        {
            _taskRepository = taskRepository;
            _taskManager = taskManager;
            _customerRepository = customerRepository;
            _activityRepository = activityRepository;
        }
        public async Task<TaskReadDto> GetAsync(Guid id)
        {
            var queryable = await _taskRepository.GetQueryableAsync();
            var query = from task in queryable
                        join customer in _customerRepository on task.CustomerId equals customer.Id
                        join activity in _activityRepository on task.ActivityId equals activity.Id
                        where task.Id == id
                        select new { task, customer, activity };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(Task), id);
            }
            var dto = ObjectMapper.Map<Task, TaskReadDto>(queryResult.task);
            dto.CustomerName = queryResult.customer.Name;
            dto.ActivityName = queryResult.activity.Name;

            return dto;
        }
        public async Task<List<TaskReadDto>> GetListAsync()
        {
            var queryable = await _taskRepository.GetQueryableAsync();
            var query = from task in queryable
                        join customer in _customerRepository on task.CustomerId equals customer.Id
                        join activity in _activityRepository on task.ActivityId equals activity.Id
                        select new { task, customer, activity };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Task, TaskReadDto>(x.task);
                dto.CustomerName = x.customer.Name;
                dto.ActivityName = x.activity.Name;
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<List<TaskReadDto>> GetListByCustomerAsync(Guid customerId)
        {
            var queryable = await _taskRepository.GetQueryableAsync();
            var query = from task in queryable
                        join customer in _customerRepository on task.CustomerId equals customer.Id
                        join activity in _activityRepository on task.ActivityId equals activity.Id
                        where task.CustomerId == customerId
                        select new { task, customer, activity };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Task, TaskReadDto>(x.task);
                dto.CustomerName = x.customer.Name;
                dto.ActivityName = x.activity.Name;
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<ListResultDto<CustomerLookupDto>> GetCustomerLookupAsync()
        {
            var list = await _customerRepository.GetListAsync();
            return new ListResultDto<CustomerLookupDto>(
                ObjectMapper.Map<List<Customer>, List<CustomerLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<ActivityLookupDto>> GetActivityLookupAsync()
        {
            var list = await _activityRepository.GetListAsync();
            return new ListResultDto<ActivityLookupDto>(
                ObjectMapper.Map<List<Activity>, List<ActivityLookupDto>>(list)
            );
        }
        public async Task<TaskReadDto> CreateAsync(TaskCreateDto input)
        {
            var obj = await _taskManager.CreateAsync(
                input.Name,
                input.StartTime,
                input.EndTime,
                input.CustomerId,
                input.ActivityId
            );

            obj.Description = input.Description;
            obj.Location = input.Location;
            obj.IsDone = input.IsDone;

            await _taskRepository.InsertAsync(obj);

            return ObjectMapper.Map<Task, TaskReadDto>(obj);
        }
        public async System.Threading.Tasks.Task UpdateAsync(Guid id, TaskUpdateDto input)
        {
            var obj = await _taskRepository.GetAsync(id);

            if (obj.Name != input.Name)
            {
                await _taskManager.ChangeNameAsync(obj, input.Name);
            }

            obj.Description = input.Description;
            obj.Location = input.Location;
            obj.StartTime = input.StartTime;
            obj.EndTime = input.EndTime;
            obj.CustomerId = input.CustomerId;
            obj.ActivityId = input.ActivityId;
            obj.IsDone = input.IsDone;

            await _taskRepository.UpdateAsync(obj);
        }
        public async System.Threading.Tasks.Task DeleteAsync(Guid id)
        {
            await _taskRepository.DeleteAsync(id);
        }
    }
}
