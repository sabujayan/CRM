using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Tasks;
using Indo.Services;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Indo.Activities
{
    public class ActivityAppService : IndoAppService, IActivityAppService
    {
        private readonly IActivityRepository _activityRepository;
        private readonly ActivityManager _activityManager;
        private readonly ITaskRepository _taskRepository;
        private readonly IServiceRepository _serviceRepository;
        public ActivityAppService(
            IActivityRepository activityRepository,
            ActivityManager activityManager,
            ITaskRepository taskRepository,
            IServiceRepository serviceRepository
            )
        {
            _activityRepository = activityRepository;
            _activityManager = activityManager;
            _taskRepository = taskRepository;
            _serviceRepository = serviceRepository;
        }
        public async Task<ActivityReadDto> GetAsync(Guid id)
        {
            var obj = await _activityRepository.GetAsync(id);
            return ObjectMapper.Map<Activity, ActivityReadDto>(obj);
        }
        public async Task<List<ActivityReadDto>> GetListAsync()
        {
            var queryable = await _activityRepository.GetQueryableAsync();
            var query = from activity in queryable
                        select new { activity };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Activity, ActivityReadDto>(x.activity);
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<ActivityReadDto> CreateAsync(ActivityCreateDto input)
        {
            var obj = await _activityManager.CreateAsync(
                input.Name
            );

            obj.Description = input.Description;

            await _activityRepository.InsertAsync(obj);

            return ObjectMapper.Map<Activity, ActivityReadDto>(obj);
        }
        public async System.Threading.Tasks.Task UpdateAsync(Guid id, ActivityUpdateDto input)
        {
            var obj = await _activityRepository.GetAsync(id);

            if (obj.Name != input.Name)
            {
                await _activityManager.ChangeNameAsync(obj, input.Name);
            }

            obj.Description = input.Description;

            await _activityRepository.UpdateAsync(obj);
        }
        public async System.Threading.Tasks.Task DeleteAsync(Guid id)
        {
            if (_taskRepository.Where(x => x.ActivityId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            await _activityRepository.DeleteAsync(id);
        }
    }
}
