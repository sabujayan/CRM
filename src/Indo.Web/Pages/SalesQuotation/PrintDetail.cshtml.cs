using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Customers;
using Indo.SalesQuotations;
using Indo.SalesQuotationDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Application.Dtos;

namespace Indo.Web.Pages.SalesQuotation
{
    public class PrintDetailModel : IndoPageModel
    {
        [BindProperty]
        public SalesQuotationViewModel SalesQuotation { get; set; }
        public CompanyViewModel Company { get; set; }        
        public CustomerViewModel Customer { get; set; }
        public PagedResultDto<SalesQuotationDetailViewModel> Details { get; set; }
        public float Total { get; set; }
        public float SubTotal { get; set; }
        public float DiscAmt { get; set; }
        public float BeforeTax { get; set; }
        public float TaxAmount { get; set; }

        private readonly ISalesQuotationAppService _salesQuotationAppService;

        private readonly ISalesQuotationDetailAppService _salesQuotationDetailAppService;

        private readonly ICustomerAppService _customerAppService;

        private readonly ICompanyAppService _companyAppService;
        public PrintDetailModel(
            ISalesQuotationAppService salesQuotationAppService,
            ISalesQuotationDetailAppService salesQuotationDetailAppService,
            CompanyAppService companyAppService,
            ICustomerAppService customerAppService
            )
        {
            _salesQuotationAppService = salesQuotationAppService;
            _companyAppService = companyAppService;
            _customerAppService = customerAppService;
            _salesQuotationDetailAppService = salesQuotationDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            SalesQuotation = ObjectMapper.Map<SalesQuotationReadDto, SalesQuotationViewModel>(await _salesQuotationAppService.GetAsync(id));

            Company = ObjectMapper.Map<CompanyReadDto, CompanyViewModel>(await _companyAppService.GetDefaultCompanyAsync());

            Customer = ObjectMapper.Map<CustomerReadDto, CustomerViewModel>(await _customerAppService.GetAsync(SalesQuotation.CustomerId));

            Total = await _salesQuotationAppService.GetSummaryTotalAsync(SalesQuotation.Id);

            SubTotal = await _salesQuotationAppService.GetSummarySubTotalAsync(SalesQuotation.Id);

            DiscAmt = await _salesQuotationAppService.GetSummaryDiscAmtAsync(SalesQuotation.Id);

            BeforeTax = await _salesQuotationAppService.GetSummaryBeforeTaxAsync(SalesQuotation.Id);

            TaxAmount = await _salesQuotationAppService.GetSummaryTaxAmountAsync(SalesQuotation.Id);

            var dtos = await _salesQuotationDetailAppService.GetListBySalesQuotationAsync(SalesQuotation.Id);

            Details = ObjectMapper.Map<PagedResultDto<SalesQuotationDetailReadDto>, PagedResultDto<SalesQuotationDetailViewModel>>(dtos);

        }

        public class SalesQuotationViewModel
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

        public class SalesQuotationDetailViewModel
        {
            public Guid SalesQuotationId { get; set; }
            public Guid ProductId { get; set; }
            public string ProductName { get; set; }
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
