using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Indo.Currencies
{
    public class CurrencyAppService : IndoAppService, ICurrencyAppService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly CurrencyManager _currencyManager;
        private readonly ICompanyRepository _companyRepository;
        public CurrencyAppService(
            ICurrencyRepository currencyRepository,
            CurrencyManager currencyManager,
            ICompanyRepository companyRepository
            )
        {
            _currencyRepository = currencyRepository;
            _currencyManager = currencyManager;
            _companyRepository = companyRepository;
        }
        public async Task<CurrencyReadDto> GetAsync(Guid id)
        {
            var obj = await _currencyRepository.GetAsync(id);
            return ObjectMapper.Map<Currency, CurrencyReadDto>(obj);
        }
        public async Task<List<CurrencyReadDto>> GetListAsync()
        {
            var queryable = await _currencyRepository.GetQueryableAsync();
            var query = from currency in queryable
                        select new { currency };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Currency, CurrencyReadDto>(x.currency);
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<CurrencyReadDto> CreateAsync(CurrencyCreateDto input)
        {
            var obj = await _currencyManager.CreateAsync(
                input.Name,
                input.Symbol
            );

            await _currencyRepository.InsertAsync(obj);

            return ObjectMapper.Map<Currency, CurrencyReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, CurrencyUpdateDto input)
        {
            var obj = await _currencyRepository.GetAsync(id);

            if (obj.Name != input.Name)
            {
                await _currencyManager.ChangeNameAsync(obj, input.Name);
            }

            obj.Symbol = input.Symbol;

            await _currencyRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            if (_companyRepository.Where(x => x.CurrencyId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            await _currencyRepository.DeleteAsync(id);
        }
    }
}
