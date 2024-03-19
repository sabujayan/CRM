using AutoMapper;
using Indo.Activities;
using Indo.Bookings;
using Indo.Calendars;
using Indo.CashAndBanks;
using Indo.Clientes;
using Indo.Companies;
using Indo.Contacts;
using Indo.Currencies;
using Indo.CustomerCreditNoteDetails;
using Indo.CustomerCreditNotes;
using Indo.CustomerInvoiceDetails;
using Indo.CustomerInvoices;
using Indo.CustomerPayments;
using Indo.Customers;
using Indo.DeliveryOrderDetails;
using Indo.DeliveryOrders;
using Indo.Departments;
using Indo.EmailsTemplates;
using Indo.EmployeeClient;
using Indo.Employees;
using Indo.EmployeeSkillMatrices;
using Indo.ExpenseDetails;
using Indo.Expenses;
using Indo.ExpenseTypes;
using Indo.GoodsReceiptDetails;
using Indo.GoodsReceipts;
using Indo.ImportantDates;
using Indo.LeadRatings;
using Indo.Leads;
using Indo.LeadSources;
using Indo.Movements;
using Indo.Notes;
using Indo.NumberSequences;
using Indo.Products;
using Indo.ProjectEmployee;
using Indo.Projectes;
using Indo.ProjectOrderDetails;
using Indo.ProjectOrders;
using Indo.PurchaseOrderDetails;
using Indo.PurchaseOrders;
using Indo.PurchaseReceiptDetails;
using Indo.PurchaseReceipts;
using Indo.Resources;
using Indo.SalesDeliveries;
using Indo.SalesDeliveryDetails;
using Indo.SalesOrderDetails;
using Indo.SalesOrders;
using Indo.SalesQuotationDetails;
using Indo.SalesQuotations;
using Indo.ServiceOrderDetails;
using Indo.ServiceOrders;
using Indo.ServiceQuotationDetails;
using Indo.ServiceQuotations;
using Indo.Services;
using Indo.Skills;
using Indo.StockAdjustmentDetails;
using Indo.StockAdjustments;
using Indo.Stocks;
using Indo.Tasks;
using Indo.Technologies;
using Indo.Todos;
using Indo.TransferOrderDetails;
using Indo.TransferOrders;
using Indo.Uoms;
using Indo.VendorBillDetails;
using Indo.VendorBills;
using Indo.VendorDebitNoteDetails;
using Indo.VendorDebitNotes;
using Indo.VendorPayments;
using Indo.Vendors;
using Indo.Warehouses;
using static Indo.Permissions.IndoPermissions;

namespace Indo
{
    public class IndoApplicationAutoMapperProfile : Profile
    {
        public IndoApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            /*SalesExecutive*/
            CreateMap<Employee, EmployeeReadDto>();
            CreateMap<EmployeeReadDto, ServiceOrders.SalesExecutiveLookupDto>();
            CreateMap<EmployeeReadDto, ProjectOrders.SalesExecutiveLookupDto>();
            CreateMap<EmployeeReadDto, SalesOrders.SalesExecutiveLookupDto>();
            CreateMap<EmployeeReadDto, ServiceQuotations.SalesExecutiveLookupDto>();
            CreateMap<EmployeeReadDto, SalesQuotations.SalesExecutiveLookupDto>();

            /*Product*/
            CreateMap<Product, ProductReadDto>();
            CreateMap<Product, PurchaseOrderDetails.ProductLookupDto>();
            CreateMap<Product, SalesOrderDetails.ProductLookupDto>();
            CreateMap<Product, PurchaseReceiptDetails.ProductLookupDto>();
            CreateMap<Product, SalesDeliveryDetails.ProductLookupDto>();
            CreateMap<Product, TransferOrderDetails.ProductLookupDto>();
            CreateMap<Product, DeliveryOrderDetails.ProductLookupDto>();
            CreateMap<Product, GoodsReceiptDetails.ProductLookupDto>();
            CreateMap<Product, StockAdjustmentDetails.ProductLookupDto>();
            CreateMap<Product, SalesQuotationDetails.ProductLookupDto>();

            /*Currency*/
            CreateMap<Currency, CurrencyReadDto>();
            CreateMap<Currency, CurrencyLookupDto>();

