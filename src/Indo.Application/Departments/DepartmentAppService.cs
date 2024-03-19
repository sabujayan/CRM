using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Employees;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Indo.Departments
{
    public class DepartmentAppService : IndoAppService, IDepartmentAppService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly DepartmentManager _departmentManager;
        private readonly IEmployeeRepository _employeeRepository;
        public DepartmentAppService(
            IDepartmentRepository departmentRepository,
            DepartmentManager departmentManager,
            IEmployeeRepository employeeRepository
            )
        {
            _departmentRepository = departmentRepository;
            _departmentManager = departmentManager;
            _employeeRepository = employeeRepository;
        }
        public async Task<DepartmentReadDto> GetAsync(Guid id)
        {
            var obj = await _departmentRepository.GetAsync(id);
            return ObjectMapper.Map<Department, DepartmentReadDto>(obj);
        }
        public async Task<List<DepartmentReadDto>> GetListAsync()
        {
            var queryable = await _departmentRepository.GetQueryableAsync();
            var query = from department in queryable
                        select new { department };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Department, DepartmentReadDto>(x.department);
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<DepartmentReadDto> CreateAsync(DepartmentCreateDto input)
        {
            var obj = await _departmentManager.CreateAsync(
                input.Name
            );

            obj.Description = input.Description;

            await _departmentRepository.InsertAsync(obj);

            return ObjectMapper.Map<Department, DepartmentReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, DepartmentUpdateDto input)
        {
            var obj = await _departmentRepository.GetAsync(id);

            if (obj.Name != input.Name)
            {
                await _departmentManager.ChangeNameAsync(obj, input.Name);
            }

            obj.Description = input.Description;

            await _departmentRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            if (_employeeRepository.Where(x => x.DepartmentId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            await _departmentRepository.DeleteAsync(id);
        }
    }
}
