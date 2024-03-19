using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Warehouses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Warehouse
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateWarehouseViewModel Warehouse { get; set; }

        private readonly IWarehouseAppService _warehouseAppService;
        public CreateModel(IWarehouseAppService warehouseAppService)
        {
            _warehouseAppService = warehouseAppService;
        }
        public void OnGet()
        {
            Warehouse = new CreateWarehouseViewModel();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var dto = ObjectMapper.Map<CreateWarehouseViewModel, WarehouseCreateDto>(Warehouse);
                await _warehouseAppService.CreateAsync(dto);
                return NoContent();
            }
            catch (WarehouseAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateWarehouseViewModel
        {
            [Required]
            [StringLength(WarehouseConsts.MaxNameLength)]
            public string Name { get; set; }

            [TextArea]
            public string Description { get; set; }
        }
    }
}
