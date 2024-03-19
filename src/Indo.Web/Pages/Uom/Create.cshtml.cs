using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Uoms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Uom
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateUomViewModel Uom { get; set; }

        private readonly IUomAppService _uomAppService;
        public CreateModel(IUomAppService uomAppService)
        {
            _uomAppService = uomAppService;
        }
        public void OnGet()
        {
            Uom = new CreateUomViewModel();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var dto = ObjectMapper.Map<CreateUomViewModel, UomCreateDto>(Uom);
                await _uomAppService.CreateAsync(dto);
                return NoContent();
            }
            catch (UomAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateUomViewModel
        {
            [Required]
            [StringLength(UomConsts.MaxNameLength)]
            public string Name { get; set; }

            [TextArea]
            public string Description { get; set; }
        }
    }
}
