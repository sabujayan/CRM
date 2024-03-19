using Indo.Departments;
using Indo.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Indo.Skills
{
    public class SkillAppService : IndoAppService,ISkillAppService
    {
        private readonly ISkillRepository _skillRepository;
        private readonly SkillManager _skillManager;
       
        public SkillAppService(
           ISkillRepository skillRepository,
            SkillManager skillManager

            )
        {
            _skillRepository = skillRepository;
            _skillManager = skillManager;
           
        }
        public async Task<SkillReadDto> GetAsync(Guid id)
        {
            var obj = await _skillRepository.GetAsync(id);
            return ObjectMapper.Map<Skill, SkillReadDto>(obj);
        }
        public async Task<List<SkillReadDto>> GetListAsync()
        {
            var queryable = await _skillRepository.GetQueryableAsync();
            var query = from skill in queryable
                        select new { skill };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Skill, SkillReadDto>(x.skill);
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<SkillReadDto> CreateAsync(SkillCreateDto input)
        {
            var obj = await _skillManager.CreateAsync(
                input.Name
            );

            //obj.n = input.Description;

            await _skillRepository.InsertAsync(obj);

            return ObjectMapper.Map<Skill, SkillReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, SkillUpdateDto input)
        {
            var obj = await _skillRepository.GetAsync(id);

            if (obj.Name != input.Name)
            {
                await _skillManager.ChangeNameAsync(obj, input.Name);
            }

            //obj.Description = input.Description;

            await _skillRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _skillRepository.DeleteAsync(id);
        }
    }
}
