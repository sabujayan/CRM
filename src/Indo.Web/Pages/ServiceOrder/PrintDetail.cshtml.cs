using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Customers;
using Indo.ServiceOrders;
using Indo.ServiceOrderDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Application.Dtos;

namespace Indo.Web.Pages.ServiceOrder
{
    public class PrintDetailModel : IndoPageModel
    {
        [BindProperty]
        public ServiceOrderViewModel ServiceOrder { get; set; }
        public CompanyViewModel Company { get; set; }        
        public CustomerViewModel Customer { get; set; }
        public PagedResultDto<ServiceOrderDetailViewModel> Details { get; set; }
        public float Total { get; set; }
        public float SubTotal { get; set; }
        public float DiscAmt { get; set; }
        public float BeforeTax { get; set; }
        public float TaxAmount { get; set; }

        private readonly IServiceOrderAppService _serviceOrderAppService;

        private readonly IServiceOrderDetailAppService _serviceOrderDetailAppService;

        private readonly ICustomerAppService _customerAppService;

        private readonly ICompanyAppService _companyAppService;
        public PrintDetailModel(
            IServiceOrderAppService serviceOrderAppService,
            IServiceOrderDetailAppService serviceOrderDetailAppService,
            CompanyAppService companyAppService,
            ICustomerAppService customerAppService
            )
        {
            _serviceOrderAppService = serviceOrderAppService;
            _companyAppService = companyAppService;
            _customerAppService = customerAppService;
            _serviceOrderDetailAppService = serviceOrderDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            ServiceOrder = ObjectMapper.Map<ServiceOrderReadDto, ServiceOrderViewModel>(await _serviceOrderAppService.GetAsync(id));

            Company = ObjectMapper.Map<CompanyReadDto, CompanyViewModel>(await _companyAppService.GetDefaultCompanyAsync());

            Customer = ObjectMapper.Map<CustomerReadDto, CustomerViewModel>(await _customerAppService.GetAsync(ServiceOrder.CustomerId));

            Total = await _serviceOrderAppService.GetSummaryTotalAsync(ServiceOrder.Id);

            SubTotal = await _serviceOrderAppService.GetSummarySubTotalAsync(ServiceOrder.Id);

            DiscAmt = await _serviceOrderAppService.GetSummaryDiscAmtAsync(ServiceOrder.Id);

            BeforeTax = await _serviceOrderAppService.GetSummaryBeforeTaxAsync(ServiceOrder.Id);

            TaxAmount = await _serviceOrderAppService.GetSummaryTaxAmountAsync(ServiceOrder.Id);

            var dtos = await _serviceOrderDetailAppService.GetListByServiceOrderAsync(ServiceOrder.Id);

            Details = ObjectMapper.Map<PagedResultDto<ServiceOrderDetailReadDto>, PagedResultDto<ServiceOrderDetailViewModel>>(dtos);

        }

        public class ServiceOrderViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            public Guid CustomerId { get; set; }
            public Guid SalesExecutiveId { get; set; }
            public string Number { get; set; }
            public string Description { get; set; }
            public DateTime OrderDate { get; set; }
            public string CurrencyName { get; set; }
            public string SourceDocument { get; set; }
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

        public class ServiceOrderDetailViewModel
        {
            public Guid ServiceOrderId { get; set; }
            public Guid ServiceId { get; set; }
            public string ServiceName { get; set; }
            public string UomName { get; set; }
            public float Price { get; set; }
            public string CurrencyName { get; set; }
            public float Quantity { get; set; }
            public float DiscAmt { get; set; }
            public float BeforeTax { get; set; }
            public float TaxAmount { get; set; }
            public float Total { get; set; }

        }
    }
}
