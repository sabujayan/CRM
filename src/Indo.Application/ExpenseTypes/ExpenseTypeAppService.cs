using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Expenses;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Indo.ExpenseTypes
{
    public class ExpenseTypeAppService : IndoAppService, IExpenseTypeAppService
    {
        private readonly IExpenseTypeRepository _expenseTypeRepository;
        private readonly ExpenseTypeManager _expenseTypeManager;
        private readonly IExpenseRepository _expenseRepository;
        public ExpenseTypeAppService(
            IExpenseTypeRepository expenseTypeRepository,
            ExpenseTypeManager expenseTypeManager,
            IExpenseRepository expenseRepository
            )
        {
            _expenseTypeRepository = expenseTypeRepository;
            _expenseTypeManager = expenseTypeManager;
            _expenseRepository = expenseRepository;
        }
        public async Task<ExpenseTypeReadDto> GetAsync(Guid id)
        {
            var obj = await _expenseTypeRepository.GetAsync(id);
            return ObjectMapper.Map<ExpenseType, ExpenseTypeReadDto>(obj);
        }
        public async Task<List<ExpenseTypeReadDto>> GetListAsync()
        {
            var queryable = await _expenseTypeRepository.GetQueryableAsync();
            var query = from expenseType in queryable
                        select new { expenseType };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<ExpenseType, ExpenseTypeReadDto>(x.expenseType);
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<ExpenseTypeReadDto> CreateAsync(ExpenseTypeCreateDto input)
        {
            var obj = await _expenseTypeManager.CreateAsync(
                input.Name
            );

            obj.Description = input.Description;

            await _expenseTypeRepository.InsertAsync(obj);

            return ObjectMapper.Map<ExpenseType, ExpenseTypeReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, ExpenseTypeUpdateDto input)
        {
            var obj = await _expenseTypeRepository.GetAsync(id);

            if (obj.Name != input.Name)
            {
                await _expenseTypeManager.ChangeNameAsync(obj, input.Name);
            }

            obj.Description = input.Description;

            await _expenseTypeRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            if (_expenseRepository.Where(x => x.ExpenseTypeId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            await _expenseTypeRepository.DeleteAsync(id);
        }
    }
}
