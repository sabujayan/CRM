using Indo.Departments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp;
using Indo.Skills;

namespace Indo.Web.Pages.Skill
{
    public class UpdateModel :IndoPageModel
    {
        [BindProperty]
        public SkillUpdateViewModel Skill { get; set; }

        private readonly ISkillAppService _skillAppService;
        public UpdateModel(ISkillAppService skillAppService)
        {
            _skillAppService = skillAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _skillAppService.GetAsync(id);
            Skill = ObjectMapper.Map<SkillReadDto, SkillUpdateViewModel>(dto);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _skillAppService.UpdateAsync(
                    Skill.Id,
                    ObjectMapper.Map<SkillUpdateViewModel, SkillUpdateDto>(Skill)
                    );
                return NoContent();
            }
            catch (SkillAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class SkillUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [StringLength(DepartmentConsts.MaxNameLength)]
            public string Name { get; set; }
        }
    }
}
