using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.TransferOrderDetails;
using Indo.TransferOrders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.TransferOrder
{
    public class CreateDetailModel : IndoPageModel
    {
        [BindProperty]
        public CreateTransferOrderDetailViewModel TransferOrderDetail { get; set; }
        public List<SelectListItem> TransferOrders { get; set; }
        public List<SelectListItem> Products { get; set; }

        private readonly ITransferOrderDetailAppService _transferOrderDetailAppService;
        public CreateDetailModel(ITransferOrderDetailAppService transferOrderDetailAppService)
        {
            _transferOrderDetailAppService = transferOrderDetailAppService;
        }
        public async Task OnGetAsync(Guid transferOrderId)
        {
            TransferOrderDetail = new CreateTransferOrderDetailViewModel();
            TransferOrderDetail.TransferOrderId = transferOrderId;

            var transferOrderLookup = await _transferOrderDetailAppService.GetTransferOrderLookupAsync();
            TransferOrders = transferOrderLookup.Items
                .Where(x => x.Id.Equals(transferOrderId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();

            var productLookup = await _transferOrderDetailAppService.GetProductLookupAsync();
            Products = productLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _transferOrderDetailAppService.CreateAsync(
                    ObjectMapper.Map<CreateTransferOrderDetailViewModel, TransferOrderDetailCreateDto>(TransferOrderDetail)
                    );
                return NoContent();

            }
            catch (Exception)
            {
                throw new UserFriendlyException($"Posting Error");
            }
        }
        public class CreateTransferOrderDetailViewModel
        {
            [Required]
            [SelectItems(nameof(TransferOrders))]
            [DisplayName("Transfer Order")]
            public Guid TransferOrderId { get; set; }

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
