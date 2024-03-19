using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.SalesDeliveries;
using Indo.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.SalesDelivery
{
    public class DetailModel : IndoPageModel
    {
        [BindProperty]
        public SalesDeliveryViewModel SalesDelivery { get; set; }
        public CustomerViewModel Customer { get; set; }

        private readonly ICustomerAppService _customerAppService;

        private readonly ISalesDeliveryAppService _salesDeliveryAppService;
        public DetailModel(
            ISalesDeliveryAppService salesDeliveryAppService,
            ICustomerAppService customerAppService
            )
        {
            _salesDeliveryAppService = salesDeliveryAppService;
            _customerAppService = customerAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            SalesDelivery = ObjectMapper.Map<SalesDeliveryReadDto, SalesDeliveryViewModel>(await _salesDeliveryAppService.GetAsync(id));

            Customer = ObjectMapper.Map<CustomerReadDto, CustomerViewModel>(await _customerAppService.GetAsync(SalesDelivery.CustomerId));
        }

        public class SalesDeliveryViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            public Guid SalesOrderId { get; set; }
            public Guid CustomerId { get; set; }
            public string Number { get; set; }
            public string SalesOrderNumber { get; set; }
            public string Description { get; set; }
            public DateTime DeliveryDate { get; set; }
            public SalesDeliveryStatus Status { get; set; }
        }

        public class CustomerViewModel
        {
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }
        }
    }
}