            /*Customer*/
            CreateMap<Customer, CustomerReadDto>();
            CreateMap<Customer, ServiceOrders.CustomerLookupDto>();
            CreateMap<Customer, ProjectOrders.CustomerLookupDto>();
            CreateMap<Customer, SalesOrders.CustomerLookupDto>();
            CreateMap<Customer, Expenses.CustomerLookupDto>();
            CreateMap<Customer, Notes.CustomerLookupDto>();
            CreateMap<Customer, ImportantDates.CustomerLookupDto>();
            CreateMap<Customer, Contacts.CustomerLookupDto>();
            CreateMap<Customer, Tasks.CustomerLookupDto>();
            CreateMap<Customer, ServiceQuotations.CustomerLookupDto>();
            CreateMap<Customer, SalesQuotations.CustomerLookupDto>();
            CreateMap<Customer, CustomerInvoices.CustomerLookupDto>();
            CreateMap<Customer, CustomerCreditNotes.CustomerLookupDto>();
            CreateMap<Customer, CustomerPayments.CustomerLookupDto>();

            /*Company*/
            CreateMap<Company, CompanyReadDto>();

            /*Uom*/
            CreateMap<Uom, UomReadDto>();
            CreateMap<Uom, Products.UomLookupDto>();
            CreateMap<Uom, Services.UomLookupDto>();
            CreateMap<Uom, CustomerInvoiceDetails.UomLookupDto>();
            CreateMap<Uom, CustomerCreditNoteDetails.UomLookupDto>();
            CreateMap<Uom, VendorBillDetails.UomLookupDto>();
            CreateMap<Uom, VendorDebitNoteDetails.UomLookupDto>();

            /*ServiceOrder*/
            CreateMap<ServiceOrder, ServiceOrderReadDto>();
            CreateMap<ServiceOrder, ServiceOrderLookupDto>();

            /*ServiceOrderDetail*/
            CreateMap<ServiceOrderDetail, ServiceOrderDetailReadDto>();

            /*ProjectOrder*/
            CreateMap<ProjectOrder, ProjectOrderReadDto>();
            CreateMap<ProjectOrder, ProjectOrderLookupDto>();

            /*ProjectOrderDetail*/
            CreateMap<ProjectOrderDetail, ProjectOrderDetailReadDto>();

            /*NumberSequence*/
            CreateMap<NumberSequence, NumberSequenceReadDto>();

            /*Buyer*/
            CreateMap<Employee, EmployeeReadDto>();
            CreateMap<EmployeeReadDto, PurchaseOrders.BuyerLookupDto>();

            /*Vendor*/
            CreateMap<Vendor, VendorReadDto>();
            CreateMap<Vendor, PurchaseOrders.VendorLookupDto>();
            CreateMap<Vendor, VendorBills.VendorLookupDto>();
            CreateMap<Vendor, VendorDebitNotes.VendorLookupDto>();
            CreateMap<Vendor, VendorPayments.VendorLookupDto>();

            /*Warehouse*/
            CreateMap<Warehouse, WarehouseReadDto>();
            CreateMap<Warehouse, Companies.WarehouseLookupDto>();
            CreateMap<Warehouse, TransferOrders.WarehouseLookupDto>();
            CreateMap<Warehouse, DeliveryOrders.WarehouseLookupDto>();
            CreateMap<Warehouse, GoodsReceipts.WarehouseLookupDto>();
            CreateMap<Warehouse, StockAdjustments.WarehouseLookupDto>();
            CreateMap<Warehouse, Movements.WarehouseLookupDto>();

            /*Service*/
            CreateMap<Service, ServiceReadDto>();
            CreateMap<Service, ServiceOrderDetails.ServiceLookupDto>();
            CreateMap<Service, ServiceQuotationDetails.ServiceLookupDto>();

            /*PurchaseOrder*/
            CreateMap<PurchaseOrder, PurchaseOrderReadDto>();
            CreateMap<PurchaseOrder, PurchaseOrderDetails.PurchaseOrderLookupDto>();
            CreateMap<PurchaseOrder, PurchaseReceipts.PurchaseOrderLookupDto>();

            /*PurchaseOrderDetail*/
            CreateMap<PurchaseOrderDetail, PurchaseOrderDetailReadDto>();

            /*SalesOrder*/
            CreateMap<SalesOrder, SalesOrderReadDto>();
            CreateMap<SalesOrder, SalesOrderDetails.SalesOrderLookupDto>();
            CreateMap<SalesOrder, SalesDeliveries.SalesOrderLookupDto>();

