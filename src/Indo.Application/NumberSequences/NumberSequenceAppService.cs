using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Products;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Indo.NumberSequences
{
    public class NumberSequenceAppService : IndoAppService, INumberSequenceAppService
    {
        private readonly INumberSequenceRepository _numberSequenceRepository;
        private readonly NumberSequenceManager _numberSequenceManager;
        public NumberSequenceAppService(
            INumberSequenceRepository numberSequenceRepository,
            NumberSequenceManager numberSequenceManager
            )
        {
            _numberSequenceRepository = numberSequenceRepository;
            _numberSequenceManager = numberSequenceManager;
        }
        public async Task<NumberSequenceReadDto> GetAsync(Guid id)
        {
            var obj = await _numberSequenceRepository.GetAsync(id);
            return ObjectMapper.Map<NumberSequence, NumberSequenceReadDto>(obj);
        }
        public async Task<string> GetNextNumberAsync(NumberSequenceModules module)
        {
            return await _numberSequenceManager.GetNextNumberAsync(module);
        }
        public async Task<List<NumberSequenceReadDto>> GetListAsync()
        {
            var queryable = await _numberSequenceRepository.GetQueryableAsync();
            var query = from numberSequence in queryable
                        select new { numberSequence };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<NumberSequence, NumberSequenceReadDto>(x.numberSequence);
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<NumberSequenceReadDto> CreateAsync(NumberSequenceCreateDto input)
        {
            var obj = await _numberSequenceManager.CreateAsync(
                input.Suffix
            );

            await _numberSequenceRepository.InsertAsync(obj);

            return ObjectMapper.Map<NumberSequence, NumberSequenceReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, NumberSequenceUpdateDto input)
        {
            var obj = await _numberSequenceRepository.GetAsync(id);

            if (obj.Suffix != input.Suffix)
            {
                await _numberSequenceManager.ChangeNameAsync(obj, input.Suffix);
            }

            obj.Suffix = input.Suffix;

            await _numberSequenceRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            var numberSequence = await _numberSequenceRepository
                .GetAsync(id);

            if (numberSequence != null && numberSequence.NextNumber > 0)
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }

            await _numberSequenceRepository.DeleteAsync(id);
        }
    }
}
