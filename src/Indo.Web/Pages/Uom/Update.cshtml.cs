using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Indo.Uoms;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Uom
{
    public class UpdateModel : IndoPageModel
    {        

        [BindProperty]
        public UomUpdateViewModel Uom { get; set; }

        private readonly IUomAppService _uomAppService;
        public UpdateModel(IUomAppService uomAppService)
        {
            _uomAppService = uomAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _uomAppService.GetAsync(id);
            Uom = ObjectMapper.Map<UomReadDto, UomUpdateViewModel>(dto);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _uomAppService.UpdateAsync(
                    Uom.Id,
                    ObjectMapper.Map<UomUpdateViewModel, UomUpdateDto>(Uom)
                    );
                return NoContent();
            }
            catch (UomAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class UomUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [StringLength(100)]
            public string Name { get; set; }

            [TextArea]
            public string Description { get; set; }
        }
    }
}
