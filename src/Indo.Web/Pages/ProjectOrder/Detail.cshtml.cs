using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.ProjectOrders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.ProjectOrder
{
    public class DetailModel : IndoPageModel
    {
        public DetailProjectOrderViewModel ProjectOrder { get; set; }
        public List<SelectListItem> Customers { get; set; }
        public List<SelectListItem> SalesExecutives { get; set; }

        private readonly IProjectOrderAppService _projectOrderAppService;
        public DetailModel(IProjectOrderAppService projectOrderAppService)
        {
            _projectOrderAppService = projectOrderAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _projectOrderAppService.GetAsync(id);
            ProjectOrder = ObjectMapper.Map<ProjectOrderReadDto, DetailProjectOrderViewModel>(dto);

            var customerLookup = await _projectOrderAppService.GetCustomerLookupAsync();
            Customers = customerLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var salesExectuiveLookup = await _projectOrderAppService.GetSalesExecutiveLookupAsync();
            SalesExecutives = salesExectuiveLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }

        public class DetailProjectOrderViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [SelectItems(nameof(Customers))]
            [DisplayName("Customer")]
            public Guid CustomerId { get; set; }
            public string CustomerName { get; set; }
            public string CustomerStreet { get; set; }
            public string CustomerCity { get; set; }
            public string CustomerState { get; set; }
            public string CustomerZipCode { get; set; }
            public string CustomerPhone { get; set; }
            public string CustomerEmail { get; set; }

            [SelectItems(nameof(SalesExecutives))]
            [DisplayName("Sales Executive")]
            public Guid SalesExecutiveId { get; set; }

            [StringLength(ProjectOrderConsts.MaxNumberLength)]
            public string Number { get; set; }
            public string Description { get; set; }
            public DateTime OrderDate { get; set; }
            public string CurrencyName { get; set; }
            public ProjectOrderStatus Status { get; set; }
            public string StatusString { get; set; }
        }


	}
}
