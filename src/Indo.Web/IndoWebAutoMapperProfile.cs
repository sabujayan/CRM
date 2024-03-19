using AutoMapper;
using Indo.Products;
using Indo.Currencies;
using Indo.Customers;
using Indo.Companies;
using Indo.Uoms;
using Indo.ServiceOrders;
using Indo.ServiceOrderDetails;
using Indo.ProjectOrders;
using Indo.ProjectOrderDetails;
using Indo.Services;
using Volo.Abp.Application.Dtos;
using Indo.Vendors;
using Indo.Warehouses;
using Indo.SalesOrders;
using Indo.SalesOrderDetails;
using Indo.PurchaseOrders;
using Indo.PurchaseOrderDetails;
using Indo.SalesDeliveryDetails;
using Indo.SalesDeliveries;
using Indo.PurchaseReceipts;
using Indo.PurchaseReceiptDetails;
using Indo.TransferOrders;
using Indo.TransferOrderDetails;
using Indo.DeliveryOrders;
using Indo.DeliveryOrderDetails;
using Indo.GoodsReceipts;
using Indo.GoodsReceiptDetails;
using Indo.NumberSequences;
using Indo.StockAdjustments;
using Indo.StockAdjustmentDetails;
using Indo.Departments;
using Indo.Employees;
using Indo.ExpenseTypes;
using Indo.Resources;
using Indo.Todos;
using Indo.Calendars;
using Indo.Bookings;
using Indo.Expenses;
using Indo.ExpenseDetails;
using Indo.Notes;
using Indo.ImportantDates;
using Indo.Contacts;
using Indo.Activities;
using Indo.LeadRatings;
using Indo.LeadSources;
using Indo.Tasks;
using Indo.ServiceQuotations;
using Indo.ServiceQuotationDetails;
using Indo.SalesQuotations;
using Indo.SalesQuotationDetails;
using Indo.CashAndBanks;
using Indo.CustomerInvoices;
using Indo.CustomerInvoiceDetails;
using Indo.CustomerCreditNotes;
using Indo.CustomerCreditNoteDetails;
using Indo.CustomerPayments;
using Indo.VendorBills;
using Indo.VendorBillDetails;
using Indo.VendorPayments;
using Indo.VendorDebitNotes;
using Indo.VendorDebitNoteDetails;
using Indo.Skills;
using Indo.Clientes;
using Indo.Projectes;
using Indo.Technologies;

