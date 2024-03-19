using Indo.Projectes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Project
{
    public class IndexModel : IndoPageModel
    {
        [BindProperty]
        public IndexViewModel Project { get; set; }
        public PagedResultDto<ProjectListViewModel> ProjectList { get; set; }
        public List<SelectListItem> Clients { get; set; }
        public List<SelectListItem> Technologys { get; set; }

        private readonly IProjectsAppService _projectsAppService;
        public IndexModel(IProjectsAppService projectsAppService)
        {
            _projectsAppService = projectsAppService;

        }
        public async Task OnGetAsync()
        {

            var clientsLookup = await _projectsAppService.GetClientLookupAsync();
            Clients = clientsLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var techlookup = await _projectsAppService.GetTechnologyLookupAsync();
            Technologys = techlookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

        }
        public class IndexViewModel
        {
            [SelectItems(nameof(Clients))]
            [DisplayName("")]
            public Guid ClientsId { get; set; }

            [SelectItems(nameof(Technologys))]
            [DisplayName("")]
            public Guid TechnologyId { get; set; }
        }
        public class ProjectListViewModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public Guid ClientsId { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public long Estimate { get; set; }
            public string Notes { get; set; }
            public string Technology { get; set; }
            public string ClientName { get; set; }
            public string technologynameist { get; set; }
            public List<string> TechnologyProjectId { get; set; }
        }
    }
}
