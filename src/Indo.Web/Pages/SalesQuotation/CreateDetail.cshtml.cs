using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.SalesQuotationDetails;
using Indo.SalesQuotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.SalesQuotation
{
    public class CreateDetailModel : IndoPageModel
    {
        [BindProperty]
        public CreateSalesQuotationDetailViewModel SalesQuotationDetail { get; set; }
        public List<SelectListItem> SalesQuotations { get; set; }
        public List<SelectListItem> Products { get; set; }

        private readonly ISalesQuotationDetailAppService _salesQuotationDetailAppService;
        public CreateDetailModel(ISalesQuotationDetailAppService salesQuotationDetailAppService)
        {
            _salesQuotationDetailAppService = salesQuotationDetailAppService;
        }
        public async Task OnGetAsync(Guid salesQuotationId)
        {
            SalesQuotationDetail = new CreateSalesQuotationDetailViewModel();
            SalesQuotationDetail.SalesQuotationId = salesQuotationId;

            var salesQuotationLookup = await _salesQuotationDetailAppService.GetSalesQuotationLookupAsync();
            SalesQuotations = salesQuotationLookup.Items
                .Where(x => x.Id.Equals(salesQuotationId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();

            var productLookup = await _salesQuotationDetailAppService.GetProductLookupAsync();
            Products = productLookup.Items
                .Select(x => new SelectListItem($"{x.Name} [Price: {x.RetailPrice.ToString("##,##.00")}]", x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _salesQuotationDetailAppService.CreateAsync(
                    ObjectMapper.Map<CreateSalesQuotationDetailViewModel, SalesQuotationDetailCreateDto>(SalesQuotationDetail)
                    );
                return NoContent();

            }
            catch (Exception)
            {
                throw new UserFriendlyException($"Posting Error");
            }
        }
        public class CreateSalesQuotationDetailViewModel
        {
            [Required]
            [SelectItems(nameof(SalesQuotations))]
            [DisplayName("Sales Quotations")]
            public Guid SalesQuotationId { get; set; }

            [Required]
            [SelectItems(nameof(Products))]
            [DisplayName("Product")]
            public Guid ProductId { get; set; }

            [Required]
            public float Quantity { get; set; }

            [DisplayName("Discount Amount")]
            public float DiscAmt { get; set; }
        }
    }
}
