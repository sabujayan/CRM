using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.SalesOrderDetails;
using Indo.SalesOrders;
using Indo.Stocks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Indo.Web.Pages.DashboardInventory
{
    public class IndexModel : IndoPageModel
    {
        public CompanyAppService _companyAppService;
        public SalesOrderAppService _salesOrderAppService;
        public StockAppService _stockAppService;
        public SalesOrderDetailAppService _salesOrderDetailAppService;

        public DashboardViewModel Dashboard { get; set; }
        public CompanyViewModel Company { get; set; }
        public float TotalCOGS { get; set; }
        public float TotalSales { get; set; }
        public float TotalMargin { get; set; }
        public IndexModel(
                CompanyAppService companyAppService,
                SalesOrderAppService salesOrderAppService,
                StockAppService stockAppService,
                SalesOrderDetailAppService salesOrderDetailAppService
            )
        {
            _companyAppService = companyAppService;
            _salesOrderAppService = salesOrderAppService;
            _stockAppService = stockAppService;
            _salesOrderDetailAppService = salesOrderDetailAppService;
        }

        public async Task OnGet()
        {
            Company = ObjectMapper.Map<CompanyReadDto, CompanyViewModel>(await _companyAppService.GetDefaultCompanyAsync());
            TotalCOGS = await _salesOrderAppService.GetTotalCOGSAsync();
            TotalSales = await _salesOrderAppService.GetTotalSalesAsync();
            TotalMargin = TotalSales - TotalCOGS;

            Dashboard = new DashboardViewModel();
            Dashboard.MonthlyValuations = await _stockAppService.GetListMonthlyValuation(5);
            Dashboard.TotalValuation = await _stockAppService.GetTotalValuationAsync();
            Dashboard.HighPerformer = await _salesOrderDetailAppService.GetListHighPerformerAsync();
            Dashboard.LowPerformer = await _salesOrderDetailAppService.GetListLowPerformerAsync();
        }

        public class DashboardViewModel
        {
            public List<MonthlyValuationDto> MonthlyValuations { get; set; }
            public float TotalValuation { get; set; }
            public List<SalesOrderDetailReadDto> HighPerformer { get; set; }
            public List<SalesOrderDetailReadDto> LowPerformer { get; set; }
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
            public Guid DefaultWarehouseId { get; set; }
            public string DefaultWarehouseName { get; set; }
        }
    }
}
