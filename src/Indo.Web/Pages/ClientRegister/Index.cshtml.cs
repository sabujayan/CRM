using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Indo.Clientes;
using Indo.Todos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.ObjectMapping;
using static Indo.Web.Pages.Clients.AddModel;

namespace Indo.Web.Pages.ClientRegister
{
    public class IndexModel : IndoPageModel
    {
        [BindProperty]
        public CreateClientRegisterViewModel Client { get; set; }

        private readonly IClientsAppService _clientsappService;
        public List<SelectListItem> Projects { get; set; }
        public IndexModel(IClientsAppService clientsAppService)
        {
            _clientsappService = clientsAppService;
        }


        public void OnGet()
        {
            Client = new CreateClientRegisterViewModel();


        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {

                ClientRegisterDto dto = ObjectMapper.Map<CreateClientRegisterViewModel, ClientRegisterDto>(Client);
                dto.Password = Client.Password;
                await _clientsappService.ClientRegister(dto);
                return Redirect("/Account/Login");
            }
            catch (ClientsAlreadyExistsException ex)
            {
                ex.ToString();
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateClientRegisterViewModel
        {
            [Required]
            [StringLength(ClientConsts.MaxNameLength)]
            public string Name { get; set; }
            public string Address { get; set; }
            public string Country { get; set; }
            public string State { get; set; }
            public string City { get; set; }            
            public string Zip { get; set; }
            [Required]
            [RegularExpression(".+\\@.+\\..+",ErrorMessage ="Please Enter a Valid Email")]
            public string Email { get; set; }
            [Required]
            [RegularExpression(@"^(0|91)?[6-9][0-9]{9}$", ErrorMessage = "Invalid Mobile Number.")]
           
            public string PhoneNumber { get; set; }
            public string Password { get; set; }
        }
    }
}