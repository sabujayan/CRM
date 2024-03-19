using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Clientes;
using Indo.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Employee
{
    public class AddModel : IndoPageModel
    {
        [BindProperty]
        public CreateEmployeeViewModel Employee { get; set; }
        public List<SelectListItem> Departments { get; set; }
        public List<SelectListItem> Skills { get; set; }
        public List<SelectListItem> Clients { get; set; }
        public List<SelectListItem> Projects { get; set; }

        private readonly IEmployeeAppService _employeeAppService;

        public AddModel(IEmployeeAppService employeeAppService)
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
                var dto = ObjectMapper.Map<CreateEmployeeViewModel, EmployeeCreateDto>(Employee);
                await _employeeAppService.CreateAsync(dto);
                return Redirect("/Employee");
            }
            catch (EmployeeAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateEmployeeViewModel
        {

            [StringLength(EmployeeConsts.MaxNameLength)]
            public string Name { get; set; }

            [StringLength(EmployeeConsts.MaxNameLength)]
            public string EmployeeNumber { get; set; }
            public string Position { get; set; }
            [Required]
            [SelectItems(nameof(Departments))]
            [DisplayName("Department")]
            public Guid DepartmentId { get; set; }
            public EmployeeGroup EmployeeGroup { get; set; }
            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Bandwidth { get; set; }
            public string SkillList { get; set; }
            public string ClientNameList { get; set; }
            public string ProjectNameList { get; set; }
        }
    }
}
