using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Customers;
using Indo.SalesOrders;
using Indo.SalesOrderDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Application.Dtos;

namespace Indo.Web.Pages.SalesOrder
{
    public class PrintDetailModel : IndoPageModel
    {
        [BindProperty]
        public SalesOrderViewModel SalesOrder { get; set; }
        public CompanyViewModel Company { get; set; }        
        public CustomerViewModel Customer { get; set; }
        public PagedResultDto<SalesOrderDetailViewModel> Details { get; set; }
        public float Total { get; set; }
        public float SubTotal { get; set; }
        public float DiscAmt { get; set; }
        public float BeforeTax { get; set; }
        public float TaxAmount { get; set; }

        private readonly ISalesOrderAppService _salesOrderAppService;

        private readonly ISalesOrderDetailAppService _salesOrderDetailAppService;

        private readonly ICustomerAppService _customerAppService;

        private readonly ICompanyAppService _companyAppService;
        public PrintDetailModel(
            ISalesOrderAppService salesOrderAppService,
            ISalesOrderDetailAppService salesOrderDetailAppService,
            CompanyAppService companyAppService,
            ICustomerAppService customerAppService
            )
        {
            _salesOrderAppService = salesOrderAppService;
            _companyAppService = companyAppService;
            _customerAppService = customerAppService;
            _salesOrderDetailAppService = salesOrderDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            SalesOrder = ObjectMapper.Map<SalesOrderReadDto, SalesOrderViewModel>(await _salesOrderAppService.GetAsync(id));

            Company = ObjectMapper.Map<CompanyReadDto, CompanyViewModel>(await _companyAppService.GetDefaultCompanyAsync());

            Customer = ObjectMapper.Map<CustomerReadDto, CustomerViewModel>(await _customerAppService.GetAsync(SalesOrder.CustomerId));

            Total = await _salesOrderAppService.GetSummaryTotalAsync(SalesOrder.Id);

            SubTotal = await _salesOrderAppService.GetSummarySubTotalAsync(SalesOrder.Id);

            DiscAmt = await _salesOrderAppService.GetSummaryDiscAmtAsync(SalesOrder.Id);

            BeforeTax = await _salesOrderAppService.GetSummaryBeforeTaxAsync(SalesOrder.Id);

            TaxAmount = await _salesOrderAppService.GetSummaryTaxAmountAsync(SalesOrder.Id);

            var dtos = await _salesOrderDetailAppService.GetListBySalesOrderAsync(SalesOrder.Id);

            Details = ObjectMapper.Map<PagedResultDto<SalesOrderDetailReadDto>, PagedResultDto<SalesOrderDetailViewModel>>(dtos);

        }

        public class SalesOrderViewModel
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

        public class SalesOrderDetailViewModel
        {
            public Guid SalesOrderId { get; set; }
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
