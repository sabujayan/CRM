using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.ProjectOrderDetails;
using Indo.ProjectOrders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.ProjectOrder
{
    public class CreateDetailModel : IndoPageModel
    {
        [BindProperty]
        public CreateProjectOrderDetailViewModel ProjectOrderDetail { get; set; }
        public List<SelectListItem> ProjectOrders { get; set; }

        private readonly IProjectOrderDetailAppService _projectOrderDetailAppService;
        public CreateDetailModel(IProjectOrderDetailAppService projectOrderDetailAppService)
        {
            _projectOrderDetailAppService = projectOrderDetailAppService;
        }
        public async Task OnGetAsync(Guid projectOrderId)
        {
            ProjectOrderDetail = new CreateProjectOrderDetailViewModel();
            ProjectOrderDetail.ProjectOrderId = projectOrderId;

            var projectOrderLookup = await _projectOrderDetailAppService.GetProjectOrderLookupAsync();
            ProjectOrders = projectOrderLookup.Items
                .Where(x => x.Id.Equals(projectOrderId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _projectOrderDetailAppService.CreateAsync(
                    ObjectMapper.Map<CreateProjectOrderDetailViewModel, ProjectOrderDetailCreateDto>(ProjectOrderDetail)
                    );
                return NoContent();

            }
            catch (Exception)
            {
                throw new UserFriendlyException($"Posting Error");
            }
        }
        public class CreateProjectOrderDetailViewModel
        {
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
