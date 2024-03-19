using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.ProjectOrderDetails
{
    public class ProjectOrderDetailManager : DomainService
    {
        private readonly IProjectOrderDetailRepository _projectOrderDetailRepository;

        public ProjectOrderDetailManager(IProjectOrderDetailRepository projectOrderDetailRepository)
        {
            _projectOrderDetailRepository = projectOrderDetailRepository;
        }
        public async Task<ProjectOrderDetail> CreateAsync(
            [NotNull] Guid projectOrderId,
            [NotNull] string projectTask,
            [NotNull] float quantity,
            [NotNull] float price
            )
        {
            Check.NotNull<Guid>(projectOrderId, nameof(projectOrderId));
            Check.NotNull<string>(projectTask, nameof(projectTask));
            Check.NotNull<float>(quantity, nameof(quantity));
            Check.NotNull<float>(price, nameof(price));

            await Task.Yield();

            var projectOrderDetail = new ProjectOrderDetail(
                GuidGenerator.Create(),
                projectOrderId,
                projectTask,
                quantity,
                price
            );

            projectOrderDetail.Recalculate();

            return projectOrderDetail;
        }
    }
}