            /*ClientRegister*/
            CreateMap<Clients, ClientRegisterDto>();

            /*SalesOrderDetail*/
            CreateMap<SalesOrderDetail, SalesOrderDetailReadDto>();

            /*PurchaseReceipt*/
            CreateMap<PurchaseReceipt, PurchaseReceiptReadDto>();
            CreateMap<PurchaseReceipt, PurchaseReceiptLookupDto>();

            /*PurchaseReceiptDetail*/
            CreateMap<PurchaseReceiptDetail, PurchaseReceiptDetailReadDto>();

            /*SalesDelivery*/
            CreateMap<SalesDelivery, SalesDeliveryReadDto>();
            CreateMap<SalesDelivery, SalesDeliveryLookupDto>();

            /*SalesDeliveryDetail*/
            CreateMap<SalesDeliveryDetail, SalesDeliveryDetailReadDto>();

            /*TransferOrder*/
            CreateMap<TransferOrder, TransferOrderReadDto>();
            CreateMap<TransferOrder, TransferOrderDetails.TransferOrderLookupDto>();
            CreateMap<TransferOrder, DeliveryOrders.TransferOrderLookupDto>();

            /*TransferOrderDetail*/
            CreateMap<TransferOrderDetail, TransferOrderDetailReadDto>();

            /*DeliveryOrder*/
            CreateMap<DeliveryOrder, DeliveryOrderReadDto>();
            CreateMap<DeliveryOrder, DeliveryOrderDetails.DeliveryOrderLookupDto>();
            CreateMap<DeliveryOrder, GoodsReceipts.DeliveryOrderLookupDto>();

            /*DeliveryOrderDetail*/
            CreateMap<DeliveryOrderDetail, DeliveryOrderDetailReadDto>();

            /*GoodsReceipt*/
            CreateMap<GoodsReceipt, GoodsReceiptReadDto>();
            CreateMap<GoodsReceipt, GoodsReceiptLookupDto>();

            /*GoodsReceiptDetail*/
            CreateMap<GoodsReceiptDetail, GoodsReceiptDetailReadDto>();

            /*StockAdjustment*/
            CreateMap<StockAdjustment, StockAdjustmentReadDto>();
            CreateMap<StockAdjustment, StockAdjustmentLookupDto>();

            /*StockAdjustmentDetail*/
            CreateMap<StockAdjustmentDetail, StockAdjustmentDetailReadDto>();

            /*Movement*/
            CreateMap<Movement, MovementReadDto>();

            /*Stock*/
            CreateMap<Stock, StockReadDto>();

            /*Department*/
            CreateMap<Department, DepartmentReadDto>();
            CreateMap<Department, DepartmentLookupDto>();


            /*Clients*/
            CreateMap<Clients, ClientsReadDto>();
            CreateMap<Clients, ClientsLookupDto>();

            //skill//
            CreateMap<Skill, SkillReadDto>();
            CreateMap<Skill, SkillLookupDto>();

            /*Employee*/
            CreateMap<Employee, EmployeeReadDto>();
            CreateMap<Employee, EmployeeLookupDto>();
            //CreateMap<Employee, EmployeeSkillMatricesCreateDto>();
            //CreateMap<EmployeeSkillMatricesCreateDto, EmployeeSkills.EmployeeSkillMatrices>();
            //CreateMap<EmployeeSkills.EmployeeSkillMatrices, EmployeeSkillMatricesCreateDto>();

            //CreateMap<EmployeesClientsMatricesCreateDto, EmployeeClient.EmployeesClientsMatrices>();
            //CreateMap<EmployeeClient.EmployeesClientsMatrices, EmployeesClientsMatricesCreateDto>();

            //CreateMap<EmployeesProjectsMatricesCreateDto, EmployeesProjectsMatrices>();
            //CreateMap<EmployeesProjectsMatrices, EmployeesProjectsMatricesCreateDto>();


            /*ExpenseType*/
            CreateMap<ExpenseType, ExpenseTypeReadDto>();
            CreateMap<ExpenseType, ExpenseTypeLookupDto>();

            /*Resource*/
            CreateMap<Resource, ResourceReadDto>();
            CreateMap<Resource, ResourceLookupDto>();

            /*Todo*/
            CreateMap<Todo, TodoReadDto>();

            /*Booking*/
            CreateMap<Booking, BookingReadDto>();

            /*Expense*/
            CreateMap<Expense, ExpenseReadDto>();
            CreateMap<Expense, ExpenseLookupDto>();

