using Indo.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Threading.Tasks;
using System;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp;
using System.Linq;
using Indo.Clientes;
using Indo.Projectes;

namespace Indo.Web.Pages.Employee
{
    public class EditModel : IndoPageModel
    {
        [BindProperty]
        public EditEmployeeViewModel Employee { get; set; }
        public List<SelectListItem> Departments { get; set; }
        public List<SelectListItem> Skills { get; set; }
        public List<SelectListItem> Clients { get; set; }
        public List<SelectListItem> Projects { get; set; }

        private readonly IEmployeeAppService _employeeAppService;

        public EditModel(IEmployeeAppService employeeAppService)
        {
            _employeeAppService = employeeAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _employeeAppService.GetAsync(id);
            Employee = ObjectMapper.Map<EmployeeReadDto, EditEmployeeViewModel>(dto);

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
                    ObjectMapper.Map<EditEmployeeViewModel, EmployeeUpdateDto>(Employee)
                    );
                return Redirect("/Employee");


            }
            catch (EmployeeAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }  
        }
        public class EditEmployeeViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [StringLength(EmployeeConsts.MaxNameLength)]
            public string Name { get; set; }

            [Required]
            [StringLength(EmployeeConsts.MaxNameLength)]
            [DisplayName("Employee ID #")]
            public string EmployeeNumber { get; set; }
            public string Position { get; set; }

            [SelectItems(nameof(Departments))]
            [DisplayName("Department")]
            public Guid DepartmentId { get; set; }

            [DisplayName("Employee Group")]
            public EmployeeGroup EmployeeGroup { get; set; }
            [TextArea]
            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public List<Guid> SkillId { get; set; }
            public string ProjectID { get; set; }
            public string SkillID { get; set; }
            public string ClientID { get; set; }
            public List<Guid> ClientsId { get; set; }
            public string Bandwidth { get; set; }
            public string skilllist { get; set; }
            public string clientnameist { get; set; }
            public List<Guid> ProjectsId { get; set; }
            public string projectnameist { get; set; }
        }
    }
}