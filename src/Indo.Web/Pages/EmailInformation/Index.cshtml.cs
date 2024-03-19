using Indo.EmailInformation;
using Indo.EmailsInformtions;
using Indo.Projectes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;

namespace Indo.Web.Pages.EmailInformation
{
    public class IndexModel : IndoPageModel
    {
        [BindProperty]
        public emailInformationViewModel EmailInformation { get; set; }
        public List<SelectListItem> Employees { get; set; }
        public List<SelectListItem> EmailTemplates { get; set; }

        private readonly IEmailInformationAppService _emailInformationsAppService;

        public IndexModel(IEmailInformationAppService emailInformationsAppService)
        {
            _emailInformationsAppService = emailInformationsAppService;
        }
        public async Task OnGetAsync()
        {
            EmailInformation = new emailInformationViewModel();

            var employeeLookup = await _emailInformationsAppService.GetEmployeeLookupAsync();
            Employees = employeeLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var template = await _emailInformationsAppService.GetEmailsTemplatesLookupAsync();
            EmailTemplates = template.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                 _emailInformationsAppService.SendEmail(
                    ObjectMapper.Map<emailInformationViewModel, EmailDto>(EmailInformation)
                    );
                return Redirect("/EmailInformation");

            }
            catch (EmailInformtionAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }

        }
        public class emailInformationViewModel
        {
            public string To { get; set; }
            public string Cc { get; set; }
            public string Bcc { get; set; }
            public Guid TemplateId { get; set; }
        }
    }
}
