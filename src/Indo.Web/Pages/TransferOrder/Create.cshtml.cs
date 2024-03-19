using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.NumberSequences;
using Indo.TransferOrders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.TransferOrder
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateTransferOrderViewModel TransferOrder { get; set; }
        public List<SelectListItem> FromWarehouse { get; set; }
        public List<SelectListItem> ToWarehouse { get; set; }

        private readonly ITransferOrderAppService _transferOrderAppService;

        private readonly INumberSequenceAppService _numberSequenceAppService;
        public CreateModel(
            ITransferOrderAppService transferOrderAppService,
            INumberSequenceAppService numberSequenceAppService
            )
        {
            _transferOrderAppService = transferOrderAppService;
            _numberSequenceAppService = numberSequenceAppService;
        }
        public async Task OnGetAsync()
        {
            TransferOrder = new CreateTransferOrderViewModel();
            TransferOrder.OrderDate = DateTime.Now;
            TransferOrder.Number = await _numberSequenceAppService.GetNextNumberAsync(NumberSequenceModules.TransferOrder);

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
                await _transferOrderAppService.CreateAsync(
                    ObjectMapper.Map<CreateTransferOrderViewModel, TransferOrderCreateDto>(TransferOrder)
                    );
                return NoContent();

            }
            catch (TransferOrderAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateTransferOrderViewModel
        {
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
