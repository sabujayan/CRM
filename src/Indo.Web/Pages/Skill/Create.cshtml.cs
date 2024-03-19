using Indo.Departments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp;
using Indo.Skills;

namespace Indo.Web.Pages.Skill
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateSkillViewModel Skill { get; set; }

        private readonly ISkillAppService _skillAppService;
        public CreateModel(ISkillAppService skillAppService)
        {
            _skillAppService = skillAppService;
        }
        public void OnGet()
        {
            Skill = new CreateSkillViewModel();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var dto = ObjectMapper.Map<CreateSkillViewModel, SkillCreateDto>(Skill);
                await _skillAppService.CreateAsync(dto);
                return NoContent();
            }
            catch (SkillAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateSkillViewModel
        {
            [Required]
            [StringLength(DepartmentConsts.MaxNameLength)]
            public string Name { get; set; }
            
        }
    }
}
