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
    public class AddModel : IndoPageModel
    {
        [BindProperty]
        public AddTechnologyViewModel tech { get; set; }
        private readonly ITechnologyAppService _technologyAppService;
        public List<SelectListItem> ParentTechnologies { get; set; }
        public AddModel(ITechnologyAppService technologyAppService)
        {
            _technologyAppService = technologyAppService;
        }
        public async Task OnGetAsync()
        {
            tech = new AddTechnologyViewModel();
            var technologylookup = await _technologyAppService.GetTechnologyLookupAsync();
            ParentTechnologies = technologylookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var dto = ObjectMapper.Map<AddTechnologyViewModel, TechnologyCreateDto>(tech);
                await _technologyAppService.CreateAsync(dto);
                return Redirect("/Technologies");

            }
            catch (TechnologyAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }

        }
        public class AddTechnologyViewModel
        {
            [Required]
            [StringLength(TechnologyConsts.MaxNameLength)]
            public string Name { get; set; }
            public string Description { get; set; }
            public Guid ParentId { get; set; }

        }
    }
}