namespace Indo.Web
{
    public class IndoWebAutoMapperProfile : Profile
    {
        public IndoWebAutoMapperProfile()
        {
            //Define your AutoMapper configuration here for the Web project.

            /*SalesExecutive*/
            CreateMap<Pages.SalesExecutive.CreateModel.CreateSalesExecutiveViewModel, EmployeeCreateDto>();
            CreateMap<Pages.SalesExecutive.UpdateModel.SalesExecutiveUpdateViewModel, EmployeeUpdateDto>();
            CreateMap<EmployeeReadDto, Pages.SalesExecutive.UpdateModel.SalesExecutiveUpdateViewModel>();

            /*Product*/
            CreateMap<Pages.Product.CreateModel.CreateProductViewModel, ProductCreateDto>();
            CreateMap<Pages.Product.UpdateModel.ProductUpdateViewModel, ProductUpdateDto>();
            CreateMap<ProductReadDto, Pages.Product.UpdateModel.ProductUpdateViewModel>();

            /*Currency*/
            CreateMap<Pages.Currency.CreateModel.CreateCurrencyViewModel, CurrencyCreateDto>();
            CreateMap<Pages.Currency.UpdateModel.CurrencyUpdateViewModel, CurrencyUpdateDto>();
            CreateMap<CurrencyReadDto, Pages.Currency.UpdateModel.CurrencyUpdateViewModel>();

            /*Customer*/
            CreateMap<Pages.Customer.CreateModel.CreateCustomerViewModel, CustomerCreateDto>();
            CreateMap<Pages.Customer.UpdateModel.CustomerUpdateViewModel, CustomerUpdateDto>();
            CreateMap<CustomerReadDto, Pages.Customer.UpdateModel.CustomerUpdateViewModel>();
            CreateMap<CustomerReadDto, Pages.Customer.DetailModel.DetailCustomerViewModel>();
            CreateMap<CustomerReadDto, Pages.ServiceOrder.PrintDetailModel.CustomerViewModel>();
            CreateMap<CustomerReadDto, Pages.ServiceOrder.DetailModel.CustomerViewModel>();
            CreateMap<CustomerReadDto, Pages.ProjectOrder.PrintDetailModel.CustomerViewModel>();
            CreateMap<CustomerReadDto, Pages.SalesOrder.PrintDetailModel.CustomerViewModel>();
            CreateMap<CustomerReadDto, Pages.SalesOrder.DetailModel.CustomerViewModel>();
            CreateMap<CustomerReadDto, Pages.SalesDelivery.PrintDetailModel.CustomerViewModel>();
            CreateMap<CustomerReadDto, Pages.SalesDelivery.DetailModel.CustomerViewModel>();
            CreateMap<CustomerReadDto, Pages.ServiceQuotation.PrintDetailModel.CustomerViewModel>();
            CreateMap<CustomerReadDto, Pages.ServiceQuotation.DetailModel.CustomerViewModel>();
            CreateMap<CustomerReadDto, Pages.SalesQuotation.PrintDetailModel.CustomerViewModel>();
            CreateMap<CustomerReadDto, Pages.SalesQuotation.DetailModel.CustomerViewModel>();
            CreateMap<CustomerReadDto, Pages.CustomerInvoice.PrintInvoiceModel.CustomerViewModel>();
            CreateMap<CustomerReadDto, Pages.CustomerInvoice.DetailModel.CustomerViewModel>();
            CreateMap<CustomerReadDto, Pages.CustomerCreditNote.PrintCreditNoteModel.CustomerViewModel>();
            CreateMap<CustomerReadDto, Pages.CustomerCreditNote.DetailModel.CustomerViewModel>();

            /*Lead*/
            CreateMap<Pages.Lead.CreateModel.CreateCustomerViewModel, CustomerCreateDto>();
            CreateMap<Pages.Lead.UpdateModel.CustomerUpdateViewModel, CustomerUpdateDto>();
            CreateMap<CustomerReadDto, Pages.Lead.UpdateModel.CustomerUpdateViewModel>();
            CreateMap<CustomerReadDto, Pages.Lead.DetailModel.DetailCustomerViewModel>();

            /*Company*/
            CreateMap<Pages.Company.UpdateModel.CompanyUpdateViewModel, CompanyUpdateDto>();
            CreateMap<CompanyReadDto, Pages.Company.UpdateModel.CompanyUpdateViewModel>();
            CreateMap<CompanyReadDto, Pages.ServiceOrder.PrintDetailModel.CompanyViewModel>();
            CreateMap<CompanyReadDto, Pages.ProjectOrder.PrintDetailModel.CompanyViewModel>();
            CreateMap<CompanyReadDto, Pages.SalesOrder.PrintDetailModel.CompanyViewModel>();
            CreateMap<CompanyReadDto, Pages.PurchaseOrder.PrintDetailModel.CompanyViewModel>();
            CreateMap<CompanyReadDto, Pages.SalesDelivery.PrintDetailModel.CompanyViewModel>();
            CreateMap<CompanyReadDto, Pages.PurchaseReceipt.PrintDetailModel.CompanyViewModel>();
            CreateMap<CompanyReadDto, Pages.DashboardInventory.IndexModel.CompanyViewModel>();
            CreateMap<CompanyReadDto, Pages.Expense.PrintDetailModel.CompanyViewModel>();
            CreateMap<CompanyReadDto, Pages.ServiceQuotation.PrintDetailModel.CompanyViewModel>();
            CreateMap<CompanyReadDto, Pages.SalesQuotation.PrintDetailModel.CompanyViewModel>();
            CreateMap<CompanyReadDto, Pages.CustomerInvoice.PrintInvoiceModel.CompanyViewModel>();
            CreateMap<CompanyReadDto, Pages.CustomerCreditNote.PrintCreditNoteModel.CompanyViewModel>();

            /*Uom*/
            CreateMap<Pages.Uom.CreateModel.CreateUomViewModel, UomCreateDto>();
            CreateMap<Pages.Uom.UpdateModel.UomUpdateViewModel, UomUpdateDto>();
            CreateMap<UomReadDto, Pages.Uom.UpdateModel.UomUpdateViewModel>();

            /*PurchaseOrder*/
            CreateMap<Pages.PurchaseOrder.CreateModel.CreatePurchaseOrderViewModel, PurchaseOrderCreateDto>();
            CreateMap<Pages.PurchaseOrder.UpdateModel.PurchaseOrderUpdateViewModel, PurchaseOrderUpdateDto>();
            CreateMap<PurchaseOrderReadDto, Pages.PurchaseOrder.UpdateModel.PurchaseOrderUpdateViewModel>();
            CreateMap<PurchaseOrderReadDto, Pages.PurchaseOrder.DetailModel.PurchaseOrderViewModel>();
            CreateMap<PurchaseOrderReadDto, Pages.PurchaseOrder.PrintDetailModel.PurchaseOrderViewModel>();

            /*PurchaseOrderDetail*/
            CreateMap<Pages.PurchaseOrder.CreateDetailModel.CreatePurchaseOrderDetailViewModel, PurchaseOrderDetailCreateDto>();
            CreateMap<Pages.PurchaseOrder.UpdateDetailModel.PurchaseOrderDetailUpdateViewModel, PurchaseOrderDetailUpdateDto>();
            CreateMap<PurchaseOrderDetailReadDto, Pages.PurchaseOrder.UpdateDetailModel.PurchaseOrderDetailUpdateViewModel>();
            CreateMap<PurchaseOrderDetailReadDto, Pages.PurchaseOrder.PrintDetailModel.PurchaseOrderDetailViewModel>();
            CreateMap<PagedResultDto<PurchaseOrderDetailReadDto>, PagedResultDto<Pages.PurchaseOrder.PrintDetailModel.PurchaseOrderDetailViewModel>>();


            /*PurchaseReceipt*/
            CreateMap<Pages.PurchaseReceipt.UpdateModel.PurchaseReceiptUpdateViewModel, PurchaseReceiptUpdateDto>();
            CreateMap<PurchaseReceiptReadDto, Pages.PurchaseReceipt.UpdateModel.PurchaseReceiptUpdateViewModel>();
            CreateMap<PurchaseReceiptReadDto, Pages.PurchaseReceipt.DetailModel.PurchaseReceiptViewModel>();
            CreateMap<PurchaseReceiptReadDto, Pages.PurchaseReceipt.PrintDetailModel.PurchaseReceiptViewModel>();

            /*PurchaseReceiptDetail*/
            CreateMap<Pages.PurchaseReceipt.CreateDetailModel.CreatePurchaseReceiptDetailViewModel, PurchaseReceiptDetailCreateDto>();
            CreateMap<Pages.PurchaseReceipt.UpdateDetailModel.PurchaseReceiptDetailUpdateViewModel, PurchaseReceiptDetailUpdateDto>();
            CreateMap<PurchaseReceiptDetailReadDto, Pages.PurchaseReceipt.UpdateDetailModel.PurchaseReceiptDetailUpdateViewModel>();
            CreateMap<PurchaseReceiptDetailReadDto, Pages.PurchaseReceipt.PrintDetailModel.PurchaseReceiptDetailViewModel>();
            CreateMap<PagedResultDto<PurchaseReceiptDetailReadDto>, PagedResultDto<Pages.PurchaseReceipt.PrintDetailModel.PurchaseReceiptDetailViewModel>>();


            /*SalesDelivery*/
            CreateMap<Pages.SalesDelivery.UpdateModel.SalesDeliveryUpdateViewModel, SalesDeliveryUpdateDto>();
            CreateMap<SalesDeliveryReadDto, Pages.SalesDelivery.UpdateModel.SalesDeliveryUpdateViewModel>();
            CreateMap<SalesDeliveryReadDto, Pages.SalesDelivery.DetailModel.SalesDeliveryViewModel>();
            CreateMap<SalesDeliveryReadDto, Pages.SalesDelivery.PrintDetailModel.SalesDeliveryViewModel>();

            /*SalesDeliveryDetail*/
            CreateMap<Pages.SalesDelivery.CreateDetailModel.CreateSalesDeliveryDetailViewModel, SalesDeliveryDetailCreateDto>();
            CreateMap<Pages.SalesDelivery.UpdateDetailModel.SalesDeliveryDetailUpdateViewModel, SalesDeliveryDetailUpdateDto>();
            CreateMap<SalesDeliveryDetailReadDto, Pages.SalesDelivery.UpdateDetailModel.SalesDeliveryDetailUpdateViewModel>();
            CreateMap<SalesDeliveryDetailReadDto, Pages.SalesDelivery.PrintDetailModel.SalesDeliveryDetailViewModel>();
            CreateMap<PagedResultDto<SalesDeliveryDetailReadDto>, PagedResultDto<Pages.SalesDelivery.PrintDetailModel.SalesDeliveryDetailViewModel>>();

            /*SalesOrder*/
            CreateMap<Pages.SalesOrder.CreateModel.CreateSalesOrderViewModel, SalesOrderCreateDto>();
            CreateMap<Pages.SalesOrder.UpdateModel.SalesOrderUpdateViewModel, SalesOrderUpdateDto>();
            CreateMap<SalesOrderReadDto, Pages.SalesOrder.UpdateModel.SalesOrderUpdateViewModel>();
            CreateMap<SalesOrderReadDto, Pages.SalesOrder.DetailModel.SalesOrderViewModel>();
            CreateMap<SalesOrderReadDto, Pages.SalesOrder.PrintDetailModel.SalesOrderViewModel>();

            /*SalesOrderDetail*/
            CreateMap<Pages.SalesOrder.CreateDetailModel.CreateSalesOrderDetailViewModel, SalesOrderDetailCreateDto>();
            CreateMap<Pages.SalesOrder.UpdateDetailModel.SalesOrderDetailUpdateViewModel, SalesOrderDetailUpdateDto>();
            CreateMap<SalesOrderDetailReadDto, Pages.SalesOrder.UpdateDetailModel.SalesOrderDetailUpdateViewModel>();
            CreateMap<SalesOrderDetailReadDto, Pages.SalesOrder.PrintDetailModel.SalesOrderDetailViewModel>();
            CreateMap<PagedResultDto<SalesOrderDetailReadDto>, PagedResultDto<Pages.SalesOrder.PrintDetailModel.SalesOrderDetailViewModel>>();

            /*TransferOrder*/
            CreateMap<Pages.TransferOrder.CreateModel.CreateTransferOrderViewModel, TransferOrderCreateDto>();
            CreateMap<Pages.TransferOrder.UpdateModel.TransferOrderUpdateViewModel, TransferOrderUpdateDto>();
            CreateMap<TransferOrderReadDto, Pages.TransferOrder.UpdateModel.TransferOrderUpdateViewModel>();
            CreateMap<TransferOrderReadDto, Pages.TransferOrder.DetailModel.TransferOrderViewModel>();
            CreateMap<TransferOrderReadDto, Pages.TransferOrder.PrintDetailModel.TransferOrderViewModel>();

            /*TransferOrderDetail*/
            CreateMap<Pages.TransferOrder.CreateDetailModel.CreateTransferOrderDetailViewModel, TransferOrderDetailCreateDto>();
            CreateMap<Pages.TransferOrder.UpdateDetailModel.TransferOrderDetailUpdateViewModel, TransferOrderDetailUpdateDto>();
            CreateMap<TransferOrderDetailReadDto, Pages.TransferOrder.UpdateDetailModel.TransferOrderDetailUpdateViewModel>();
            CreateMap<TransferOrderDetailReadDto, Pages.TransferOrder.PrintDetailModel.TransferOrderDetailViewModel>();
            CreateMap<PagedResultDto<TransferOrderDetailReadDto>, PagedResultDto<Pages.TransferOrder.PrintDetailModel.TransferOrderDetailViewModel>>();


            /*DeliveryOrder*/
            CreateMap<Pages.DeliveryOrder.UpdateModel.DeliveryOrderUpdateViewModel, DeliveryOrderUpdateDto>();
            CreateMap<DeliveryOrderReadDto, Pages.DeliveryOrder.UpdateModel.DeliveryOrderUpdateViewModel>();
            CreateMap<DeliveryOrderReadDto, Pages.DeliveryOrder.DetailModel.DeliveryOrderViewModel>();
            CreateMap<DeliveryOrderReadDto, Pages.DeliveryOrder.PrintDetailModel.DeliveryOrderViewModel>();

            /*DeliveryOrderDetail*/
            CreateMap<Pages.DeliveryOrder.CreateDetailModel.CreateDeliveryOrderDetailViewModel, DeliveryOrderDetailCreateDto>();
            CreateMap<Pages.DeliveryOrder.UpdateDetailModel.DeliveryOrderDetailUpdateViewModel, DeliveryOrderDetailUpdateDto>();
            CreateMap<DeliveryOrderDetailReadDto, Pages.DeliveryOrder.UpdateDetailModel.DeliveryOrderDetailUpdateViewModel>();
            CreateMap<DeliveryOrderDetailReadDto, Pages.DeliveryOrder.PrintDetailModel.DeliveryOrderDetailViewModel>();
            CreateMap<PagedResultDto<DeliveryOrderDetailReadDto>, PagedResultDto<Pages.DeliveryOrder.PrintDetailModel.DeliveryOrderDetailViewModel>>();


            /*GoodsReceipt*/
            CreateMap<Pages.GoodsReceipt.CreateModel.CreateGoodsReceiptViewModel, GoodsReceiptCreateDto>();
            CreateMap<Pages.GoodsReceipt.UpdateModel.GoodsReceiptUpdateViewModel, GoodsReceiptUpdateDto>();
            CreateMap<GoodsReceiptReadDto, Pages.GoodsReceipt.UpdateModel.GoodsReceiptUpdateViewModel>();
            CreateMap<GoodsReceiptReadDto, Pages.GoodsReceipt.DetailModel.GoodsReceiptViewModel>();
            CreateMap<GoodsReceiptReadDto, Pages.GoodsReceipt.PrintDetailModel.GoodsReceiptViewModel>();

            /*GoodsReceiptDetail*/
            CreateMap<Pages.GoodsReceipt.CreateDetailModel.CreateGoodsReceiptDetailViewModel, GoodsReceiptDetailCreateDto>();
            CreateMap<Pages.GoodsReceipt.UpdateDetailModel.GoodsReceiptDetailUpdateViewModel, GoodsReceiptDetailUpdateDto>();
            CreateMap<GoodsReceiptDetailReadDto, Pages.GoodsReceipt.UpdateDetailModel.GoodsReceiptDetailUpdateViewModel>();
            CreateMap<GoodsReceiptDetailReadDto, Pages.GoodsReceipt.PrintDetailModel.GoodsReceiptDetailViewModel>();
            CreateMap<PagedResultDto<GoodsReceiptDetailReadDto>, PagedResultDto<Pages.GoodsReceipt.PrintDetailModel.GoodsReceiptDetailViewModel>>();


            /*ServiceOrder*/
            CreateMap<Pages.ServiceOrder.CreateModel.CreateServiceOrderViewModel, ServiceOrderCreateDto>();
            CreateMap<Pages.ServiceOrder.UpdateModel.ServiceOrderUpdateViewModel, ServiceOrderUpdateDto>();
            CreateMap<ServiceOrderReadDto, Pages.ServiceOrder.UpdateModel.ServiceOrderUpdateViewModel>();
            CreateMap<ServiceOrderReadDto, Pages.ServiceOrder.DetailModel.ServiceOrderViewModel>();
            CreateMap<ServiceOrderReadDto, Pages.ServiceOrder.PrintDetailModel.ServiceOrderViewModel>();

            /*ServiceOrderDetail*/
            CreateMap<Pages.ServiceOrder.CreateDetailModel.CreateServiceOrderDetailViewModel, ServiceOrderDetailCreateDto>();
            CreateMap<Pages.ServiceOrder.UpdateDetailModel.ServiceOrderDetailUpdateViewModel, ServiceOrderDetailUpdateDto>();
            CreateMap<ServiceOrderDetailReadDto, Pages.ServiceOrder.UpdateDetailModel.ServiceOrderDetailUpdateViewModel>();
            CreateMap<ServiceOrderDetailReadDto, Pages.ServiceOrder.PrintDetailModel.ServiceOrderDetailViewModel>();
            CreateMap<PagedResultDto<ServiceOrderDetailReadDto>, PagedResultDto<Pages.ServiceOrder.PrintDetailModel.ServiceOrderDetailViewModel>>();


            /*ServiceQuotation*/
            CreateMap<Pages.ServiceQuotation.CreateModel.CreateServiceQuotationViewModel, ServiceQuotationCreateDto>();
            CreateMap<Pages.ServiceQuotation.UpdateModel.ServiceQuotationUpdateViewModel, ServiceQuotationUpdateDto>();
            CreateMap<ServiceQuotationReadDto, Pages.ServiceQuotation.UpdateModel.ServiceQuotationUpdateViewModel>();
            CreateMap<ServiceQuotationReadDto, Pages.ServiceQuotation.DetailModel.ServiceQuotationViewModel>();
            CreateMap<ServiceQuotationReadDto, Pages.ServiceQuotation.PrintDetailModel.ServiceQuotationViewModel>();

            /*ServiceQuotationDetail*/
            CreateMap<Pages.ServiceQuotation.CreateDetailModel.CreateServiceQuotationDetailViewModel, ServiceQuotationDetailCreateDto>();
            CreateMap<Pages.ServiceQuotation.UpdateDetailModel.ServiceQuotationDetailUpdateViewModel, ServiceQuotationDetailUpdateDto>();
            CreateMap<ServiceQuotationDetailReadDto, Pages.ServiceQuotation.UpdateDetailModel.ServiceQuotationDetailUpdateViewModel>();
            CreateMap<ServiceQuotationDetailReadDto, Pages.ServiceQuotation.PrintDetailModel.ServiceQuotationDetailViewModel>();
            CreateMap<PagedResultDto<ServiceQuotationDetailReadDto>, PagedResultDto<Pages.ServiceQuotation.PrintDetailModel.ServiceQuotationDetailViewModel>>();


            /*SalesQuotation*/
            CreateMap<Pages.SalesQuotation.CreateModel.CreateSalesQuotationViewModel, SalesQuotationCreateDto>();
            CreateMap<Pages.SalesQuotation.UpdateModel.SalesQuotationUpdateViewModel, SalesQuotationUpdateDto>();
            CreateMap<SalesQuotationReadDto, Pages.SalesQuotation.UpdateModel.SalesQuotationUpdateViewModel>();
            CreateMap<SalesQuotationReadDto, Pages.SalesQuotation.DetailModel.SalesQuotationViewModel>();
            CreateMap<SalesQuotationReadDto, Pages.SalesQuotation.PrintDetailModel.SalesQuotationViewModel>();

            /*SalesQuotationDetail*/
            CreateMap<Pages.SalesQuotation.CreateDetailModel.CreateSalesQuotationDetailViewModel, SalesQuotationDetailCreateDto>();
            CreateMap<Pages.SalesQuotation.UpdateDetailModel.SalesQuotationDetailUpdateViewModel, SalesQuotationDetailUpdateDto>();
            CreateMap<SalesQuotationDetailReadDto, Pages.SalesQuotation.UpdateDetailModel.SalesQuotationDetailUpdateViewModel>();
            CreateMap<SalesQuotationDetailReadDto, Pages.SalesQuotation.PrintDetailModel.SalesQuotationDetailViewModel>();
            CreateMap<PagedResultDto<SalesQuotationDetailReadDto>, PagedResultDto<Pages.SalesQuotation.PrintDetailModel.SalesQuotationDetailViewModel>>();

            /*ProjectOrder*/
            CreateMap<Pages.ProjectOrder.CreateModel.CreateProjectOrderViewModel, ProjectOrderCreateDto>();
            CreateMap<Pages.ProjectOrder.UpdateModel.ProjectOrderUpdateViewModel, ProjectOrderUpdateDto>();
            CreateMap<ProjectOrderReadDto, Pages.ProjectOrder.UpdateModel.ProjectOrderUpdateViewModel>();
            CreateMap<ProjectOrderReadDto, Pages.ProjectOrder.DetailModel.DetailProjectOrderViewModel>();
            CreateMap<ProjectOrderReadDto, Pages.ProjectOrder.PrintDetailModel.ProjectOrderViewModel>();

            /*ProjectOrderDetail*/
            CreateMap<Pages.ProjectOrder.CreateDetailModel.CreateProjectOrderDetailViewModel, ProjectOrderDetailCreateDto>();
            CreateMap<Pages.ProjectOrder.UpdateDetailModel.ProjectOrderDetailUpdateViewModel, ProjectOrderDetailUpdateDto>();
            CreateMap<ProjectOrderDetailReadDto, Pages.ProjectOrder.UpdateDetailModel.ProjectOrderDetailUpdateViewModel>();
            CreateMap<ProjectOrderDetailReadDto, Pages.ProjectOrder.PrintDetailModel.ProjectOrderDetailViewModel>();
            CreateMap<PagedResultDto<ProjectOrderDetailReadDto>, PagedResultDto<Pages.ProjectOrder.PrintDetailModel.ProjectOrderDetailViewModel>>();

            /*Service*/
            CreateMap<Pages.Service.CreateModel.CreateServiceViewModel, ServiceCreateDto>();
            CreateMap<Pages.Service.UpdateModel.ServiceUpdateViewModel, ServiceUpdateDto>();
            CreateMap<ServiceReadDto, Pages.Service.UpdateModel.ServiceUpdateViewModel>();

            /*Buyer*/
            CreateMap<Pages.Buyer.CreateModel.CreateBuyerViewModel, EmployeeCreateDto>();
            CreateMap<Pages.Buyer.UpdateModel.BuyerUpdateViewModel, EmployeeUpdateDto>();
            CreateMap<EmployeeReadDto, Pages.Buyer.UpdateModel.BuyerUpdateViewModel>();

            /*Vendor*/
            CreateMap<Pages.Vendor.CreateModel.CreateVendorViewModel, VendorCreateDto>();
            CreateMap<Pages.Vendor.UpdateModel.VendorUpdateViewModel, VendorUpdateDto>();
            CreateMap<VendorReadDto, Pages.Vendor.UpdateModel.VendorUpdateViewModel>();
            CreateMap<VendorReadDto, Pages.PurchaseOrder.PrintDetailModel.VendorViewModel>();
            CreateMap<VendorReadDto, Pages.PurchaseOrder.DetailModel.VendorViewModel>();
            CreateMap<VendorReadDto, Pages.PurchaseReceipt.PrintDetailModel.VendorViewModel>();
            CreateMap<VendorReadDto, Pages.PurchaseReceipt.DetailModel.VendorViewModel>();
            CreateMap<VendorReadDto, Pages.VendorBill.DetailModel.VendorViewModel>();
            CreateMap<VendorReadDto, Pages.VendorDebitNote.PrintDebitNoteModel.VendorViewModel>();
            CreateMap<VendorReadDto, Pages.VendorDebitNote.DetailModel.VendorViewModel>();

            /*Warehouse*/
            CreateMap<Pages.Warehouse.CreateModel.CreateWarehouseViewModel, WarehouseCreateDto>();
            CreateMap<Pages.Warehouse.UpdateModel.WarehouseUpdateViewModel, WarehouseUpdateDto>();
            CreateMap<WarehouseReadDto, Pages.Warehouse.UpdateModel.WarehouseUpdateViewModel>();

            /*NumberSequence*/
            CreateMap<Pages.NumberSequence.UpdateModel.NumberSequenceUpdateViewModel, NumberSequenceUpdateDto>();
            CreateMap<NumberSequenceReadDto, Pages.NumberSequence.UpdateModel.NumberSequenceUpdateViewModel>();

            /*StockAdjustment*/
            CreateMap<Pages.StockAdjustment.CreateModel.CreateStockAdjustmentViewModel, StockAdjustmentCreateDto>();
            CreateMap<Pages.StockAdjustment.UpdateModel.StockAdjustmentUpdateViewModel, StockAdjustmentUpdateDto>();
            CreateMap<StockAdjustmentReadDto, Pages.StockAdjustment.UpdateModel.StockAdjustmentUpdateViewModel>();
            CreateMap<StockAdjustmentReadDto, Pages.StockAdjustment.DetailModel.StockAdjustmentViewModel>();
            CreateMap<StockAdjustmentReadDto, Pages.StockAdjustment.PrintDetailModel.StockAdjustmentViewModel>();

            /*TransferOrderDetail*/
            CreateMap<Pages.StockAdjustment.CreateDetailModel.CreateStockAdjustmentDetailViewModel, StockAdjustmentDetailCreateDto>();
            CreateMap<Pages.StockAdjustment.UpdateDetailModel.StockAdjustmentDetailUpdateViewModel, StockAdjustmentDetailUpdateDto>();
            CreateMap<StockAdjustmentDetailReadDto, Pages.StockAdjustment.UpdateDetailModel.StockAdjustmentDetailUpdateViewModel>();
            CreateMap<StockAdjustmentDetailReadDto, Pages.StockAdjustment.PrintDetailModel.StockAdjustmentDetailViewModel>();
            CreateMap<PagedResultDto<StockAdjustmentDetailReadDto>, PagedResultDto<Pages.StockAdjustment.PrintDetailModel.StockAdjustmentDetailViewModel>>();

            /*Department*/
            CreateMap<Pages.Department.CreateModel.CreateDepartmentViewModel, DepartmentCreateDto>();
            CreateMap<Pages.Department.UpdateModel.DepartmentUpdateViewModel, DepartmentUpdateDto>();
            CreateMap<DepartmentReadDto, Pages.Department.UpdateModel.DepartmentUpdateViewModel>();
            CreateMap<Pages.Skill.CreateModel.CreateSkillViewModel, SkillCreateDto>();
            CreateMap<Pages.Skill.UpdateModel.SkillUpdateViewModel, SkillUpdateDto>();
            CreateMap<SkillReadDto, Pages.Skill.UpdateModel.SkillUpdateViewModel>();

            /*Client*/
            CreateMap<Pages.Clients.CreateModel.CreateClientViewModel, ClientsCreateDto>();
            CreateMap<Pages.Clients.AddModel.CreateClientViewModel, ClientsCreateDto>();
            CreateMap<Pages.Clients.UpdateModel.ClientUpdateViewModel, ClientsUpdateDto>();
            CreateMap<Pages.Clients.EditModel.ClientEditViewModel, ClientsUpdateDto>();
            CreateMap<ClientsReadDto, Pages.Clients.UpdateModel.ClientUpdateViewModel>();
            CreateMap<ClientsReadDto, Pages.Clients.EditModel.ClientEditViewModel>();
            CreateMap<ClientsReadDto, Pages.Clients.IndexModel.ClientListViewModel>();
            CreateMap<PagedResultDto<ClientsReadDto>, PagedResultDto<Pages.Clients.IndexModel.ClientListViewModel>>();


            /*Project*/
            CreateMap<Pages.Project.AddModel.CreateProjectViewModel, ProjectsCreateDto>();
            CreateMap<Pages.Project.CreateModel.CreateProjectViewModel, ProjectsCreateDto>();
            CreateMap<Pages.Project.UpdateModel.ProjectUpdateViewModel, ProjectsUpdateDto>();
            CreateMap<Pages.Project.EditModel.ProjectUpdateViewModel, ProjectsUpdateDto>();
            CreateMap<ProjectsReadDto, Pages.Project.UpdateModel.ProjectUpdateViewModel>();
            CreateMap<ProjectsReadDto, Pages.Project.EditModel.ProjectUpdateViewModel>();
            CreateMap<ProjectsReadDto, Pages.Project.IndexModel.ProjectListViewModel>();
            CreateMap<PagedResultDto<ProjectsReadDto>, PagedResultDto<Pages.Project.IndexModel.ProjectListViewModel>>();


            /*Technology*/
            CreateMap<Pages.Technologies.AddModel.AddTechnologyViewModel, TechnologyCreateDto>();
            CreateMap<TechnologyReadDto, Pages.Technologies.IndexModel.TechnologyListViewModel>();
            CreateMap<TechnologyReadDto, Pages.Technologies.EditModel.TechnologyUpdateViewModel>();
            CreateMap<PagedResultDto<TechnologyReadDto>, PagedResultDto<Pages.Technologies.IndexModel.TechnologyListViewModel>>();
            CreateMap<Pages.Technologies.EditModel.TechnologyUpdateViewModel, TechnologyUpdateDto>();

            /*Employee*/
            CreateMap<Pages.Employee.AddModel.CreateEmployeeViewModel, EmployeeCreateDto>();
            CreateMap<Pages.Employee.CreateModel.CreateEmployeeViewModel, EmployeeCreateDto>();
            CreateMap<Pages.Employee.UpdateModel.EmployeeUpdateViewModel, EmployeeUpdateDto>();
            CreateMap<Pages.Employee.EditModel.EditEmployeeViewModel, EmployeeUpdateDto>();
            CreateMap<EmployeeReadDto, Pages.Employee.UpdateModel.EmployeeUpdateViewModel>();
            CreateMap<EmployeeReadDto, Pages.Employee.EditModel.EditEmployeeViewModel>();
            CreateMap<EmployeeReadDto, Pages.Employee.IndexModel.EmployeeListViewModel>();
            CreateMap<PagedResultDto<EmployeeReadDto>, PagedResultDto<Pages.Employee.IndexModel.EmployeeListViewModel>>();

            //CreateMap<EmployeeReadDto, Pages.Employee.EditModel.EditEmployeeViewModel>();

            //CreateMap<Pages.Employee.EditModel.EditEmployeeViewModel, EmployeeCreateDto>();
            //CreateMap<EmployeeCreateDto, Pages.Employee.EditModel.EditEmployeeViewModel>();
            /*ExpenseType*/
            CreateMap<Pages.ExpenseType.CreateModel.CreateExpenseTypeViewModel, ExpenseTypeCreateDto>();
            CreateMap<Pages.ExpenseType.UpdateModel.ExpenseTypeUpdateViewModel, ExpenseTypeUpdateDto>();
            CreateMap<ExpenseTypeReadDto, Pages.ExpenseType.UpdateModel.ExpenseTypeUpdateViewModel>();

            /*Resource*/
            CreateMap<Pages.Resource.CreateModel.CreateResourceViewModel, ResourceCreateDto>();
            CreateMap<Pages.Resource.UpdateModel.ResourceUpdateViewModel, ResourceUpdateDto>();
            CreateMap<ResourceReadDto, Pages.Resource.UpdateModel.ResourceUpdateViewModel>();

            /*Todo*/
            CreateMap<Pages.Todo.CreateModel.CreateTodoViewModel, TodoCreateDto>();
            CreateMap<Pages.Todo.UpdateModel.TodoUpdateViewModel, TodoUpdateDto>();
            CreateMap<TodoReadDto, Pages.Todo.UpdateModel.TodoUpdateViewModel>();

            /*Calendar*/
            CreateMap<Pages.Calendar.CreateModel.CreateCalendarViewModel, CalendarCreateDto>();
            CreateMap<Pages.Calendar.UpdateModel.CalendarUpdateViewModel, CalendarUpdateDto>();
            CreateMap<CalendarReadDto, Pages.Calendar.UpdateModel.CalendarUpdateViewModel>();

            /*Booking*/
            CreateMap<Pages.Booking.CreateModel.CreateBookingViewModel, BookingCreateDto>();
            CreateMap<Pages.Booking.UpdateModel.BookingUpdateViewModel, BookingUpdateDto>();
            CreateMap<BookingReadDto, Pages.Booking.UpdateModel.BookingUpdateViewModel>();

            /*Expense*/
            CreateMap<Pages.Expense.CreateModel.CreateExpenseViewModel, ExpenseCreateDto>();
            CreateMap<Pages.Expense.UpdateModel.ExpenseUpdateViewModel, ExpenseUpdateDto>();
            CreateMap<ExpenseReadDto, Pages.Expense.UpdateModel.ExpenseUpdateViewModel>();
            CreateMap<ExpenseReadDto, Pages.Expense.DetailModel.DetailExpenseViewModel>();
            CreateMap<ExpenseReadDto, Pages.Expense.PrintDetailModel.ExpenseViewModel>();
            CreateMap<Pages.Expense.CreateDetailModel.CreateExpenseDetailViewModel, ExpenseDetailCreateDto>();
            CreateMap<Pages.Expense.UpdateDetailModel.ExpenseDetailUpdateViewModel, ExpenseDetailUpdateDto>();
            CreateMap<ExpenseDetailReadDto, Pages.Expense.UpdateDetailModel.ExpenseDetailUpdateViewModel>();
            CreateMap<ExpenseDetailReadDto, Pages.Expense.PrintDetailModel.ExpenseDetailViewModel>();
            CreateMap<PagedResultDto<ExpenseDetailReadDto>, PagedResultDto<Pages.Expense.PrintDetailModel.ExpenseDetailViewModel>>();

            /*Note*/
            CreateMap<Pages.Note.CreateModel.CreateNoteViewModel, NoteCreateDto>();
            CreateMap<Pages.Note.UpdateModel.NoteUpdateViewModel, NoteUpdateDto>();
            CreateMap<NoteReadDto, Pages.Note.UpdateModel.NoteUpdateViewModel>();

            /*ImportantDate*/
            CreateMap<Pages.ImportantDate.CreateModel.CreateImportantDateViewModel, ImportantDateCreateDto>();
            CreateMap<Pages.ImportantDate.UpdateModel.ImportantDateUpdateViewModel, ImportantDateUpdateDto>();
            CreateMap<ImportantDateReadDto, Pages.ImportantDate.UpdateModel.ImportantDateUpdateViewModel>();

            /*Contact*/
            CreateMap<Pages.Contact.CreateModel.CreateContactViewModel, ContactCreateDto>();
            CreateMap<Pages.Contact.UpdateModel.ContactUpdateViewModel, ContactUpdateDto>();
            CreateMap<ContactReadDto, Pages.Contact.UpdateModel.ContactUpdateViewModel>();

            /*Activity*/
            CreateMap<Pages.Activity.CreateModel.CreateActivityViewModel, ActivityCreateDto>();
            CreateMap<Pages.Activity.UpdateModel.ActivityUpdateViewModel, ActivityUpdateDto>();
            CreateMap<ActivityReadDto, Pages.Activity.UpdateModel.ActivityUpdateViewModel>();

            /*LeadRating*/
            CreateMap<Pages.LeadRating.CreateModel.CreateLeadRatingViewModel, LeadRatingCreateDto>();
            CreateMap<Pages.LeadRating.UpdateModel.LeadRatingUpdateViewModel, LeadRatingUpdateDto>();
            CreateMap<LeadRatingReadDto, Pages.LeadRating.UpdateModel.LeadRatingUpdateViewModel>();

            /*LeadSource*/
            CreateMap<Pages.LeadSource.CreateModel.CreateLeadSourceViewModel, LeadSourceCreateDto>();
            CreateMap<Pages.LeadSource.UpdateModel.LeadSourceUpdateViewModel, LeadSourceUpdateDto>();
            CreateMap<LeadSourceReadDto, Pages.LeadSource.UpdateModel.LeadSourceUpdateViewModel>();

            /*Task*/
            CreateMap<Pages.Tasks.CreateModel.CreateTaskViewModel, TaskCreateDto>();
            CreateMap<Pages.Tasks.UpdateModel.TaskUpdateViewModel, TaskUpdateDto>();
            CreateMap<TaskReadDto, Pages.Tasks.UpdateModel.TaskUpdateViewModel>();

            /*CashAndBank*/
            CreateMap<Pages.CashAndBank.CreateModel.CreateCashAndBankViewModel, CashAndBankCreateDto>();
            CreateMap<Pages.CashAndBank.UpdateModel.CashAndBankUpdateViewModel, CashAndBankUpdateDto>();
            CreateMap<CashAndBankReadDto, Pages.CashAndBank.UpdateModel.CashAndBankUpdateViewModel>();


            /*ClientRegister*/
            CreateMap<Pages.ClientRegister.IndexModel.CreateClientRegisterViewModel, ClientRegisterDto>();

            /*CustomerInvoice*/
            CreateMap<Pages.CustomerInvoice.CreateModel.CreateCustomerInvoiceViewModel, CustomerInvoiceCreateDto>();
            CreateMap<Pages.CustomerInvoice.UpdateModel.CustomerInvoiceUpdateViewModel, CustomerInvoiceUpdateDto>();
            CreateMap<CustomerInvoiceReadDto, Pages.CustomerInvoice.UpdateModel.CustomerInvoiceUpdateViewModel>();
            CreateMap<CustomerInvoiceReadDto, Pages.CustomerInvoice.DetailModel.CustomerInvoiceViewModel>();
            CreateMap<CustomerInvoiceReadDto, Pages.CustomerInvoice.PrintInvoiceModel.CustomerInvoiceViewModel>();

            /*CustomerInvoiceDetail*/
            CreateMap<Pages.CustomerInvoice.CreateDetailModel.CreateCustomerInvoiceDetailViewModel, CustomerInvoiceDetailCreateDto>();
            CreateMap<Pages.CustomerInvoice.UpdateDetailModel.CustomerInvoiceDetailUpdateViewModel, CustomerInvoiceDetailUpdateDto>();
            CreateMap<CustomerInvoiceDetailReadDto, Pages.CustomerInvoice.UpdateDetailModel.CustomerInvoiceDetailUpdateViewModel>();
            CreateMap<CustomerInvoiceDetailReadDto, Pages.CustomerInvoice.PrintInvoiceModel.CustomerInvoiceDetailViewModel>();
            CreateMap<PagedResultDto<CustomerInvoiceDetailReadDto>, PagedResultDto<Pages.CustomerInvoice.PrintInvoiceModel.CustomerInvoiceDetailViewModel>>();


            /*CustomerCreditNote*/
            CreateMap<Pages.CustomerCreditNote.CreateModel.CreateCustomerCreditNoteViewModel, CustomerCreditNoteCreateDto>();
            CreateMap<Pages.CustomerCreditNote.UpdateModel.CustomerCreditNoteUpdateViewModel, CustomerCreditNoteUpdateDto>();
            CreateMap<CustomerCreditNoteReadDto, Pages.CustomerCreditNote.UpdateModel.CustomerCreditNoteUpdateViewModel>();
            CreateMap<CustomerCreditNoteReadDto, Pages.CustomerCreditNote.DetailModel.CustomerCreditNoteViewModel>();
            CreateMap<CustomerCreditNoteReadDto, Pages.CustomerCreditNote.PrintCreditNoteModel.CustomerCreditNoteViewModel>();

            /*CustomerCreditNoteDetail*/
            CreateMap<Pages.CustomerCreditNote.CreateDetailModel.CreateCustomerCreditNoteDetailViewModel, CustomerCreditNoteDetailCreateDto>();
            CreateMap<Pages.CustomerCreditNote.UpdateDetailModel.CustomerCreditNoteDetailUpdateViewModel, CustomerCreditNoteDetailUpdateDto>();
            CreateMap<CustomerCreditNoteDetailReadDto, Pages.CustomerCreditNote.UpdateDetailModel.CustomerCreditNoteDetailUpdateViewModel>();
            CreateMap<CustomerCreditNoteDetailReadDto, Pages.CustomerCreditNote.PrintCreditNoteModel.CustomerCreditNoteDetailViewModel>();
            CreateMap<PagedResultDto<CustomerCreditNoteDetailReadDto>, PagedResultDto<Pages.CustomerCreditNote.PrintCreditNoteModel.CustomerCreditNoteDetailViewModel>>();


            /*CustomerPayment*/
            CreateMap<Pages.CustomerPayment.CreateModel.CreateCustomerPaymentViewModel, CustomerPaymentCreateDto>();
            CreateMap<Pages.CustomerPayment.UpdateModel.CustomerPaymentUpdateViewModel, CustomerPaymentUpdateDto>();
            CreateMap<CustomerPaymentReadDto, Pages.CustomerPayment.UpdateModel.CustomerPaymentUpdateViewModel>();


            /*VendorBill*/
            CreateMap<Pages.VendorBill.CreateModel.CreateVendorBillViewModel, VendorBillCreateDto>();
            CreateMap<Pages.VendorBill.UpdateModel.VendorBillUpdateViewModel, VendorBillUpdateDto>();
            CreateMap<VendorBillReadDto, Pages.VendorBill.UpdateModel.VendorBillUpdateViewModel>();
            CreateMap<VendorBillReadDto, Pages.VendorBill.DetailModel.VendorBillViewModel>();

            /*VendorBillDetail*/
            CreateMap<Pages.VendorBill.CreateDetailModel.CreateVendorBillDetailViewModel, VendorBillDetailCreateDto>();
            CreateMap<Pages.VendorBill.UpdateDetailModel.VendorBillDetailUpdateViewModel, VendorBillDetailUpdateDto>();
            CreateMap<VendorBillDetailReadDto, Pages.VendorBill.UpdateDetailModel.VendorBillDetailUpdateViewModel>();


            /*VendorPayment*/
            CreateMap<Pages.VendorPayment.CreateModel.CreateVendorPaymentViewModel, VendorPaymentCreateDto>();
            CreateMap<Pages.VendorPayment.UpdateModel.VendorPaymentUpdateViewModel, VendorPaymentUpdateDto>();
            CreateMap<VendorPaymentReadDto, Pages.VendorPayment.UpdateModel.VendorPaymentUpdateViewModel>();


            /*VendorDebitNote*/
            CreateMap<Pages.VendorDebitNote.CreateModel.CreateVendorDebitNoteViewModel, VendorDebitNoteCreateDto>();
            CreateMap<Pages.VendorDebitNote.UpdateModel.VendorDebitNoteUpdateViewModel, VendorDebitNoteUpdateDto>();
            CreateMap<VendorDebitNoteReadDto, Pages.VendorDebitNote.UpdateModel.VendorDebitNoteUpdateViewModel>();
            CreateMap<VendorDebitNoteReadDto, Pages.VendorDebitNote.DetailModel.VendorDebitNoteViewModel>();
            CreateMap<VendorDebitNoteReadDto, Pages.VendorDebitNote.PrintDebitNoteModel.VendorDebitNoteViewModel>();

            /*VendorDebitNoteDetail*/
            CreateMap<Pages.VendorDebitNote.CreateDetailModel.CreateVendorDebitNoteDetailViewModel, VendorDebitNoteDetailCreateDto>();
            CreateMap<Pages.VendorDebitNote.UpdateDetailModel.VendorDebitNoteDetailUpdateViewModel, VendorDebitNoteDetailUpdateDto>();
            CreateMap<VendorDebitNoteDetailReadDto, Pages.VendorDebitNote.UpdateDetailModel.VendorDebitNoteDetailUpdateViewModel>();
            CreateMap<VendorDebitNoteDetailReadDto, Pages.VendorDebitNote.PrintDebitNoteModel.VendorDebitNoteDetailViewModel>();
            CreateMap<PagedResultDto<VendorDebitNoteDetailReadDto>, PagedResultDto<Pages.VendorDebitNote.PrintDebitNoteModel.VendorDebitNoteDetailViewModel>>();





        }
    }
}
