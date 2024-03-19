using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.ProjectsTechnologies
{
    public class ProjectsTechnologyMatricesManager : DomainService
    {
        private readonly IProjectsTechnologyMatricesRepository _projectsTechnologyMatricesRepository;
        public ProjectsTechnologyMatricesManager(IProjectsTechnologyMatricesRepository projectsTechnologyMatricesRepository)
        {
            _projectsTechnologyMatricesRepository = projectsTechnologyMatricesRepository;
        }

        public async Task<ProjectsTechnologyMatrices> CreateAsync(
         [NotNull] Guid ProjectsId,
         [NotNull] Guid TechnologyId
         )
        {
            Check.NotNull(ProjectsId, nameof(ProjectsId));
            Check.NotNull(TechnologyId, nameof(TechnologyId));

            return new ProjectsTechnologyMatrices(
                GuidGenerator.Create(),
                ProjectsId,
                TechnologyId
            );
        }
    }
}
