

using Indo.Clientes;
using Indo.Employees;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Indo.Web.Pages.Employee
{
    public class IndexModel : IndoPageModel
    {
        [BindProperty]
        public PagedResultDto<EmployeeListViewModel> EmpList { get; set; }

        private readonly IEmployeeAppService _employeeAppService;
        public IndexModel(IEmployeeAppService employeeAppService)
        {
            _employeeAppService = employeeAppService;

        }
        public async Task OnGetAsync()
        {
            var input = new GetEmployeeInfoListDto
            {
                Filter = ""
            };
            var dto = await _employeeAppService.GetEmployeeList(input);
            EmpList = ObjectMapper.Map<PagedResultDto<EmployeeReadDto>,PagedResultDto<EmployeeListViewModel>>(dto);

        }
        public class EmployeeListViewModel
        {
            public Guid Id { get; set; }
            public Guid DepartmentId { get; set; }
            public string Name { get; set; }
            public string EmployeeNumber { get; set; }
            public string Position { get; set; }
            public EmployeeGroup EmployeeGroup { get; set; }
            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public Guid SkillId { get; set; }
            public List<string> EmployeeSkillId { get; set; }
            public Guid EmployeeSkillMatricesId { get; set; }
            public string ProjectID { get; set; }
            public string ClientID { get; set; }
            public string Bandwidth { get; set; }
            public string skilllist { get; set; }
            public string clientist { get; set; }
            public string projectnamelist { get; set; }
            public string Skills { get; set; }
            public string ProjectName { get; set; }
            public string ClientName { get; set; }
            public string DepartmentName { get; set; }
            public List<string> EmployeeClientId { get; set; }
            public List<string> EmployeeProjectId { get; set; }

        }
    }
}
