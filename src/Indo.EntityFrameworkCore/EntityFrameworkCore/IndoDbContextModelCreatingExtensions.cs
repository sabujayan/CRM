using Indo.Accounts;
using Indo.Activities;
using Indo.Addresss;
using Indo.Bookings;
using Indo.Calendars;
using Indo.CashAndBanks;
using Indo.Clientes;
using Indo.ClientesAddress;
using Indo.ClientesContact;
using Indo.ClientsProjects;
using Indo.Companies;
using Indo.Contacts;
using Indo.ContactsAddress;
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
using Indo.EmailsAttachments;
using Indo.EmailsInformtions;
using Indo.EmailsTemplates;
using Indo.EmployeeClient;
using Indo.Employees;
using Indo.EmployeeSkills;
using Indo.ExpenseDetails;
using Indo.Expenses;
using Indo.ExpenseTypes;
using Indo.GoodsReceiptDetails;
using Indo.GoodsReceipts;
using Indo.ImportantDates;
using Indo.LeadRatings;
using Indo.Leads;
using Indo.LeadsAddress;
using Indo.LeadSources;
using Indo.Movements;
using Indo.Notes;
using Indo.NumberSequences;
using Indo.Products;
using Indo.ProjectEmployee;
using Indo.Projectes;
using Indo.ProjectOrderDetails;
using Indo.ProjectOrders;
using Indo.ProjectsTechnologies;
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
using Indo.Users;
using Indo.VendorBillDetails;
using Indo.VendorBills;
using Indo.VendorDebitNoteDetails;
using Indo.VendorDebitNotes;
using Indo.VendorPayments;
using Indo.Vendors;
using Indo.Warehouses;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;
using Volo.Abp.Users.EntityFrameworkCore;

