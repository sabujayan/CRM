using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.SalesOrders;
using Indo.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.SalesOrder
{
    public class DetailModel : IndoPageModel
    {
        [BindProperty]
        public SalesOrderViewModel SalesOrder { get; set; }
        public CustomerViewModel Customer { get; set; }

        private readonly ICustomerAppService _customerAppService;

        private readonly ISalesOrderAppService _salesOrderAppService;
        public DetailModel(
            ISalesOrderAppService salesOrderAppService,
            ICustomerAppService customerAppService
            )
        {
            _salesOrderAppService = salesOrderAppService;
            _customerAppService = customerAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            SalesOrder = ObjectMapper.Map<SalesOrderReadDto, SalesOrderViewModel>(await _salesOrderAppService.GetAsync(id));

            Customer = ObjectMapper.Map<CustomerReadDto, CustomerViewModel>(await _customerAppService.GetAsync(SalesOrder.CustomerId));
        }

        public class SalesOrderViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            public Guid CustomerId { get; set; }
            public string CustomerName { get; set; }
            public Guid SalesExecutiveId { get; set; }
            public string Number { get; set; }
            public string Description { get; set; }
            public DateTime OrderDate { get; set; }
            public string CurrencyName { get; set; }
            public SalesOrderStatus Status { get; set; }
            public string SourceDocument { get; set; }
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
