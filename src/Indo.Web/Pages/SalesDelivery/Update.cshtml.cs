using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.SalesDeliveries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.SalesDelivery
{
    public class UpdateModel : IndoPageModel
    {
        [BindProperty]
        public SalesDeliveryUpdateViewModel SalesDelivery { get; set; }
        public List<SelectListItem> SalesOrders { get; set; }

        private readonly ISalesDeliveryAppService _salesDeliveryAppService;
        public UpdateModel(ISalesDeliveryAppService salesDeliveryAppService)
        {
            _salesDeliveryAppService = salesDeliveryAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _salesDeliveryAppService.GetAsync(id);
            SalesDelivery = ObjectMapper.Map<SalesDeliveryReadDto, SalesDeliveryUpdateViewModel>(dto);

            var salesOrderLookup = await _salesDeliveryAppService.GetSalesOrderLookupAsync();
            SalesOrders = salesOrderLookup.Items
                .Where(x => x.Id.Equals(SalesDelivery.SalesOrderId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _salesDeliveryAppService.UpdateAsync(
                    SalesDelivery.Id,
                    ObjectMapper.Map<SalesDeliveryUpdateViewModel, SalesDeliveryUpdateDto>(SalesDelivery)
                );
                return NoContent();

            }
            catch (SalesDeliveryAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }

        public class SalesDeliveryUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [SelectItems(nameof(SalesOrders))]
            [DisplayName("Sales Order")]
            public Guid SalesOrderId { get; set; }

            [Required]
            [StringLength(SalesDeliveryConsts.MaxNumberLength)]
            public string Number { get; set; }
            [TextArea]
            public string Description { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [DisplayName("Delivery Date")]
            public DateTime DeliveryDate { get; set; }
        }
    }
}
