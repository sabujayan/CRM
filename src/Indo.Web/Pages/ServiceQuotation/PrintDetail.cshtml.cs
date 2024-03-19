using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Customers;
using Indo.ServiceQuotations;
using Indo.ServiceQuotationDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Application.Dtos;

namespace Indo.Web.Pages.ServiceQuotation
{
    public class PrintDetailModel : IndoPageModel
    {
        [BindProperty]
        public ServiceQuotationViewModel ServiceQuotation { get; set; }
        public CompanyViewModel Company { get; set; }        
        public CustomerViewModel Customer { get; set; }
        public PagedResultDto<ServiceQuotationDetailViewModel> Details { get; set; }
        public float Total { get; set; }
        public float SubTotal { get; set; }
        public float DiscAmt { get; set; }
        public float BeforeTax { get; set; }
        public float TaxAmount { get; set; }

        private readonly IServiceQuotationAppService _serviceQuotationAppService;

        private readonly IServiceQuotationDetailAppService _serviceQuotationDetailAppService;

        private readonly ICustomerAppService _customerAppService;

        private readonly ICompanyAppService _companyAppService;
        public PrintDetailModel(
            IServiceQuotationAppService serviceQuotationAppService,
            IServiceQuotationDetailAppService serviceQuotationDetailAppService,
            CompanyAppService companyAppService,
            ICustomerAppService customerAppService
            )
        {
            _serviceQuotationAppService = serviceQuotationAppService;
            _companyAppService = companyAppService;
            _customerAppService = customerAppService;
            _serviceQuotationDetailAppService = serviceQuotationDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            ServiceQuotation = ObjectMapper.Map<ServiceQuotationReadDto, ServiceQuotationViewModel>(await _serviceQuotationAppService.GetAsync(id));

            Company = ObjectMapper.Map<CompanyReadDto, CompanyViewModel>(await _companyAppService.GetDefaultCompanyAsync());

            Customer = ObjectMapper.Map<CustomerReadDto, CustomerViewModel>(await _customerAppService.GetAsync(ServiceQuotation.CustomerId));

            Total = await _serviceQuotationAppService.GetSummaryTotalAsync(ServiceQuotation.Id);

            SubTotal = await _serviceQuotationAppService.GetSummarySubTotalAsync(ServiceQuotation.Id);

            DiscAmt = await _serviceQuotationAppService.GetSummaryDiscAmtAsync(ServiceQuotation.Id);

            BeforeTax = await _serviceQuotationAppService.GetSummaryBeforeTaxAsync(ServiceQuotation.Id);

            TaxAmount = await _serviceQuotationAppService.GetSummaryTaxAmountAsync(ServiceQuotation.Id);

            var dtos = await _serviceQuotationDetailAppService.GetListByServiceQuotationAsync(ServiceQuotation.Id);

            Details = ObjectMapper.Map<PagedResultDto<ServiceQuotationDetailReadDto>, PagedResultDto<ServiceQuotationDetailViewModel>>(dtos);

        }

        public class ServiceQuotationViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            public Guid CustomerId { get; set; }
            public Guid SalesExecutiveId { get; set; }
            public string Number { get; set; }
            public string Description { get; set; }
            public DateTime QuotationDate { get; set; }
            public DateTime QuotationValidUntilDate { get; set; }
            public string CurrencyName { get; set; }
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

        public class ServiceQuotationDetailViewModel
        {
            public Guid ServiceQuotationId { get; set; }
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
