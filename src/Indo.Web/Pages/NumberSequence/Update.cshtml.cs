using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Indo.NumberSequences;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace Indo.Web.Pages.NumberSequence
{
    public class UpdateModel : IndoPageModel
    {        

        [BindProperty]
        public NumberSequenceUpdateViewModel NumberSequence { get; set; }

        private readonly INumberSequenceAppService _numberSequenceAppService;
        public UpdateModel(INumberSequenceAppService numberSequenceAppService)
        {
            _numberSequenceAppService = numberSequenceAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _numberSequenceAppService.GetAsync(id);
            NumberSequence = ObjectMapper.Map<NumberSequenceReadDto, NumberSequenceUpdateViewModel>(dto);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _numberSequenceAppService.UpdateAsync(
                    NumberSequence.Id,
                    ObjectMapper.Map<NumberSequenceUpdateViewModel, NumberSequenceUpdateDto>(NumberSequence)
                    );
                return NoContent();
            }
            catch (NumberSequenceAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class NumberSequenceUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [StringLength(100)]
            public string Suffix { get; set; }
        }
    }
}
