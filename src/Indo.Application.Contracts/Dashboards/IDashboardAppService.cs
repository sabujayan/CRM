using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.Dashboards
{
    public interface IDashboardAppService : IApplicationService
    {

        Task<List<MonthlyQtyDto>> GetListPurchaseAndSalesMonthlyQty(int monthsCount);

    }
}
