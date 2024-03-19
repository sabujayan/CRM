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
    public class AddModel : IndoPageModel
    {
        [BindProperty]
        public CreateProjectViewModel Project { get; set; }
        public List<SelectListItem> Clients { get; set; }
        private readonly IProjectsAppService _projectsAppService;
        public List<SelectListItem> Technologys { get; set; }

        public AddModel(IProjectsAppService projectsAppService)
        {
            _projectsAppService = projectsAppService;
        }
        public async Task OnGetAsync()
        {
            Project = new CreateProjectViewModel();
            var clientsLookup = await _projectsAppService.GetClientLookupAsync();
            Clients = clientsLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var techlookup = await _projectsAppService.GetTechnologyLookupAsync();
            Technologys = techlookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _projectsAppService.CreateAsync(
                    ObjectMapper.Map<CreateProjectViewModel, ProjectsCreateDto>(Project)
                    );
                return Redirect("/Project");

            }
            catch (ProjectsAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }

        }

        public class CreateProjectViewModel
        {
            public string Name { get; set; }

            [Required]
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
            public float EstimateHours { get; set; }
            [TextArea]
            public string Notes { get; set; }
            public string Technology { get; set; }

            [SelectItems(nameof(Technologys))]
            [DisplayName("Technology")]
            public Guid TechnologyId { get; set; }
            public string technologynameist { get; set; }
           
        }
    }
}
