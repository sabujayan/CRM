using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Customers;
using Indo.ProjectOrders;
using Indo.SalesOrders;
using Indo.ServiceOrders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Indo.Web.Pages.DashboardMain
{
    public class IndexModel : IndoPageModel
    {
        public DashboardViewModel Dashboard { get; set; }

        private readonly IProjectOrderAppService _projectOrderAppService;
        private readonly IServiceOrderAppService _serviceOrderAppService;
        private readonly ISalesOrderAppService _salesOrderAppService;
        private readonly ICustomerAppService _customerAppService;
        public CompanyAppService _companyAppService;
        public IndexModel(
            IProjectOrderAppService projectOrderAppService,
            CompanyAppService companyAppService,
            IServiceOrderAppService serviceOrderAppService,
            ISalesOrderAppService salesOrderAppService,
            ICustomerAppService customerAppService
            )
        {
            _projectOrderAppService = projectOrderAppService;
            _companyAppService = companyAppService;
            _serviceOrderAppService = serviceOrderAppService;
            _customerAppService = customerAppService;
            _salesOrderAppService = salesOrderAppService;
        }
        public IEnumerable<DateTime> EachMonth(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddMonths(1))
                yield return day;
        }
        public async Task OnGet()
        {
            var countSalesOrder = await _salesOrderAppService.GetCountOrderAsync();
            var countServiceOrder = await _serviceOrderAppService.GetCountOrderAsync();
            var countProjectOrder = await _projectOrderAppService.GetCountOrderAsync();
            var defaultCompany = await _companyAppService.GetDefaultCompanyAsync();

            Dashboard = new DashboardViewModel();

            Dashboard.LastFiveProjectOrders = new List<ProjectOrderReadDto>();
            Dashboard.LastFiveProjectOrders = await _projectOrderAppService.GetListLastFiveOrderAsync();
            Dashboard.MonthlyEarnings = await _projectOrderAppService.GetListConfirmMonthlyEarning(5);
            Dashboard.ProjectOrderTotal = countProjectOrder.CountDraft + countProjectOrder.CountConfirm + countProjectOrder.CountCancelled;
            Dashboard.ProjectOrderConfirm = countProjectOrder.CountConfirm;
            Dashboard.ProjectOrderConfirmProgress = 100 * (Dashboard.ProjectOrderConfirm / Dashboard.ProjectOrderTotal);
            Dashboard.ProjectOrderConfirmProgressString = $"{Dashboard.ProjectOrderConfirmProgress.ToString("0,0.0")}%";
            Dashboard.SalesOrderTotal = countSalesOrder.CountDraft + countSalesOrder.CountConfirm + countSalesOrder.CountCancelled;
            Dashboard.SalesOrderConfirm = countSalesOrder.CountConfirm;
            Dashboard.SalesOrderConfirmProgress = 100 * (Dashboard.SalesOrderConfirm / Dashboard.SalesOrderTotal);
            Dashboard.SalesOrderConfirmProgressString = $"{Dashboard.SalesOrderConfirmProgress.ToString("0,0.0")}%";
            Dashboard.ServiceOrderTotal = countServiceOrder.CountDraft + countServiceOrder.CountConfirm + countServiceOrder.CountCancelled;
            Dashboard.ServiceOrderConfirm = countServiceOrder.CountConfirm;
            Dashboard.ServiceOrderConfirmProgress = 100 * (Dashboard.ServiceOrderConfirm / Dashboard.ServiceOrderTotal);
            Dashboard.ServiceOrderConfirmProgressString = $"{Dashboard.ServiceOrderConfirmProgress.ToString("0,0.0")}%";
            Dashboard.OrderTotal = Dashboard.ProjectOrderTotal + Dashboard.SalesOrderTotal + Dashboard.ServiceOrderTotal;
            Dashboard.CurrencyName = defaultCompany.CurrencyName;
            Dashboard.CountStage1 = await _customerAppService.GetLeadCountByStage(CustomerStage.SalesEngage);
            Dashboard.CountStage2 = await _customerAppService.GetLeadCountByStage(CustomerStage.SalesAccepted);
            Dashboard.CountStage3 = await _customerAppService.GetLeadCountByStage(CustomerStage.SalesQualified);

        }

        public class DashboardViewModel
        {
            public float OrderTotal { get; set; }
            public float ProjectOrderTotal { get; set; }
            public float ProjectOrderConfirm { get; set; }
            public float ProjectOrderConfirmProgress { get; set; }
            public string ProjectOrderConfirmProgressString { get; set; }
            public float SalesOrderTotal { get; set; }
            public float SalesOrderConfirm { get; set; }
            public float SalesOrderConfirmProgress { get; set; }
            public string SalesOrderConfirmProgressString { get; set; }
            public float ServiceOrderTotal { get; set; }
            public float ServiceOrderConfirm { get; set; }
            public float ServiceOrderConfirmProgress { get; set; }
            public string ServiceOrderConfirmProgressString { get; set; }
            public List<ProjectOrderReadDto> LastFiveProjectOrders { get; set; }
            public List<MonthlyEarningDto> MonthlyEarnings { get; set; }
            public string CurrencyName { get; set; }
            public int CountStage1 { get; set; }
            public int CountStage2 { get; set; }
            public int CountStage3 { get; set; }
        }

    }
}
