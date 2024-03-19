using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.ServiceOrders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.ServiceOrder
{
    public class UpdateModel : IndoPageModel
    {
        [BindProperty]
        public ServiceOrderUpdateViewModel ServiceOrder { get; set; }
        public List<SelectListItem> Customers { get; set; }
        public List<SelectListItem> SalesExecutives { get; set; }

        private readonly IServiceOrderAppService _serviceOrderAppService;
        public UpdateModel(IServiceOrderAppService serviceOrderAppService)
        {
            _serviceOrderAppService = serviceOrderAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _serviceOrderAppService.GetAsync(id);
            ServiceOrder = ObjectMapper.Map<ServiceOrderReadDto, ServiceOrderUpdateViewModel>(dto);

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
                await _serviceOrderAppService.UpdateAsync(
                    ServiceOrder.Id,
                    ObjectMapper.Map<ServiceOrderUpdateViewModel, ServiceOrderUpdateDto>(ServiceOrder)
                );
                return NoContent();

            }
            catch (ServiceOrderAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }

        public class ServiceOrderUpdateViewModel
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
