using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.NumberSequences;
using Indo.ServiceOrders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.ServiceOrder
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateServiceOrderViewModel ServiceOrder { get; set; }
        public List<SelectListItem> Customers { get; set; }
        public List<SelectListItem> SalesExecutives { get; set; }

        private readonly IServiceOrderAppService _serviceOrderAppService;

        private readonly INumberSequenceAppService _numberSequenceAppService;
        public CreateModel(
            IServiceOrderAppService serviceOrderAppService,
            INumberSequenceAppService numberSequenceAppService
            )
        {
            _serviceOrderAppService = serviceOrderAppService;
            _numberSequenceAppService = numberSequenceAppService;
        }
        public async Task OnGetAsync()
        {
            ServiceOrder = new CreateServiceOrderViewModel();
            ServiceOrder.OrderDate = DateTime.Now;
            ServiceOrder.Number = await _numberSequenceAppService.GetNextNumberAsync(NumberSequenceModules.ServiceOrder);

            var customerLookup = await _serviceOrderAppService.GetCustomerLookupAsync();
            Customers = customerLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var salesExectuiveLookup = await _serviceOrderAppService.GetSalesExecutiveLookupAsync();
            SalesExecutives = salesExectuiveLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _serviceOrderAppService.CreateAsync(
                    ObjectMapper.Map<CreateServiceOrderViewModel, ServiceOrderCreateDto>(ServiceOrder)
                    );
                return NoContent();

            }
            catch (ServiceOrderAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateServiceOrderViewModel
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
            [StringLength(ServiceOrderConsts.MaxNumberLength)]
            public string Number { get; set; }

            [TextArea]
            public string Description { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [DisplayName("Order Date")]
            public DateTime OrderDate { get; set; }
        }
    }
}
