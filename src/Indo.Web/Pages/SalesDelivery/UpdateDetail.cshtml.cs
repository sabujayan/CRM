using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.SalesDeliveryDetails;
using Indo.SalesDeliveries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.SalesDelivery
{
    public class UpdateDetailModel : IndoPageModel
    {
        [BindProperty]
        public SalesDeliveryDetailUpdateViewModel SalesDeliveryDetail { get; set; }
        public List<SelectListItem> SalesDeliveries { get; set; }
        public List<SelectListItem> Products { get; set; }
        public SalesDeliveryStatus Status { get; set; }

        private readonly ISalesDeliveryDetailAppService _salesDeliveryDetailAppService;
        public UpdateDetailModel(ISalesDeliveryDetailAppService salesDeliveryDetailAppService)
        {
            _salesDeliveryDetailAppService = salesDeliveryDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _salesDeliveryDetailAppService.GetAsync(id);
            Status = dto.Status;
            SalesDeliveryDetail = ObjectMapper.Map<SalesDeliveryDetailReadDto, SalesDeliveryDetailUpdateViewModel>(dto);

            var salesDeliveryLookup = await _salesDeliveryDetailAppService.GetSalesDeliveryLookupAsync();
            SalesDeliveries = salesDeliveryLookup.Items
                .Where(x => x.Id.Equals(SalesDeliveryDetail.SalesDeliveryId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();

            var productLookup = await _salesDeliveryDetailAppService.GetProductBySalesDeliveryLookupAsync(SalesDeliveryDetail.SalesDeliveryId);
            Products = productLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _salesDeliveryDetailAppService.UpdateAsync(
                    SalesDeliveryDetail.Id,
                    ObjectMapper.Map<SalesDeliveryDetailUpdateViewModel, SalesDeliveryDetailUpdateDto>(SalesDeliveryDetail)
                );
                return NoContent();

            }
            catch (Exception)
            {
                throw new UserFriendlyException($"Posting Error");
            }
        }

        public class SalesDeliveryDetailUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [SelectItems(nameof(SalesDeliveries))]
            [DisplayName("Sales Delivery")]
            public Guid SalesDeliveryId { get; set; }

            [Required]
            [SelectItems(nameof(Products))]
            [DisplayName("Product")]
            public Guid ProductId { get; set; }

            [Required]
            [Range(0f, float.MaxValue, ErrorMessage = "Value should be greater than or equal to 0")]
            public float Quantity { get; set; }
        }
    }
}
