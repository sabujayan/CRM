using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.DeliveryOrderDetails;
using Indo.DeliveryOrders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.DeliveryOrder
{
    public class CreateDetailModel : IndoPageModel
    {
        [BindProperty]
        public CreateDeliveryOrderDetailViewModel DeliveryOrderDetail { get; set; }
        public List<SelectListItem> DeliveryOrders { get; set; }
        public List<SelectListItem> Products { get; set; }

        private readonly IDeliveryOrderDetailAppService _deliveryOrderDetailAppService;
        public CreateDetailModel(IDeliveryOrderDetailAppService deliveryOrderDetailAppService)
        {
            _deliveryOrderDetailAppService = deliveryOrderDetailAppService;
        }
        public async Task OnGetAsync(Guid deliveryOrderId)
        {
            DeliveryOrderDetail = new CreateDeliveryOrderDetailViewModel();
            DeliveryOrderDetail.DeliveryOrderId = deliveryOrderId;

            var deliveryOrderLookup = await _deliveryOrderDetailAppService.GetDeliveryOrderLookupAsync();
            DeliveryOrders = deliveryOrderLookup.Items
                .Where(x => x.Id.Equals(deliveryOrderId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();

            var productLookup = await _deliveryOrderDetailAppService.GetProductByDeliveryOrderLookupAsync(deliveryOrderId);
            Products = productLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _deliveryOrderDetailAppService.CreateAsync(
                    ObjectMapper.Map<CreateDeliveryOrderDetailViewModel, DeliveryOrderDetailCreateDto>(DeliveryOrderDetail)
                    );
                return NoContent();

            }
            catch (Exception)
            {
                throw new UserFriendlyException($"Posting Error");
            }
        }
        public class CreateDeliveryOrderDetailViewModel
        {
            [Required]
            [SelectItems(nameof(DeliveryOrders))]
            [DisplayName("Delivery Order")]
            public Guid DeliveryOrderId { get; set; }

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
