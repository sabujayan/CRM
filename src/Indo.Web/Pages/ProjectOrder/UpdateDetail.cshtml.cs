using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.ProjectOrderDetails;
using Indo.ProjectOrders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.ProjectOrder
{
    public class UpdateDetailModel : IndoPageModel
    {
        [BindProperty]
        public ProjectOrderDetailUpdateViewModel ProjectOrderDetail { get; set; }
        public List<SelectListItem> ProjectOrders { get; set; }
        public ProjectOrderStatus Status { get; set; }

        private readonly IProjectOrderDetailAppService _projectOrderDetailAppService;
        public UpdateDetailModel(IProjectOrderDetailAppService projectOrderDetailAppService)
        {
            _projectOrderDetailAppService = projectOrderDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _projectOrderDetailAppService.GetAsync(id);
            Status = dto.Status;
            ProjectOrderDetail = ObjectMapper.Map<ProjectOrderDetailReadDto, ProjectOrderDetailUpdateViewModel>(dto);

            var projectOrderLookup = await _projectOrderDetailAppService.GetProjectOrderLookupAsync();
            ProjectOrders = projectOrderLookup.Items
                .Where(x => x.Id.Equals(ProjectOrderDetail.ProjectOrderId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _projectOrderDetailAppService.UpdateAsync(
                    ProjectOrderDetail.Id,
                    ObjectMapper.Map<ProjectOrderDetailUpdateViewModel, ProjectOrderDetailUpdateDto>(ProjectOrderDetail)
                );
                return NoContent();

            }
            catch (Exception)
            {
                throw new UserFriendlyException($"Posting Error");
            }
        }

        public class ProjectOrderDetailUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [SelectItems(nameof(ProjectOrders))]
            [DisplayName("Project Orders")]
            public Guid ProjectOrderId { get; set; }

            [Required]
            [DisplayName("Project Task")]
            public string ProjectTask { get; set; }

            [Required]
            public float Price { get; set; }

            [Required]
            public float Quantity { get; set; }
        }
    }
}
