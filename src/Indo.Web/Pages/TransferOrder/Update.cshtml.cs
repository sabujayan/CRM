using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.TransferOrders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.TransferOrder
{
    public class UpdateModel : IndoPageModel
    {
        [BindProperty]
        public TransferOrderUpdateViewModel TransferOrder { get; set; }
        public List<SelectListItem> FromWarehouse { get; set; }
        public List<SelectListItem> ToWarehouse { get; set; }

        private readonly ITransferOrderAppService _transferOrderAppService;
        public UpdateModel(ITransferOrderAppService transferOrderAppService)
        {
            _transferOrderAppService = transferOrderAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _transferOrderAppService.GetAsync(id);
            TransferOrder = ObjectMapper.Map<TransferOrderReadDto, TransferOrderUpdateViewModel>(dto);

            var fromLookup = await _transferOrderAppService.GetWarehouseLookupAsync();
            FromWarehouse = fromLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var toLookup = await _transferOrderAppService.GetWarehouseLookupAsync();
            ToWarehouse = toLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _transferOrderAppService.UpdateAsync(
                    TransferOrder.Id,
                    ObjectMapper.Map<TransferOrderUpdateViewModel, TransferOrderUpdateDto>(TransferOrder)
                );
                return NoContent();

            }
            catch (TransferOrderAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }

        public class TransferOrderUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }


            [Required]
            [SelectItems(nameof(FromWarehouse))]
            [DisplayName("From Warehouse")]
            public Guid FromWarehouseId { get; set; }

            [Required]
            [SelectItems(nameof(ToWarehouse))]
            [DisplayName("To Warehouse")]
            public Guid ToWarehouseId { get; set; }

            [Required]
            [StringLength(TransferOrderConsts.MaxNumberLength)]
            public string Number { get; set; }

            [TextArea]
            public string Description { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [DisplayName("Transfer Date")]
            public DateTime OrderDate { get; set; }
        }
    }
}
