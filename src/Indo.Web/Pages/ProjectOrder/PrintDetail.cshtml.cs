using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Customers;
using Indo.ProjectOrders;
using Indo.ProjectOrderDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Application.Dtos;

namespace Indo.Web.Pages.ProjectOrder
{
    public class PrintDetailModel : IndoPageModel
    {
        [BindProperty]
        public ProjectOrderViewModel ProjectOrder { get; set; }
        public CompanyViewModel Company { get; set; }        
        public CustomerViewModel Customer { get; set; }
        public PagedResultDto<ProjectOrderDetailViewModel> Details { get; set; }
        public float Total { get; set; }

        private readonly IProjectOrderAppService _projectOrderAppService;

        private readonly IProjectOrderDetailAppService _projectOrderDetailAppService;

        private readonly ICustomerAppService _customerAppService;

        private readonly ICompanyAppService _companyAppService;
        public PrintDetailModel(
            IProjectOrderAppService projectOrderAppService,
            IProjectOrderDetailAppService projectOrderDetailAppService,
            CompanyAppService companyAppService,
            ICustomerAppService customerAppService
            )
        {
            _projectOrderAppService = projectOrderAppService;
            _companyAppService = companyAppService;
            _customerAppService = customerAppService;
            _projectOrderDetailAppService = projectOrderDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            ProjectOrder = ObjectMapper.Map<ProjectOrderReadDto, ProjectOrderViewModel>(await _projectOrderAppService.GetAsync(id));

            Company = ObjectMapper.Map<CompanyReadDto, CompanyViewModel>(await _companyAppService.GetDefaultCompanyAsync());

            Customer = ObjectMapper.Map<CustomerReadDto, CustomerViewModel>(await _customerAppService.GetAsync(ProjectOrder.CustomerId));

            Total = await _projectOrderAppService.GetSummaryTotalAsync(ProjectOrder.Id);

            var dtos = await _projectOrderDetailAppService.GetListByProjectOrderAsync(ProjectOrder.Id);

            Details = ObjectMapper.Map<PagedResultDto<ProjectOrderDetailReadDto>, PagedResultDto<ProjectOrderDetailViewModel>>(dtos);

        }

        public class ProjectOrderViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            public Guid CustomerId { get; set; }
            public Guid SalesExecutiveId { get; set; }
            public string Number { get; set; }
            public string Description { get; set; }
            public DateTime OrderDate { get; set; }
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

        public class ProjectOrderDetailViewModel
        {
            public Guid ProjectOrderId { get; set; }
            public string ProjectTask { get; set; }
            public string UomName { get; set; }
            public float Price { get; set; }
            public string CurrencyName { get; set; }
            public float Quantity { get; set; }
            public float Total { get; set; }

        }
    }
}
