using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.NumberSequences;
using Indo.Todos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Todo
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateTodoViewModel Todo { get; set; }

        private readonly ITodoAppService _todoAppService;

        private readonly INumberSequenceAppService _numberSequenceAppService;
        public CreateModel(
            ITodoAppService todoAppService,
            INumberSequenceAppService numberSequenceAppService
            )
        {
            _todoAppService = todoAppService;
            _numberSequenceAppService = numberSequenceAppService;
        }
        public async Task OnGet()
        {
            Todo = new CreateTodoViewModel();
            Todo.Name = await _numberSequenceAppService.GetNextNumberAsync(NumberSequenceModules.TodoList);
            Todo.StartTime = DateTime.Now;
            Todo.EndTime = Todo.StartTime.AddHours(1);
            Todo.IsDone = false;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var dto = ObjectMapper.Map<CreateTodoViewModel, TodoCreateDto>(Todo);
                await _todoAppService.CreateAsync(dto);
                return NoContent();
            }
            catch (TodoAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateTodoViewModel
        {
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
