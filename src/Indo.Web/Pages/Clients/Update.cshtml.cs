using Indo.Clientes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Clients
{
    public class UpdateModel : IndoPageModel
    {
        [BindProperty]
        public ClientUpdateViewModel Client { get; set; }

        private readonly IClientsAppService _clientsappService;
        public UpdateModel(IClientsAppService clientsappService)
        {
            _clientsappService = clientsappService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _clientsappService.GetAsync(id);
            Client = ObjectMapper.Map<ClientsReadDto, ClientUpdateViewModel>(dto);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _clientsappService.UpdateAsync(
                    Client.Id,
                    ObjectMapper.Map<ClientUpdateViewModel, ClientsUpdateDto>(Client)
                    );
                return NoContent();
            }
            catch (ClientsAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class ClientUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [StringLength(ClientConsts.MaxNameLength)]
            public string Name { get; set; }
            [TextArea]
            public string Address { get; set; }
            public string Country { get; set; }
            public string State { get; set; }
            public string City { get; set; }
            public string Zip { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public Guid ClientsId { get; set; }
            public Guid AddressId { get; set; }

            public Guid ContactId { get; set; }

        }
    }
}
