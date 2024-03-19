using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Notes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Note
{
    public class UpdateModel : IndoPageModel
    {
        [BindProperty]
        public NoteUpdateViewModel Note { get; set; }
        public List<SelectListItem> Customers { get; set; }

        private readonly INoteAppService _noteAppService;
        public UpdateModel(INoteAppService noteAppService)
        {
            _noteAppService = noteAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _noteAppService.GetAsync(id);
            Note = ObjectMapper.Map<NoteReadDto, NoteUpdateViewModel>(dto);


            var customerLookup = await _noteAppService.GetCustomerLookupAsync();
            Customers = customerLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _noteAppService.UpdateAsync(
                    Note.Id,
                    ObjectMapper.Map<NoteUpdateViewModel, NoteUpdateDto>(Note)
                );
                return NoContent();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException($"{ex.InnerException.Message}");
            }
        }

        public class NoteUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [SelectItems(nameof(Customers))]
            [DisplayName("Customer")]
            public Guid CustomerId { get; set; }

            [Required]
            [TextArea]
            public string Description { get; set; }
        }
    }
}
