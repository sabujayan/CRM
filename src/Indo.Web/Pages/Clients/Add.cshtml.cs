using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Clientes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Clients
{
    public class AddModel : IndoPageModel
    {
        [BindProperty]
        public CreateClientViewModel Client { get; set; }
        private readonly IClientsAppService _clientsappService;
        public List<SelectListItem> Projects { get; set; }
        public AddModel(IClientsAppService clientsappService)
        {
            _clientsappService = clientsappService;
        }
        public async Task OnGetAsync()
        {
            Client = new CreateClientViewModel();
            var projectlookup = await _clientsappService.GetProjectLookupAsync();
            Projects = projectlookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var dto = ObjectMapper.Map<CreateClientViewModel, ClientsCreateDto>(Client);
                await _clientsappService.CreateAsync(dto);
                return Redirect("/Clients");
            }
            catch (ClientsAlreadyExistsException ex)
            {
                ex.ToString();
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateClientViewModel
        {
            public string Name { get; set; }
            public string Address { get; set; }
            public string Country { get; set; }
            public string State { get; set; }
            public string City { get; set; }
            public string Zip { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public Guid ProjectsId { get; set; }
            public string projectnameist { get; set; }

        }
    }
}
