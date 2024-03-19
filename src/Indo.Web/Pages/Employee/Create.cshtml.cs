using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Employee
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateEmployeeViewModel Employee { get; set; }
        public List<SelectListItem> Departments { get; set; }
        public List<SelectListItem> Skills { get; set; }
        public List<SelectListItem> Clients { get; set; }
        public List<SelectListItem> Projects { get; set; }

        private readonly IEmployeeAppService _employeeAppService;

        public CreateModel(IEmployeeAppService employeeAppService)
        {
            _employeeAppService = employeeAppService;
        }
        public async Task OnGetAsync()
        {
            Employee = new CreateEmployeeViewModel();

            var departmentLookup = await _employeeAppService.GetDepartmentLookupAsync();
            Departments = departmentLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var skilllookup = await _employeeAppService.GetSkillLookupAsync();
            Skills = skilllookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var clientlookup = await _employeeAppService.GetClientLookupAsync();
            Clients = clientlookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var projectlookup = await _employeeAppService.GetProjectLookupAsync();
            Projects = projectlookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _employeeAppService.CreateAsync(
                    ObjectMapper.Map<CreateEmployeeViewModel, EmployeeCreateDto>(Employee)
                    );
                return NoContent();
            }
            catch (EmployeeAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateEmployeeViewModel
        {
            [SelectItems(nameof(Departments))]
            [DisplayName("Department")]
            public Guid DepartmentId { get; set; }

            [Required]
            [StringLength(EmployeeConsts.MaxNameLength)]
            public string Name { get; set; }

            [Required]
            [StringLength(EmployeeConsts.MaxNameLength)]
            [DisplayName("Employee ID #")]
            public string EmployeeNumber { get; set; }
            public string Position { get; set; }

            [DisplayName("Employee Group")]
            public EmployeeGroup EmployeeGroup { get; set; }
            [TextArea]
            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }

            [SelectItems(nameof(Skills))]
            [DisplayName("Skill")]
            public Guid SkillId { get; set; }
            public string ProjectID { get; set; }
            public string ClientID { get; set; }

            [SelectItems(nameof(Clients))]
            [DisplayName("Clients")]
            public Guid ClientsId { get; set; }
            public string Bandwidth { get; set; }
            public string skilllist { get; set; }
            public string clientnameist { get; set; }
            [DisplayName("Projects")]
            public List<string> ProjectsId { get; set; }
            public string projectnameist { get; set; }
        }
    }
}
