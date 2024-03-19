using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.SalesOrderDetails;
using Indo.SalesOrders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.SalesOrder
{
    public class UpdateDetailModel : IndoPageModel
    {
        [BindProperty]
        public SalesOrderDetailUpdateViewModel SalesOrderDetail { get; set; }
        public List<SelectListItem> SalesOrders { get; set; }
        public List<SelectListItem> Products { get; set; }
        public SalesOrderStatus Status { get; set; }

        private readonly ISalesOrderDetailAppService _salesOrderDetailAppService;
        public UpdateDetailModel(ISalesOrderDetailAppService salesOrderDetailAppService)
        {
            _salesOrderDetailAppService = salesOrderDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _salesOrderDetailAppService.GetAsync(id);
            Status = dto.Status;
            SalesOrderDetail = ObjectMapper.Map<SalesOrderDetailReadDto, SalesOrderDetailUpdateViewModel>(dto);

            var salesOrderLookup = await _salesOrderDetailAppService.GetSalesOrderLookupAsync();
            SalesOrders = salesOrderLookup.Items
                .Where(x => x.Id.Equals(SalesOrderDetail.SalesOrderId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();

            var productLookup = await _salesOrderDetailAppService.GetProductLookupAsync();
            Products = productLookup.Items
                .Select(x => new SelectListItem($"{x.Name} [Retail Price: {x.RetailPrice.ToString("##,##.00")}]", x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _salesOrderDetailAppService.UpdateAsync(
                    SalesOrderDetail.Id,
                    ObjectMapper.Map<SalesOrderDetailUpdateViewModel, SalesOrderDetailUpdateDto>(SalesOrderDetail)
                );
                return NoContent();

            }
            catch (Exception)
            {
                throw new UserFriendlyException($"Posting Error");
            }
        }

        public class SalesOrderDetailUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [SelectItems(nameof(SalesOrders))]
            [DisplayName("Sales Order")]
            public Guid SalesOrderId { get; set; }

            [Required]
            [SelectItems(nameof(Products))]
            [DisplayName("Product")]
            public Guid ProductId { get; set; }

            [Required]
            [Range(0f, float.MaxValue, ErrorMessage = "Value should be greater than or equal to 0")]
            public float Quantity { get; set; }

            [Range(0f, float.MaxValue, ErrorMessage = "Value should be greater than or equal to 0")]
            [DisplayName("Discount Amount")]
            public float DiscAmt { get; set; }
        }
    }
}
