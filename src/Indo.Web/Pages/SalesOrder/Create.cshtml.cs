using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.NumberSequences;
using Indo.SalesOrders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.SalesOrder
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateSalesOrderViewModel SalesOrder { get; set; }
        public List<SelectListItem> Customers { get; set; }
        public List<SelectListItem> SalesExecutives { get; set; }

        private readonly ISalesOrderAppService _salesOrderAppService;

        private readonly INumberSequenceAppService _numberSequenceAppService;
        public CreateModel(
            ISalesOrderAppService salesOrderAppService,
            INumberSequenceAppService numberSequenceAppService
            )
        {
            _salesOrderAppService = salesOrderAppService;
            _numberSequenceAppService = numberSequenceAppService;
        }
        public async Task OnGetAsync()
        {
            SalesOrder = new CreateSalesOrderViewModel();
            SalesOrder.OrderDate = DateTime.Now;
            SalesOrder.Number = await _numberSequenceAppService.GetNextNumberAsync(NumberSequenceModules.SalesOrder);

            var customerLookup = await _salesOrderAppService.GetCustomerLookupAsync();
            Customers = customerLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var salesExectuiveLookup = await _salesOrderAppService.GetSalesExecutiveLookupAsync();
            SalesExecutives = salesExectuiveLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _salesOrderAppService.CreateAsync(
                    ObjectMapper.Map<CreateSalesOrderViewModel, SalesOrderCreateDto>(SalesOrder)
                    );
                return NoContent();

            }
            catch (SalesOrderAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateSalesOrderViewModel
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
            [StringLength(SalesOrderConsts.MaxNumberLength)]
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
