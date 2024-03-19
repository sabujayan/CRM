using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Customers;
using Indo.CustomerInvoices;
using Indo.CustomerInvoiceDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Application.Dtos;

namespace Indo.Web.Pages.CustomerInvoice
{
    public class PrintInvoiceModel : IndoPageModel
    {
        [BindProperty]
        public CustomerInvoiceViewModel CustomerInvoice { get; set; }
        public CompanyViewModel Company { get; set; }        
        public CustomerViewModel Customer { get; set; }
        public PagedResultDto<CustomerInvoiceDetailViewModel> Details { get; set; }
        public float Total { get; set; }
        public float SubTotal { get; set; }
        public float DiscAmt { get; set; }
        public float BeforeTax { get; set; }
        public float TaxAmount { get; set; }

        private readonly ICustomerInvoiceAppService _customerInvoiceAppService;

        private readonly ICustomerInvoiceDetailAppService _customerInvoiceDetailAppService;

        private readonly ICustomerAppService _customerAppService;

        private readonly ICompanyAppService _companyAppService;
        public PrintInvoiceModel(
            ICustomerInvoiceAppService customerInvoiceAppService,
            ICustomerInvoiceDetailAppService customerInvoiceDetailAppService,
            CompanyAppService companyAppService,
            ICustomerAppService customerAppService
            )
        {
            _customerInvoiceAppService = customerInvoiceAppService;
            _companyAppService = companyAppService;
            _customerAppService = customerAppService;
            _customerInvoiceDetailAppService = customerInvoiceDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            CustomerInvoice = ObjectMapper.Map<CustomerInvoiceReadDto, CustomerInvoiceViewModel>(await _customerInvoiceAppService.GetAsync(id));

            Company = ObjectMapper.Map<CompanyReadDto, CompanyViewModel>(await _companyAppService.GetDefaultCompanyAsync());

            Customer = ObjectMapper.Map<CustomerReadDto, CustomerViewModel>(await _customerAppService.GetAsync(CustomerInvoice.CustomerId));

            Total = await _customerInvoiceAppService.GetSummaryTotalAsync(CustomerInvoice.Id);

            SubTotal = await _customerInvoiceAppService.GetSummarySubTotalAsync(CustomerInvoice.Id);

            DiscAmt = await _customerInvoiceAppService.GetSummaryDiscAmtAsync(CustomerInvoice.Id);

            BeforeTax = await _customerInvoiceAppService.GetSummaryBeforeTaxAsync(CustomerInvoice.Id);

            TaxAmount = await _customerInvoiceAppService.GetSummaryTaxAmountAsync(CustomerInvoice.Id);

            var dtos = await _customerInvoiceDetailAppService.GetListByCustomerInvoiceAsync(CustomerInvoice.Id);

            Details = ObjectMapper.Map<PagedResultDto<CustomerInvoiceDetailReadDto>, PagedResultDto<CustomerInvoiceDetailViewModel>>(dtos);

        }

        public class CustomerInvoiceViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            public Guid CustomerId { get; set; }
            public Guid SalesExecutiveId { get; set; }
            public string Number { get; set; }
            public string Description { get; set; }
            public string TermAndCondition { get; set; }
            public string PaymentNote { get; set; }
            public DateTime InvoiceDate { get; set; }
            public DateTime InvoiceDueDate { get; set; }
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

        public class CustomerInvoiceDetailViewModel
        {
            public Guid CustomerInvoiceId { get; set; }
            public Guid ServiceId { get; set; }
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
