using Indo.Projectes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Project
{
    public class UpdateModel : IndoPageModel
    {
        [BindProperty]
        public ProjectUpdateViewModel Project { get; set; }
        public List<SelectListItem> Clients { get; set; }
        private readonly IProjectsAppService _projectsAppService;

        public UpdateModel(IProjectsAppService projectsAppService)
        {
            _projectsAppService = projectsAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _projectsAppService.GetAsync(id);
            Project = ObjectMapper.Map<ProjectsReadDto, ProjectUpdateViewModel>(dto);

            var clientsLookup = await _projectsAppService.GetClientLookupAsync();
            Clients = clientsLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _projectsAppService.UpdateAsync(
                    Project.Id,
                    ObjectMapper.Map<ProjectUpdateViewModel, ProjectsUpdateDto>(Project)
                );
                return NoContent();

            }
            catch (ProjectsAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class ProjectUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            [Required]
            [StringLength(ProjectsConsts.MaxNameLength)]
            public string Name { get; set; }

            [SelectItems(nameof(Clients))]
            [DisplayName("Client")]
            public Guid ClientsId { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [DisplayName("Start Date")]
            public DateTime StartDate { get; set; }
            [Required]
            [DataType(DataType.Date)]
            [DisplayName("End Date")]
            public DateTime EndDate { get; set; }
            public long Estimate { get; set; }
            [TextArea]
            public string Notes { get; set; }
            public string Technology { get; set; }
        }
    }
}
