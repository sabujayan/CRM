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

namespace Indo.Web.Pages.SalesExecutive
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateSalesExecutiveViewModel Employee { get; set; }
        public List<SelectListItem> Departments { get; set; }

        private readonly IEmployeeAppService _employeeAppService;
        public CreateModel(IEmployeeAppService employeeAppService)
        {
            _employeeAppService = employeeAppService;
        }
        public async Task OnGetAsync()
        {
            Employee = new CreateSalesExecutiveViewModel();

            Employee.EmployeeGroup = EmployeeGroup.Sales;

            var departmentLookup = await _employeeAppService.GetDepartmentLookupAsync();
            Departments = departmentLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                Employee.EmployeeGroup = EmployeeGroup.Sales;

                await _employeeAppService.CreateAsync(
                    ObjectMapper.Map<CreateSalesExecutiveViewModel, EmployeeCreateDto>(Employee)
                    );
                return NoContent();

            }
            catch (EmployeeAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateSalesExecutiveViewModel
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
            [InputInfoText("Un-Editable, default to Sales.")]
            public EmployeeGroup EmployeeGroup { get; set; }

            [TextArea]
            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
        }
    }
}
