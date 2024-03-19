using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Departments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Department
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateDepartmentViewModel Department { get; set; }

        private readonly IDepartmentAppService _departmentAppService;
        public CreateModel(IDepartmentAppService departmentAppService)
        {
            _departmentAppService = departmentAppService;
        }
        public void OnGet()
        {
            Department = new CreateDepartmentViewModel();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var dto = ObjectMapper.Map<CreateDepartmentViewModel,DepartmentCreateDto>(Department);
                await _departmentAppService.CreateAsync(dto);
                return NoContent();
            }
            catch (DepartmentAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateDepartmentViewModel
        {
            [Required]
            [StringLength(DepartmentConsts.MaxNameLength)]
            public string Name { get; set; }

            [TextArea]
            public string Description { get; set; }
        }
    }
}
