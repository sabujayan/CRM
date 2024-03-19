using Indo.Clientes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Clients
{
    public class IndexModel : IndoPageModel
    {
        [BindProperty]
        public IndexViewModel Clients { get; set; }
        public PagedResultDto<ClientListViewModel> ClientList { get; set; }
        public List<SelectListItem> Projects { get; set; }

        private readonly IClientsAppService _clientsappService;

        public IndexModel(IClientsAppService clientsappService)
        {
            _clientsappService = clientsappService;
        }

        public async Task OnGetAsync()
        {
            var input = new GetClientInfoListDto
            {
                Filter = ""
            };

            var dto = await _clientsappService.GetClientList(input);
            ClientList = ObjectMapper.Map<PagedResultDto<ClientsReadDto>, PagedResultDto<ClientListViewModel>>(dto);

            var projectlookup = await _clientsappService.GetProjectLookupAsync();
            Projects = projectlookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }

        public class IndexViewModel
        {

            [SelectItems(nameof(Projects))]
            [DisplayName("")]
            public Guid ProjectsId { get; set; }
        }

        public class ClientListViewModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
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
