using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Employee
{
    public class UpdateModel : IndoPageModel
    {
        [BindProperty]
        public EmployeeUpdateViewModel Employee { get; set; }
        public List<SelectListItem> Departments { get; set; }
        public List<SelectListItem> Skills { get; set; }
        public List<SelectListItem> Clients { get; set; }
        public List<SelectListItem> Projects { get; set; }

        private readonly IEmployeeAppService _employeeAppService;
        public UpdateModel(IEmployeeAppService employeeAppService)
        {
            _employeeAppService = employeeAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _employeeAppService.GetAsync(id);
            Employee = ObjectMapper.Map<EmployeeReadDto, EmployeeUpdateViewModel>(dto);

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
                await _employeeAppService.UpdateAsync(
                    Employee.Id,
                    ObjectMapper.Map<EmployeeUpdateViewModel, EmployeeUpdateDto>(Employee)
                );
                return Redirect("/Employee");

            }
            catch (EmployeeAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }

        public class EmployeeUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
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
            [DisplayName("Skills")]
            public List<string> EmployeeSkillId { get; set; }
            public Guid EmployeeSkillMatricesId { get; set; }
            public string ProjectID { get; set; }
            public string ClientID { get; set; }
            public string Bandwidth { get; set; }
            public string skilllist { get; set; }
            public string clientnamelist { get; set; }
            public string projectnamelist { get; set; }
            public string Skills { get; set; }

            [SelectItems(nameof(Clients))]
            [DisplayName("Clients")]
            public List<string> EmployeeClientId { get; set; }

            [SelectItems(nameof(Projects))]
            [DisplayName("Projects")]
            public List<string> EmployeeProjectId { get; set; }

        }
    }
}