namespace Indo.EntityFrameworkCore
{
    public static class IndoDbContextModelCreatingExtensions
    {
        public static void ConfigureIndo(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<Customer>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "Customers", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).IsRequired().HasMaxLength(CustomerConsts.MaxNameLength);
            });
            builder.Entity<Skill>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "Skills", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).IsRequired().HasMaxLength(SkillConsts.MaxNameLength);
            });
            /*builder.Entity<EmployeeSkillMatrices>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "EmployeeSkillMatricess", IndoConsts.DbSchema);
                b.ConfigureByConvention();
             });*/
            builder.Entity<EmployeesClientsMatrices>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "EmployeeClientMatrices", IndoConsts.DbSchema);
                b.ConfigureByConvention();
            });
            builder.Entity<EmployeesProjectsMatrices>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "EmployeeProjectMatrices", IndoConsts.DbSchema);
                b.ConfigureByConvention();
            });

            builder.Entity<ClientsProjectsMatrices>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "ClientProjectMatrices", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasOne<Projects>().WithMany().HasForeignKey(x => x.ProjectsId).IsRequired();
                b.HasOne<Clients>().WithMany().HasForeignKey(x => x.ClientsId).IsRequired();
            });

            builder.Entity<Product>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "Products", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).IsRequired().HasMaxLength(ProductConsts.MaxNameLength);
                b.HasOne<Uom>().WithMany().HasForeignKey(x => x.UomId).IsRequired();
            });

            builder.Entity<Currency>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "Currencies", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).IsRequired().HasMaxLength(CurrencyConsts.MaxNameLength);
            });

            builder.Entity<Company>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "Companies", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).IsRequired().HasMaxLength(CompanyConsts.MaxNameLength);
                b.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).IsRequired();
                b.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.DefaultWarehouseId).IsRequired();
            });

            builder.Entity<Uom>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "Uoms", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).IsRequired().HasMaxLength(UomConsts.MaxNameLength);
            });

            builder.Entity<ServiceOrder>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "ServiceOrders", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Number).IsRequired().HasMaxLength(ServiceOrderConsts.MaxNumberLength);
                b.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId).IsRequired();
                b.HasOne<Employee>().WithMany().HasForeignKey(x => x.SalesExecutiveId).IsRequired();
                b.Property(x => x.OrderDate).IsRequired();
            });

            builder.Entity<ServiceOrderDetail>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "ServiceOrderDetails", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasOne<ServiceOrder>().WithMany().HasForeignKey(x => x.ServiceOrderId).IsRequired();
                b.HasOne<Service>().WithMany().HasForeignKey(x => x.ServiceId).IsRequired();
            });

            builder.Entity<ProjectOrder>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "ProjectOrders", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Number).IsRequired().HasMaxLength(ProjectOrderConsts.MaxNumberLength);
                b.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId).IsRequired();
                b.HasOne<Employee>().WithMany().HasForeignKey(x => x.SalesExecutiveId).IsRequired();
                b.Property(x => x.OrderDate).IsRequired();
            });

            builder.Entity<ProjectOrderDetail>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "ProjectOrderDetails", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasOne<ProjectOrder>().WithMany().HasForeignKey(x => x.ProjectOrderId).IsRequired();
            });

            builder.Entity<NumberSequence>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "NumberSequences", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Suffix).IsRequired().HasMaxLength(NumberSequenceConsts.MaxPrefixLength);
            });

            builder.Entity<Vendor>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "Vendors", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).IsRequired().HasMaxLength(VendorConsts.MaxNameLength);
            });

            builder.Entity<PurchaseOrder>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "PurchaseOrders", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Number).IsRequired().HasMaxLength(PurchaseOrderConsts.MaxNumberLength);
                b.HasOne<Vendor>().WithMany().HasForeignKey(x => x.VendorId).IsRequired();
                b.HasOne<Employee>().WithMany().HasForeignKey(x => x.BuyerId).IsRequired();
                b.Property(x => x.OrderDate).IsRequired();
            });

            builder.Entity<PurchaseOrderDetail>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "PurchaseOrderDetails", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasOne<PurchaseOrder>().WithMany().HasForeignKey(x => x.PurchaseOrderId).IsRequired();
                b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId).IsRequired();
            });

            builder.Entity<PurchaseReceipt>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "PurchaseReceipts", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Number).IsRequired().HasMaxLength(PurchaseReceiptConsts.MaxNumberLength);
                b.HasOne<PurchaseOrder>().WithMany().HasForeignKey(x => x.PurchaseOrderId).IsRequired();
                b.Property(x => x.ReceiptDate).IsRequired();
            });

            builder.Entity<PurchaseReceiptDetail>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "PurchaseReceiptDetails", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasOne<PurchaseReceipt>().WithMany().HasForeignKey(x => x.PurchaseReceiptId).IsRequired();
                b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId).IsRequired();
            });

            builder.Entity<SalesOrder>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "SalesOrders", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Number).IsRequired().HasMaxLength(SalesOrderConsts.MaxNumberLength);
                b.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId).IsRequired();
                b.HasOne<Employee>().WithMany().HasForeignKey(x => x.SalesExecutiveId).IsRequired();
                b.Property(x => x.OrderDate).IsRequired();
            });

            builder.Entity<SalesOrderDetail>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "SalesOrderDetails", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasOne<SalesOrder>().WithMany().HasForeignKey(x => x.SalesOrderId).IsRequired();
                b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId).IsRequired();
            });

            builder.Entity<SalesDelivery>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "SalesDeliveries", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Number).IsRequired().HasMaxLength(SalesDeliveryConsts.MaxNumberLength);
                b.HasOne<SalesOrder>().WithMany().HasForeignKey(x => x.SalesOrderId).IsRequired();
                b.Property(x => x.DeliveryDate).IsRequired();
            });

            builder.Entity<SalesDeliveryDetail>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "SalesDeliveryDetails", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasOne<SalesDelivery>().WithMany().HasForeignKey(x => x.SalesDeliveryId).IsRequired();
                b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId).IsRequired();
            });

            builder.Entity<Warehouse>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "Warehouses", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).IsRequired().HasMaxLength(WarehouseConsts.MaxNameLength);
            });

            builder.Entity<TransferOrder>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "TransferOrders", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Number).IsRequired().HasMaxLength(TransferOrderConsts.MaxNumberLength);
                b.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.FromWarehouseId).OnDelete(DeleteBehavior.NoAction).IsRequired();
                b.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.ToWarehouseId).OnDelete(DeleteBehavior.NoAction).IsRequired();
                b.Property(x => x.OrderDate).IsRequired();
            });

            builder.Entity<TransferOrderDetail>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "TransferOrderDetails", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasOne<TransferOrder>().WithMany().HasForeignKey(x => x.TransferOrderId).IsRequired();
                b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId).IsRequired();
            });

            builder.Entity<DeliveryOrder>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "DeliveryOrders", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Number).IsRequired().HasMaxLength(DeliveryOrderConsts.MaxNumberLength);
                b.HasOne<TransferOrder>().WithMany().HasForeignKey(x => x.TransferOrderId).IsRequired();
                b.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.FromWarehouseId).OnDelete(DeleteBehavior.NoAction).IsRequired();
                b.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.ToWarehouseId).OnDelete(DeleteBehavior.NoAction).IsRequired();
                b.Property(x => x.OrderDate).IsRequired();
            });

            builder.Entity<DeliveryOrderDetail>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "DeliveryOrderDetails", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasOne<DeliveryOrder>().WithMany().HasForeignKey(x => x.DeliveryOrderId).IsRequired();
                b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId).IsRequired();
            });

            builder.Entity<GoodsReceipt>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "GoodsReceipts", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Number).IsRequired().HasMaxLength(GoodsReceiptConsts.MaxNumberLength);
                b.HasOne<DeliveryOrder>().WithMany().HasForeignKey(x => x.DeliveryOrderId).IsRequired();
                b.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.FromWarehouseId).OnDelete(DeleteBehavior.NoAction).IsRequired();
                b.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.ToWarehouseId).OnDelete(DeleteBehavior.NoAction).IsRequired();
                b.Property(x => x.OrderDate).IsRequired();
            });

            builder.Entity<GoodsReceiptDetail>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "GoodsReceiptDetails", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasOne<GoodsReceipt>().WithMany().HasForeignKey(x => x.GoodsReceiptId).IsRequired();
                b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId).IsRequired();
            });

            builder.Entity<StockAdjustment>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "StockAdjustments", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Number).IsRequired().HasMaxLength(StockAdjustmentConsts.MaxNumberLength);
                b.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.WarehouseId).OnDelete(DeleteBehavior.NoAction).IsRequired();
                b.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.FromWarehouseId).OnDelete(DeleteBehavior.NoAction).IsRequired();
                b.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.ToWarehouseId).OnDelete(DeleteBehavior.NoAction).IsRequired();
                b.Property(x => x.AdjustmentDate).IsRequired();
            });

            builder.Entity<StockAdjustmentDetail>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "StockAdjustmentDetails", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasOne<StockAdjustment>().WithMany().HasForeignKey(x => x.StockAdjustmentId).IsRequired();
                b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId).IsRequired();
            });

            builder.Entity<Movement>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "Movements", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Number).IsRequired().HasMaxLength(MovementConsts.MaxNumberLength);
                b.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.FromWarehouseId).OnDelete(DeleteBehavior.NoAction).IsRequired();
                b.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.ToWarehouseId).OnDelete(DeleteBehavior.NoAction).IsRequired();
                b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId).IsRequired();
                b.Property(x => x.MovementDate).IsRequired();
                b.Property(x => x.SourceDocument).IsRequired();
                b.Property(x => x.Module).IsRequired();
            });

            builder.Entity<Stock>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "Stocks", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasOne<Movement>().WithMany().HasForeignKey(x => x.MovementId).IsRequired();
                b.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.WarehouseId).IsRequired();
                b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.NoAction).IsRequired();
                b.Property(x => x.TransactionDate).IsRequired();
                b.Property(x => x.SourceDocument).IsRequired();
                b.Property(x => x.Flow).IsRequired();
                b.Property(x => x.Qty).IsRequired();
            });

            builder.Entity<Service>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "Services", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).IsRequired().HasMaxLength(ServiceConsts.MaxNameLength);
                b.HasOne<Uom>().WithMany().HasForeignKey(x => x.UomId).IsRequired();
            });

            builder.Entity<Department>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "Departments", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).IsRequired().HasMaxLength(DepartmentConsts.MaxNameLength);
            });

            builder.Entity<Clients>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "Clientses", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).IsRequired().HasMaxLength(ClientConsts.MaxNameLength);
            });

            builder.Entity<ClientsAddress>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "ClientAddress", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Address).IsRequired().HasMaxLength(ClientsAddressConsts.MaxNameLength);
                b.HasOne<Clients>().WithMany().HasForeignKey(x => x.ClientsId).IsRequired();
            });

            builder.Entity<ClientsContact>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "ClientContact", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Email).IsRequired().HasMaxLength(ClientsContactConsts.MaxNameLength);
                b.HasOne<Clients>().WithMany().HasForeignKey(x => x.ClientsId).IsRequired();
            });

            builder.Entity<Employee>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "Employees", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).IsRequired().HasMaxLength(EmployeeConsts.MaxNameLength);
                b.Property(x => x.EmployeeNumber).IsRequired().HasMaxLength(EmployeeConsts.MaxNameLength);
                b.HasOne<Department>().WithMany().HasForeignKey(x => x.DepartmentId).IsRequired();
            });

            builder.Entity<Projects>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "Project", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).IsRequired().HasMaxLength(ProjectsConsts.MaxNameLength);
                b.HasOne<Clients>().WithMany().HasForeignKey(x => x.ClientsId).IsRequired();
            });

            builder.Entity<ExpenseType>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "ExpenseTypes", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).IsRequired().HasMaxLength(ExpenseTypeConsts.MaxNameLength);
            });

            builder.Entity<Resource>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "Resources", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).IsRequired().HasMaxLength(ResourceConsts.MaxNameLength);
            });

            builder.Entity<Todo>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "Todos", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).IsRequired().HasMaxLength(TodoConsts.MaxNameLength);
            });

            builder.Entity<Booking>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "Bookings", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).IsRequired().HasMaxLength(BookingConsts.MaxNameLength);
                b.HasOne<Resource>().WithMany().HasForeignKey(x => x.ResourceId).IsRequired();
            });

            builder.Entity<Expense>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "Expenses", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Number).IsRequired().HasMaxLength(ExpenseConsts.MaxNumberLength);
                b.HasOne<Employee>().WithMany().HasForeignKey(x => x.EmployeeId).IsRequired();
                b.HasOne<ExpenseType>().WithMany().HasForeignKey(x => x.ExpenseTypeId).IsRequired();
                b.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId).IsRequired();
            });

            builder.Entity<ExpenseDetail>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "ExpenseDetails", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasOne<Expense>().WithMany().HasForeignKey(x => x.ExpenseId).IsRequired();
            });

            builder.Entity<Calendar>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "Calendars", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).IsRequired().HasMaxLength(CalendarConsts.MaxNameLength);
            });

            builder.Entity<Note>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "Notes", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Description).IsRequired();
                b.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId).IsRequired();
            });

            builder.Entity<ImportantDate>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "ImportantDates", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).IsRequired().HasMaxLength(ImportantDateConsts.MaxNameLength);
                b.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId).IsRequired();
            });

            builder.Entity<Contact>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "Contacts", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).IsRequired().HasMaxLength(ContactConsts.MaxNameLength);
                b.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId).IsRequired();
            });

            builder.Entity<Activity>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "Activities", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).IsRequired().HasMaxLength(ActivityConsts.MaxNameLength);
            });

            builder.Entity<LeadRating>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "LeadRatings", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).IsRequired().HasMaxLength(LeadRatingConsts.MaxNameLength);
            });

            builder.Entity<LeadSource>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "LeadSources", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).IsRequired().HasMaxLength(LeadSourceConsts.MaxNameLength);
            });

            builder.Entity<Task>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "Tasks", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).IsRequired().HasMaxLength(TaskConsts.MaxNameLength);
                b.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId).IsRequired();
                b.HasOne<Activity>().WithMany().HasForeignKey(x => x.ActivityId).IsRequired();
            });

            builder.Entity<ServiceQuotation>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "ServiceQuotations", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Number).IsRequired().HasMaxLength(ServiceQuotationConsts.MaxNumberLength);
                b.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId).IsRequired();
                b.HasOne<Employee>().WithMany().HasForeignKey(x => x.SalesExecutiveId).IsRequired();
                b.Property(x => x.QuotationDate).IsRequired();
                b.Property(x => x.QuotationValidUntilDate).IsRequired();
            });

            builder.Entity<ServiceQuotationDetail>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "ServiceQuotationDetails", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasOne<ServiceQuotation>().WithMany().HasForeignKey(x => x.ServiceQuotationId).IsRequired();
                b.HasOne<Service>().WithMany().HasForeignKey(x => x.ServiceId).IsRequired();
            });

            builder.Entity<SalesQuotation>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "SalesQuotations", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Number).IsRequired().HasMaxLength(SalesQuotationConsts.MaxNumberLength);
                b.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId).IsRequired();
                b.HasOne<Employee>().WithMany().HasForeignKey(x => x.SalesExecutiveId).IsRequired();
                b.Property(x => x.QuotationDate).IsRequired();
                b.Property(x => x.QuotationValidUntilDate).IsRequired();
            });

            builder.Entity<SalesQuotationDetail>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "SalesQuotationDetails", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasOne<SalesQuotation>().WithMany().HasForeignKey(x => x.SalesQuotationId).IsRequired();
                b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId).IsRequired();
            });

            builder.Entity<CashAndBank>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "CashAndBanks", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).IsRequired().HasMaxLength(CashAndBankConsts.MaxNameLength);
            });

            builder.Entity<CustomerInvoice>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "CustomerInvoices", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Number).IsRequired().HasMaxLength(CustomerInvoiceConsts.MaxNumberLength);
                b.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId).IsRequired();
                b.Property(x => x.InvoiceDate).IsRequired();
                b.Property(x => x.InvoiceDueDate).IsRequired();
            });

            builder.Entity<CustomerInvoiceDetail>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "CustomerInvoiceDetails", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasOne<CustomerInvoice>().WithMany().HasForeignKey(x => x.CustomerInvoiceId).IsRequired();
                b.HasOne<Uom>().WithMany().HasForeignKey(x => x.UomId).IsRequired();
            });

            builder.Entity<CustomerCreditNote>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "CustomerCreditNotes", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Number).IsRequired().HasMaxLength(CustomerCreditNoteConsts.MaxNumberLength);
                b.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId).IsRequired().OnDelete(DeleteBehavior.NoAction);
                b.HasOne<CustomerInvoice>().WithMany().HasForeignKey(x => x.CustomerInvoiceId).IsRequired();
                b.Property(x => x.CreditNoteDate).IsRequired();
            });

            builder.Entity<CustomerCreditNoteDetail>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "CustomerCreditNoteDetails", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasOne<CustomerCreditNote>().WithMany().HasForeignKey(x => x.CustomerCreditNoteId).IsRequired();
                b.HasOne<Uom>().WithMany().HasForeignKey(x => x.UomId).IsRequired();
            });

            builder.Entity<CustomerPayment>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "CustomerPayments", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Number).IsRequired().HasMaxLength(CustomerPaymentConsts.MaxNumberLength);
                b.HasOne<CashAndBank>().WithMany().HasForeignKey(x => x.CashAndBankId).IsRequired();
                b.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId).IsRequired();
                b.Property(x => x.Amount).IsRequired();
                b.Property(x => x.SourceDocument).IsRequired();
                b.Property(x => x.SourceDocumentId).IsRequired();
                b.Property(x => x.SourceDocumentModule).IsRequired();
            });

            builder.Entity<VendorBill>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "VendorBills", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Number).IsRequired().HasMaxLength(VendorBillConsts.MaxNumberLength);
                b.HasOne<Vendor>().WithMany().HasForeignKey(x => x.VendorId).IsRequired();
                b.Property(x => x.BillDate).IsRequired();
                b.Property(x => x.BillDueDate).IsRequired();
            });

            builder.Entity<VendorBillDetail>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "VendorBillDetailDetails", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasOne<VendorBill>().WithMany().HasForeignKey(x => x.VendorBillId).IsRequired();
                b.HasOne<Uom>().WithMany().HasForeignKey(x => x.UomId).IsRequired();
            });

            builder.Entity<VendorDebitNote>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "VendorDebitNotes", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Number).IsRequired().HasMaxLength(VendorDebitNoteConsts.MaxNumberLength);
                b.HasOne<Vendor>().WithMany().HasForeignKey(x => x.VendorId).IsRequired().OnDelete(DeleteBehavior.NoAction); ;
                b.HasOne<VendorBill>().WithMany().HasForeignKey(x => x.VendorBillId).IsRequired();
                b.Property(x => x.DebitNoteDate).IsRequired();
            });

            builder.Entity<VendorDebitNoteDetail>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "VendorDebitNoteDetails", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasOne<VendorDebitNote>().WithMany().HasForeignKey(x => x.VendorDebitNoteId).IsRequired();
                b.HasOne<Uom>().WithMany().HasForeignKey(x => x.UomId).IsRequired();
            });

            builder.Entity<VendorPayment>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "VendorPayments", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Number).IsRequired().HasMaxLength(VendorPaymentConsts.MaxNumberLength);
                b.HasOne<CashAndBank>().WithMany().HasForeignKey(x => x.CashAndBankId).IsRequired();
                b.HasOne<Vendor>().WithMany().HasForeignKey(x => x.VendorId).IsRequired();
                b.Property(x => x.Amount).IsRequired();
                b.Property(x => x.SourceDocument).IsRequired();
                b.Property(x => x.SourceDocumentId).IsRequired();
                b.Property(x => x.SourceDocumentModule).IsRequired();
            });

            builder.Entity<Technology>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "Technology", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).IsRequired().HasMaxLength(TechnologyConsts.MaxNameLength);
                b.HasOne<Technology>().WithMany().HasForeignKey(x => x.ParentId);
            });

            builder.Entity<ProjectsTechnologyMatrices>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "ProjectTechnologyMatrices", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasOne<Projects>().WithMany().HasForeignKey(x => x.ProjectsId).IsRequired();
                b.HasOne<Technology>().WithMany().HasForeignKey(x => x.TechnologyId).IsRequired();
            });

            builder.Entity<EmailTemplates>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "EmailTemplate", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.TemplateName).IsRequired().HasMaxLength(EmailTemplatesConsts.MaxNameLength);
            });


            builder.Entity<EmailAttachment>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "EmailAttachments", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasOne<EmailTemplates>().WithMany().HasForeignKey(x => x.TemplateId).IsRequired();
            });

            builder.Entity<EmailInformtion>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "EmailInformtions", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasOne<Employee>().WithMany().HasForeignKey(x => x.EmployeeId).IsRequired();
                b.HasOne<EmailTemplates>().WithMany().HasForeignKey(x => x.TemplateId).IsRequired();
            });


            /*builder.Entity<LeadsInfo>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "LeadInfos", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.FirstName).IsRequired().HasMaxLength(LeadsConsts.MaxNameLength);
                b.Property(x => x.LastName).IsRequired().HasMaxLength(LeadsConsts.MaxNameLength);
            });*/

            /*builder.Entity<ContactsInfo>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "ContactsInfos", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.FirstName).IsRequired().HasMaxLength(ContactConsts.MaxNameLength);
                b.Property(x => x.LastName).IsRequired().HasMaxLength(ContactConsts.MaxNameLength);
            });*/

            /*builder.Entity<AccountsInfo>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "AccountsInfos", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.AccountName).IsRequired().HasMaxLength(AccountConsts.MaxNameLength);
            });*/

            builder.Entity<AccountsInfo>(b =>
            {
                b.ConfigureByConvention();
            });

            builder.Entity<AddressInfo>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "AddressInfos", IndoConsts.DbSchema);
                b.ConfigureByConvention();
            });

            builder.Entity<LeadsInfo>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "LeadInfos", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasMany(x => x.LeadsAddresses)
                    .WithOne()
                    .HasForeignKey(x => x.LeadsId);
            });

            builder.Entity<ContactsInfo>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "ContactsInfos", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasMany(x => x.ContactsAddresses)
                    .WithOne()
                    .HasForeignKey(x => x.ContactId);
            });

            builder.Entity<ContactsAddressMatrix>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "ContactsAddressMatrixs", IndoConsts.DbSchema);
                b.ConfigureByConvention();
            });

            builder.Entity<LeadsAddressMatrix>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "LeadsAddressMatrixs", IndoConsts.DbSchema);
                b.ConfigureByConvention();
            });

            /*builder.Entity<AccountsInfo>(b =>
            {
                b.ToTable(IndoConsts.DbTablePrefix + "AccountsInfos", IndoConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasOne<IdentityUser>().WithMany().HasForeignKey(x => x.UserId);
            });*/

        }
    }
}