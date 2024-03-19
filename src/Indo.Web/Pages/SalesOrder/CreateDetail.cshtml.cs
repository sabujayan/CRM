using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.SalesOrderDetails;
using Indo.SalesOrders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.SalesOrder
{
    public class CreateDetailModel : IndoPageModel
    {
        [BindProperty]
        public CreateSalesOrderDetailViewModel SalesOrderDetail { get; set; }
        public List<SelectListItem> SalesOrders { get; set; }
        public List<SelectListItem> Products { get; set; }

        private readonly ISalesOrderDetailAppService _salesOrderDetailAppService;
        public CreateDetailModel(ISalesOrderDetailAppService salesOrderDetailAppService)
        {
            _salesOrderDetailAppService = salesOrderDetailAppService;
        }
        public async Task OnGetAsync(Guid salesOrderId)
        {
            SalesOrderDetail = new CreateSalesOrderDetailViewModel();
            SalesOrderDetail.SalesOrderId = salesOrderId;

            var salesOrderLookup = await _salesOrderDetailAppService.GetSalesOrderLookupAsync();
            SalesOrders = salesOrderLookup.Items
                .Where(x => x.Id.Equals(salesOrderId))
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
                await _salesOrderDetailAppService.CreateAsync(
                    ObjectMapper.Map<CreateSalesOrderDetailViewModel, SalesOrderDetailCreateDto>(SalesOrderDetail)
                    );
                return NoContent();

            }
            catch (Exception)
            {
                throw new UserFriendlyException($"Posting Error");
            }
        }
        public class CreateSalesOrderDetailViewModel
        {
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
