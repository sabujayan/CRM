using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.Technologies
{
    public class TechnologyManager : DomainService
    {
        private readonly ITechnologyRepository _technologyRepository;

        public TechnologyManager(ITechnologyRepository technologyRepository)
        {
            _technologyRepository = technologyRepository;
        }


        public async Task<Technology> CreateAsync(
       [NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existing = await _technologyRepository.FindAsync(x => x.Name.Equals(name));
            if (existing != null)
            {
                throw new TechnologyAlreadyExistsException(name);
            }

            return new Technology(
                GuidGenerator.Create(),
                name
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] Technology department,
           [NotNull] string newName)
        {
            Check.NotNull(department, nameof(department));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _technologyRepository.FindAsync(x => x.Name.Equals(newName));
            if (existing != null && existing.Id != department.Id)
            {
                throw new TechnologyAlreadyExistsException(newName);
            }

            department.ChangeName(newName);
        }


    }
}
