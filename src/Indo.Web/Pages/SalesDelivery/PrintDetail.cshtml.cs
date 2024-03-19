using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Customers;
using Indo.SalesDeliveries;
using Indo.SalesDeliveryDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Application.Dtos;

namespace Indo.Web.Pages.SalesDelivery
{
    public class PrintDetailModel : IndoPageModel
    {
        [BindProperty]
        public SalesDeliveryViewModel SalesDelivery { get; set; }
        public CompanyViewModel Company { get; set; }        
        public CustomerViewModel Customer { get; set; }
        public PagedResultDto<SalesDeliveryDetailViewModel> Details { get; set; }

        private readonly ISalesDeliveryAppService _salesDeliveryAppService;

        private readonly ISalesDeliveryDetailAppService _salesDeliveryDetailAppService;

        private readonly ICustomerAppService _customerAppService;

        private readonly ICompanyAppService _companyAppService;
        public PrintDetailModel(
            ISalesDeliveryAppService salesDeliveryAppService,
            ISalesDeliveryDetailAppService salesDeliveryDetailAppService,
            CompanyAppService companyAppService,
            ICustomerAppService customerAppService
            )
        {
            _salesDeliveryAppService = salesDeliveryAppService;
            _companyAppService = companyAppService;
            _customerAppService = customerAppService;
            _salesDeliveryDetailAppService = salesDeliveryDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            SalesDelivery = ObjectMapper.Map<SalesDeliveryReadDto, SalesDeliveryViewModel>(await _salesDeliveryAppService.GetAsync(id));

            Company = ObjectMapper.Map<CompanyReadDto, CompanyViewModel>(await _companyAppService.GetDefaultCompanyAsync());

            Customer = ObjectMapper.Map<CustomerReadDto, CustomerViewModel>(await _customerAppService.GetAsync(SalesDelivery.CustomerId));

            var dtos = await _salesDeliveryDetailAppService.GetListBySalesDeliveryAsync(SalesDelivery.Id);

            Details = ObjectMapper.Map<PagedResultDto<SalesDeliveryDetailReadDto>, PagedResultDto<SalesDeliveryDetailViewModel>>(dtos);

        }

        public class SalesDeliveryViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            public Guid CustomerId { get; set; }
            public Guid SalesOrderId { get; set; }
            public string Number { get; set; }
            public string SalesOrderNumber { get; set; }
            public string Description { get; set; }
            public DateTime DeliveryDate { get; set; }
        }

        public class CompanyViewModel
        {
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }
            public Guid CurrencyId { get; set; }
            public string CurrencyName { get; set; }
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

        public class SalesDeliveryDetailViewModel
        {
            public Guid SalesDeliveryId { get; set; }
            public Guid ProductId { get; set; }
            public string ProductName { get; set; }
            public string UomName { get; set; }
            public float Quantity { get; set; }

        }
    }
}
