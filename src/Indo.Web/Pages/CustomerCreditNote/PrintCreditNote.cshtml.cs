using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Customers;
using Indo.CustomerCreditNotes;
using Indo.CustomerCreditNoteDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Application.Dtos;

namespace Indo.Web.Pages.CustomerCreditNote
{
    public class PrintCreditNoteModel : IndoPageModel
    {
        [BindProperty]
        public CustomerCreditNoteViewModel CustomerCreditNote { get; set; }
        public CompanyViewModel Company { get; set; }        
        public CustomerViewModel Customer { get; set; }
        public PagedResultDto<CustomerCreditNoteDetailViewModel> Details { get; set; }
        public float Total { get; set; }
        public float SubTotal { get; set; }
        public float DiscAmt { get; set; }
        public float BeforeTax { get; set; }
        public float TaxAmount { get; set; }

        private readonly ICustomerCreditNoteAppService _customerCreditNoteAppService;

        private readonly ICustomerCreditNoteDetailAppService _customerCreditNoteDetailAppService;

        private readonly ICustomerAppService _customerAppService;

        private readonly ICompanyAppService _companyAppService;
        public PrintCreditNoteModel(
            ICustomerCreditNoteAppService customerCreditNoteAppService,
            ICustomerCreditNoteDetailAppService customerCreditNoteDetailAppService,
            CompanyAppService companyAppService,
            ICustomerAppService customerAppService
            )
        {
            _customerCreditNoteAppService = customerCreditNoteAppService;
            _companyAppService = companyAppService;
            _customerAppService = customerAppService;
            _customerCreditNoteDetailAppService = customerCreditNoteDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            CustomerCreditNote = ObjectMapper.Map<CustomerCreditNoteReadDto, CustomerCreditNoteViewModel>(await _customerCreditNoteAppService.GetAsync(id));

            Company = ObjectMapper.Map<CompanyReadDto, CompanyViewModel>(await _companyAppService.GetDefaultCompanyAsync());

            Customer = ObjectMapper.Map<CustomerReadDto, CustomerViewModel>(await _customerAppService.GetAsync(CustomerCreditNote.CustomerId));

            Total = await _customerCreditNoteAppService.GetSummaryTotalAsync(CustomerCreditNote.Id);

            SubTotal = await _customerCreditNoteAppService.GetSummarySubTotalAsync(CustomerCreditNote.Id);

            DiscAmt = await _customerCreditNoteAppService.GetSummaryDiscAmtAsync(CustomerCreditNote.Id);

            BeforeTax = await _customerCreditNoteAppService.GetSummaryBeforeTaxAsync(CustomerCreditNote.Id);

            TaxAmount = await _customerCreditNoteAppService.GetSummaryTaxAmountAsync(CustomerCreditNote.Id);

            var dtos = await _customerCreditNoteDetailAppService.GetListByCustomerCreditNoteAsync(CustomerCreditNote.Id);

            Details = ObjectMapper.Map<PagedResultDto<CustomerCreditNoteDetailReadDto>, PagedResultDto<CustomerCreditNoteDetailViewModel>>(dtos);

        }

        public class CustomerCreditNoteViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            public Guid CustomerId { get; set; }
            public Guid SalesExecutiveId { get; set; }
            public string Number { get; set; }
            public string Description { get; set; }
            public string PaymentNote { get; set; }
            public DateTime CreditNoteDate { get; set; }
            public string CurrencyName { get; set; }
            public string CustomerInvoiceNumber { get; set; }
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

        public class CustomerCreditNoteDetailViewModel
        {
            public Guid CustomerCreditNoteId { get; set; }
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
