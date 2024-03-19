using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.SalesOrders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.SalesOrder
{
    public class UpdateModel : IndoPageModel
    {
        [BindProperty]
        public SalesOrderUpdateViewModel SalesOrder { get; set; }
        public List<SelectListItem> Customers { get; set; }
        public List<SelectListItem> SalesExecutives { get; set; }

        private readonly ISalesOrderAppService _salesOrderAppService;
        public UpdateModel(ISalesOrderAppService salesOrderAppService)
        {
            _salesOrderAppService = salesOrderAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _salesOrderAppService.GetAsync(id);
            SalesOrder = ObjectMapper.Map<SalesOrderReadDto, SalesOrderUpdateViewModel>(dto);

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
                await _salesOrderAppService.UpdateAsync(
                    SalesOrder.Id,
                    ObjectMapper.Map<SalesOrderUpdateViewModel, SalesOrderUpdateDto>(SalesOrder)
                );
                return NoContent();

            }
            catch (SalesOrderAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }

        public class SalesOrderUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

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
