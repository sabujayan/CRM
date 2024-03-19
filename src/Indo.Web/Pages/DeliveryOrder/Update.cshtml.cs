using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.DeliveryOrders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.DeliveryOrder
{
    public class UpdateModel : IndoPageModel
    {
        [BindProperty]
        public DeliveryOrderUpdateViewModel DeliveryOrder { get; set; }
        public List<SelectListItem> FromWarehouse { get; set; }
        public List<SelectListItem> ToWarehouse { get; set; }
        public List<SelectListItem> TransferOrder { get; set; }

        private readonly IDeliveryOrderAppService _deliveryOrderAppService;
        public UpdateModel(IDeliveryOrderAppService deliveryOrderAppService)
        {
            _deliveryOrderAppService = deliveryOrderAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _deliveryOrderAppService.GetAsync(id);
            DeliveryOrder = ObjectMapper.Map<DeliveryOrderReadDto, DeliveryOrderUpdateViewModel>(dto);

            var fromLookup = await _deliveryOrderAppService.GetWarehouseLookupAsync();
            FromWarehouse = fromLookup.Items
                .Where(x => x.Id.Equals(dto.FromWarehouseId))
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var toLookup = await _deliveryOrderAppService.GetWarehouseLookupAsync();
            ToWarehouse = toLookup.Items
                .Where(x => x.Id.Equals(dto.ToWarehouseId))
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var transferOrderLookup = await _deliveryOrderAppService.GetTransferOrderLookupAsync();
            TransferOrder = transferOrderLookup.Items
                .Where(x => x.Id.Equals(dto.TransferOrderId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _deliveryOrderAppService.UpdateAsync(
                    DeliveryOrder.Id,
                    ObjectMapper.Map<DeliveryOrderUpdateViewModel, DeliveryOrderUpdateDto>(DeliveryOrder)
                );
                return NoContent();

            }
            catch (DeliveryOrderAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }

        public class DeliveryOrderUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }


            [Required]
            [SelectItems(nameof(TransferOrder))]
            [DisplayName("Transfer Order")]
            public Guid TransferOrderId { get; set; }


            [Required]
            [SelectItems(nameof(FromWarehouse))]
            [DisplayName("From Warehouse")]
            public Guid FromWarehouseId { get; set; }


            [Required]
            [SelectItems(nameof(ToWarehouse))]
            [DisplayName("To Warehouse")]
            public Guid ToWarehouseId { get; set; }

            [Required]
            [StringLength(DeliveryOrderConsts.MaxNumberLength)]
            public string Number { get; set; }

            [TextArea]
            public string Description { get; set; }

            [Required]
            [DisplayName("Delivery Date")]
            public DateTime OrderDate { get; set; }
        }
    }
}