            /*Calendar*/
            CreateMap<Calendar, CalendarReadDto>();

            /*ExpenseDetail*/
            CreateMap<ExpenseDetail, ExpenseDetailReadDto>();

            /*Note*/
            CreateMap<Note, NoteReadDto>();

            /*ImportantDate*/
            CreateMap<ImportantDate, ImportantDateReadDto>();

            /*Contact*/
            CreateMap<Contact, ContactReadDto>();

            /*Activity*/
            CreateMap<Activity, ActivityReadDto>();
            CreateMap<Activity, Tasks.ActivityLookupDto>();

            /*LeadRating*/
            CreateMap<LeadRating, LeadRatingReadDto>();
            CreateMap<LeadRating, LeadRatingLookupDto>();

            /*LeadSource*/
            CreateMap<LeadSource, LeadSourceReadDto>();
            CreateMap<LeadSource, LeadSourceLookupDto>();

            /*Task*/
            CreateMap<Task, TaskReadDto>();

            /*ServiceQuotation*/
            CreateMap<ServiceQuotation, ServiceQuotationReadDto>();
            CreateMap<ServiceQuotation, ServiceQuotationLookupDto>();

            /*ServiceQuotationDetail*/
            CreateMap<ServiceQuotationDetail, ServiceQuotationDetailReadDto>();

            /*SalesQuotation*/
            CreateMap<SalesQuotation, SalesQuotationReadDto>();
            CreateMap<SalesQuotation, SalesQuotationLookupDto>();

            /*SalesQuotationDetail*/
            CreateMap<SalesQuotationDetail, SalesQuotationDetailReadDto>();

            /*CashAndBank*/
            CreateMap<CashAndBank, CashAndBankReadDto>();
            CreateMap<CashAndBank, CustomerPayments.CashAndBankLookupDto>();
            CreateMap<CashAndBank, VendorPayments.CashAndBankLookupDto>();

            /*CustomerInvoice*/
            CreateMap<CustomerInvoice, CustomerInvoiceReadDto>();
            CreateMap<CustomerInvoice, CustomerInvoiceDetails.CustomerInvoiceLookupDto>();
            CreateMap<CustomerInvoice, CustomerCreditNotes.CustomerInvoiceLookupDto>();

            /*CustomerInvoiceDetail*/
            CreateMap<CustomerInvoiceDetail, CustomerInvoiceDetailReadDto>();

            /*CustomerCreditNote*/
            CreateMap<CustomerCreditNote, CustomerCreditNoteReadDto>();
            CreateMap<CustomerCreditNote, CustomerCreditNoteDetails.CustomerCreditNoteLookupDto>();

            /*CustomerCreditNoteDetail*/
            CreateMap<CustomerCreditNoteDetail, CustomerCreditNoteDetailReadDto>();

            /*CustomerPayment*/
            CreateMap<CustomerPayment, CustomerPaymentReadDto>();

            /*VendorBill*/
            CreateMap<VendorBill, VendorBillReadDto>();
            CreateMap<VendorBill, VendorBillDetails.VendorBillLookupDto>();
            CreateMap<VendorBill, VendorDebitNotes.VendorBillLookupDto>();

            /*VendorBillDetail*/
            CreateMap<VendorBillDetail, VendorBillDetailReadDto>();

            /*VendorDebitNote*/
            CreateMap<VendorDebitNote, VendorDebitNoteReadDto>();
            CreateMap<VendorDebitNote, VendorDebitNoteDetails.VendorDebitNoteLookupDto>();

            /*VendorDebitNoteDetail*/
            CreateMap<VendorDebitNoteDetail, VendorDebitNoteDetailReadDto>();

            /*VendorPayment*/
            CreateMap<VendorPayment, VendorPaymentReadDto>();

            /*Project*/
            CreateMap<Projects, ProjectsReadDto>();
            CreateMap<Projects, ProjectsLookupDto>();

            /*Technology*/
            CreateMap<Technology, TechnologyReadDto>();
            CreateMap<Technology, TechnologyLookupDto>();

            /*Employee*/
            CreateMap<Employee, EmployeeLookupDto>();

            /*EmailTemplates*/
            CreateMap<EmailTemplates, EmailsTemplatesLookUpDto>();

            /*LeadsInfo*/
            CreateMap<LeadsInfo, LeadsDto>();
        }
    }
}
