using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.Projectes
{
    public class ProjectsManager : DomainService
    {
        private readonly IProjectsRepository _projectsRepository;
        public ProjectsManager(IProjectsRepository projectsRepository)
        {
            _projectsRepository = projectsRepository;
        }
        public async Task<Projects> CreateAsync(
         [NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existing = await _projectsRepository.FindAsync(x => x.Name.Equals(name));
            if (existing != null)
            {
                throw new ProjectsAlreadyExistsException(name);
            }

            return new Projects(
                GuidGenerator.Create(),
                name
            );
        }

        public async Task ChangeNameAsync(
          [NotNull] Projects project,
          [NotNull] string newName)
        {
            Check.NotNull(project, nameof(project));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _projectsRepository.FindAsync(x => x.Name.Equals(newName));
            if (existing != null && existing.Id != project.Id)
            {
                throw new ProjectsAlreadyExistsException(newName);
            }

            project.ChangeName(newName);
        }
    }
}
