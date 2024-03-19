using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Indo.Departments;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Department
{
    public class UpdateModel : IndoPageModel
    {        

        [BindProperty]
        public DepartmentUpdateViewModel Department { get; set; }

        private readonly IDepartmentAppService _departmentAppService;
        public UpdateModel(IDepartmentAppService departmentAppService)
        {
            _departmentAppService = departmentAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _departmentAppService.GetAsync(id);
            Department = ObjectMapper.Map<DepartmentReadDto, DepartmentUpdateViewModel>(dto);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _departmentAppService.UpdateAsync(
                    Department.Id,
                    ObjectMapper.Map<DepartmentUpdateViewModel, DepartmentUpdateDto>(Department)
                    );
                return NoContent();
            }
            catch (DepartmentAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class DepartmentUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [StringLength(DepartmentConsts.MaxNameLength)]
            public string Name { get; set; }

            [TextArea]
            public string Description { get; set; }
        }
    }
}
