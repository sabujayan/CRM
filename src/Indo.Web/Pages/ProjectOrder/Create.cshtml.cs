using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.NumberSequences;
using Indo.ProjectOrders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.ProjectOrder
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateProjectOrderViewModel ProjectOrder { get; set; }
        public List<SelectListItem> Customers { get; set; }
        public List<SelectListItem> SalesExecutives { get; set; }

        private readonly IProjectOrderAppService _projectOrderAppService;

        private readonly INumberSequenceAppService _numberSequenceAppService;
        public CreateModel(
            IProjectOrderAppService projectOrderAppService,
            INumberSequenceAppService numberSequenceAppService
            )
        {
            _projectOrderAppService = projectOrderAppService;
            _numberSequenceAppService = numberSequenceAppService;
        }
        public async Task OnGetAsync()
        {
            ProjectOrder = new CreateProjectOrderViewModel();
            ProjectOrder.OrderDate = DateTime.Now;
            ProjectOrder.Number = await _numberSequenceAppService.GetNextNumberAsync(NumberSequenceModules.ProjectOrder);

            var customerLookup = await _projectOrderAppService.GetCustomerLookupAsync();
            Customers = customerLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var salesExectuiveLookup = await _projectOrderAppService.GetSalesExecutiveLookupAsync();
            SalesExecutives = salesExectuiveLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _projectOrderAppService.CreateAsync(
                    ObjectMapper.Map<CreateProjectOrderViewModel, ProjectOrderCreateDto>(ProjectOrder)
                    );
                return NoContent();

            }
            catch (ProjectOrderAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateProjectOrderViewModel
        {
            [Required]
            [SelectItems(nameof(Customers))]
            [DisplayName("Customer")]
            public Guid CustomerId { get; set; }

            [Required]
            [SelectItems(nameof(SalesExecutives))]
            [DisplayName("Sales Executive")]
            public Guid SalesExecutiveId { get; set; }

            [Required]
            [StringLength(ProjectOrderConsts.MaxNumberLength)]
            public string Number { get; set; }

            [TextArea]
            public string Description { get; set; }

            [Required]
            public ProjectOrderRating Rating { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [DisplayName("Order Date")]
            public DateTime OrderDate { get; set; }
        }
    }
}
