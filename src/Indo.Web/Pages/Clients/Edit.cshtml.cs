using Indo.Clientes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Clients
{
    public class EditModel : IndoPageModel
    {

        [BindProperty]
        public ClientEditViewModel Client { get; set; }
        private readonly IClientsAppService _clientsappService;
        public List<SelectListItem> Projects { get; set; }
        public EditModel(IClientsAppService clientsappService)
        {
            _clientsappService = clientsappService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _clientsappService.GetAsync(id);
            Client = ObjectMapper.Map<ClientsReadDto, ClientEditViewModel>(dto);

            var projectlookup = await _clientsappService.GetProjectLookupAsync();
            Projects = projectlookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _clientsappService.UpdateAsync(
                    Client.Id,
                    ObjectMapper.Map<ClientEditViewModel, ClientsUpdateDto>(Client)
                    );
                return Redirect("/Clients");
            }
            catch (ClientsAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }

        public class ClientEditViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
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
            public Guid ClientsId { get; set; }
            public Guid AddressId { get; set; }
            public Guid ContactId { get; set; }
            public List<string> ClientProjectId { get; set; }
        }
    }
}
