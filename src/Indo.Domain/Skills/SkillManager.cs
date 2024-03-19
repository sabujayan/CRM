using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.Skills
{
    public class SkillManager:DomainService
    {
        private readonly ISkillRepository _skillRepository;

        public SkillManager(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }
        public async Task<Skill> CreateAsync(
            [NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existing = await _skillRepository.FindAsync(x => x.Name.Equals(name));
            if (existing != null)
            {
                throw new SkillAlreadyExistsException(name);
            }

            return new Skill(
                GuidGenerator.Create(),
                name
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] Skill skill,
           [NotNull] string newName)
        {
            Check.NotNull(skill, nameof(skill));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _skillRepository.FindAsync(x => x.Name.Equals(newName));
            if (existing != null && existing.Id != skill.Id)
            {
                throw new SkillAlreadyExistsException(newName);
            }

            skill.ChangeName(newName);
        }
    }
}
