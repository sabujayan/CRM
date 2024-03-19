using Indo.Projectes;
using Indo.Technologies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;

namespace Indo.Web.Pages.Technologies
{
    public class EditModel : IndoPageModel
    {
        [BindProperty]
        public TechnologyUpdateViewModel tech { get; set; }

        private readonly ITechnologyAppService _technologyAppService;
        public List<SelectListItem> ParentTechnologies { get; set; }
        public EditModel(ITechnologyAppService technologyAppService)
        {
            _technologyAppService = technologyAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _technologyAppService.GetAsync(id);
            if(dto.ParentId==Guid.Empty)
            {
                dto.ParentStatus = "fail";
            }
            else
            {
                dto.ParentStatus = "success";
            }
            tech = ObjectMapper.Map<TechnologyReadDto, TechnologyUpdateViewModel>(dto);

            var x = await _technologyAppService.GetTechnologyLookupAsync();
            ParentTechnologies = x.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _technologyAppService.UpdateAsync(
                    tech.Id,
                    ObjectMapper.Map<TechnologyUpdateViewModel, TechnologyUpdateDto>(tech)
                );
                return Redirect("/Technologies");

            }
            catch (TechnologyAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }

        public class TechnologyUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            [Required]
            [StringLength(TechnologyConsts.MaxNameLength)]
            public string Name { get; set; }
            public string Description { get; set; }
            public Guid ParentId { get; set; }
            public string ParentStatus { get; set; }


        }
    }
}
