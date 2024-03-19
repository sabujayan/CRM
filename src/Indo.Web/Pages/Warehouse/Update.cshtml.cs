using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Indo.Warehouses;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Warehouse
{
    public class UpdateModel : IndoPageModel
    {        

        [BindProperty]
        public WarehouseUpdateViewModel Warehouse { get; set; }

        private readonly IWarehouseAppService _warehouseAppService;
        public UpdateModel(IWarehouseAppService warehouseAppService)
        {
            _warehouseAppService = warehouseAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _warehouseAppService.GetAsync(id);
            Warehouse = ObjectMapper.Map<WarehouseReadDto, WarehouseUpdateViewModel>(dto);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _warehouseAppService.UpdateAsync(
                    Warehouse.Id,
                    ObjectMapper.Map<WarehouseUpdateViewModel, WarehouseUpdateDto>(Warehouse)
                    );
                return NoContent();
            }
            catch (WarehouseAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class WarehouseUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [StringLength(WarehouseConsts.MaxNameLength)]
            public string Name { get; set; }

            [TextArea]
            public string Description { get; set; }
        }
    }
}
