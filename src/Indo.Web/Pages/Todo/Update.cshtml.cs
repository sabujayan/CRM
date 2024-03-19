using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Indo.Todos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Todo
{
    public class UpdateModel : IndoPageModel
    {        

        [BindProperty]
        public TodoUpdateViewModel Todo { get; set; }

        private readonly ITodoAppService _todoAppService;
        public UpdateModel(ITodoAppService todoAppService)
        {
            _todoAppService = todoAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _todoAppService.GetAsync(id);
            Todo = ObjectMapper.Map<TodoReadDto, TodoUpdateViewModel>(dto);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _todoAppService.UpdateAsync(
                    Todo.Id,
                    ObjectMapper.Map<TodoUpdateViewModel, TodoUpdateDto>(Todo)
                    );
                return NoContent();
            }
            catch (TodoAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class TodoUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [StringLength(TodoConsts.MaxNameLength)]
            public string Name { get; set; }

            [TextArea]
            public string Description { get; set; }

            [Required]
            [DisplayName("Start Date Time")]
            public DateTime StartTime { get; set; }

            [Required]
            [DisplayName("End Date Time")]
            public DateTime EndTime { get; set; }

            [DisplayName("Is Done?")]
            public bool IsDone { get; set; }

            public string Location { get; set; }
        }
    }
}
