using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Notes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Note
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateNoteViewModel Note { get; set; }
        public List<SelectListItem> Customers { get; set; }

        private readonly INoteAppService _noteAppService;
        public CreateModel(INoteAppService noteAppService)
        {
            _noteAppService = noteAppService;
        }
        public async Task OnGetAsync()
        {
            Note = new CreateNoteViewModel();

            var customerLookup = await _noteAppService.GetCustomerLookupAsync();
            Customers = customerLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _noteAppService.CreateAsync(
                    ObjectMapper.Map<CreateNoteViewModel, NoteCreateDto>(Note)
                    );
                return NoContent();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException($"{ex.InnerException.Message}");
            }
        }
        public class CreateNoteViewModel
        {
            [SelectItems(nameof(Customers))]
            [DisplayName("Customer")]
            public Guid CustomerId { get; set; }

            [Required]
            [TextArea]
            public string Description { get; set; }
        }
    }
}
