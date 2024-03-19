using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Customers;
using Indo.ProjectOrderDetails;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Indo.SalesOrders;
using Indo.PurchaseOrders;

namespace Indo.Dashboards
{
    public class DashboardAppService : IndoAppService, IDashboardAppService
    {
        private readonly SalesOrderAppService _salesOrderAppService;
        private readonly PurchaseOrderAppService _purchaseOrderAppService;
        public DashboardAppService(
            SalesOrderAppService salesOrderAppService,
            PurchaseOrderAppService purchaseOrderAppService
            )
        {
            _salesOrderAppService = salesOrderAppService;
            _purchaseOrderAppService = purchaseOrderAppService;
        }
        public IEnumerable<DateTime> EachMonth(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddMonths(1))
                yield return day;
        }
        public async Task<List<MonthlyQtyDto>> GetListPurchaseAndSalesMonthlyQty(int monthsCount)
        {
            var result = new List<MonthlyQtyDto>();
            var year = DateTime.Now.Year;
            var endDate = new DateTime(year, 12, 30);//DateTime.Now;
            var startDate = new DateTime(year, 1, 30);//endDate.AddMonths(-monthsCount);
            foreach (var item in EachMonth(startDate, endDate))
            {
                result.Add(new MonthlyQtyDto
                {
                    MonthName = item.ToString("MMM"),
                    QtyPurchase = await _purchaseOrderAppService.GetTotalQtyByYearMonthAsync(item.Year, item.Month),
                    QtySales = await _salesOrderAppService.GetTotalQtyByYearMonthAsync(item.Year, item.Month)
                });
            }
            return result;
        }
    }
}
