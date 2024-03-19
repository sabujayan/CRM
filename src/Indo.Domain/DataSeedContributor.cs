using System;
using System.Threading.Tasks;
using Indo.Currencies;
using Indo.Companies;
using Indo.Uoms;
using Indo.Customers;
using Indo.Services;
using Indo.ProjectOrderDetails;
using Indo.ProjectOrders;
using Indo.ServiceOrderDetails;
using Indo.ServiceOrders;
using Volo.Abp.Guids;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using System.Collections.Generic;
using Indo.NumberSequences;
using Indo.Warehouses;
using Indo.Products;
using Indo.PurchaseOrders;
using Indo.PurchaseReceipts;
using Indo.Vendors;
using Indo.PurchaseOrderDetails;
using Indo.PurchaseReceiptDetails;
using Indo.SalesOrders;
using Indo.SalesOrderDetails;
using Indo.SalesDeliveries;
using Indo.TransferOrders;
using Indo.TransferOrderDetails;
using Indo.DeliveryOrders;
using Indo.GoodsReceipts;
using Indo.StockAdjustments;
using Indo.StockAdjustmentDetails;
using Indo.Departments;
using Indo.Employees;
using Indo.ExpenseTypes;
using Indo.Resources;
using Indo.Activities;
using Indo.LeadRatings;
using Indo.LeadSources;
using Indo.CashAndBanks;
using Indo.CustomerInvoices;
using Indo.CustomerPayments;
using Indo.VendorBills;
using Indo.VendorPayments;
using Indo.ServiceQuotations;
using Indo.SalesQuotations;

namespace Indo
{
    public class DataSeedContributor
        : IDataSeedContributor, ITransientDependency
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly CompanyManager _companyManager;
        private readonly IUomRepository _uomRepository;
        private readonly UomManager _uomManager;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly CurrencyManager _currencyManager;
        private readonly ICustomerRepository _customerRepository;
        private readonly CustomerManager _customerManager;
        private readonly IServiceRepository _serviceRepository;
        private readonly ServiceManager _serviceManager;
        private readonly IProjectOrderRepository _projectOrderRepository;
        private readonly IProjectOrderDetailRepository _projectOrderDetailRepository;
        private readonly ProjectOrderManager _projectOrderManager;
        private readonly ProjectOrderDetailManager _projectOrderDetailManager;
        private readonly IServiceOrderRepository _serviceOrderRepository;
        private readonly IServiceOrderDetailRepository _serviceOrderDetailRepository;
        private readonly ServiceOrderManager _serviceOrderManager;
        private readonly ServiceOrderDetailManager _serviceOrderDetailManager;
        private readonly IGuidGenerator _guidGenerator;
        private readonly INumberSequenceRepository _numberSequenceRepository;
        private readonly NumberSequenceManager _numberSequenceManager;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly WarehouseManager _warehouseManager;
        private readonly IProductRepository _productRepository;
        private readonly ProductManager _productManager;
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly PurchaseOrderManager _purchaseOrderManager;
        private readonly IPurchaseReceiptRepository _purchaseReceiptRepository;
        private readonly PurchaseReceiptManager _purchaseReceiptManager;
        private readonly IVendorRepository _vendorRepository;
        private readonly VendorManager _vendorManager;
        private readonly IPurchaseOrderDetailRepository _purchaseOrderDetailRepository;
        private readonly PurchaseOrderDetailManager _purchaseOrderDetailManager;
        private readonly IPurchaseReceiptDetailRepository _purchaseReceiptDetailRepository;
        private readonly PurchaseReceiptDetailManager _purchaseReceiptDetailManager;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly SalesOrderManager _salesOrderManager;
        private readonly ISalesOrderDetailRepository _salesOrderDetailRepository;
        private readonly SalesOrderDetailManager _salesOrderDetailManager;
        private readonly ISalesDeliveryRepository _salesDeliveryRepository;
        private readonly SalesDeliveryManager _salesDeliveryManager;
        private readonly ITransferOrderRepository _transferOrderRepository;
        private readonly TransferOrderManager _transferOrderManager;
        private readonly ITransferOrderDetailRepository _transferOrderDetailRepository;
        private readonly TransferOrderDetailManager _transferOrderDetailManager;
        private readonly DeliveryOrderManager _deliveryOrderManager;
        private readonly GoodsReceiptManager _goodsReceiptManager;
        private readonly IStockAdjustmentRepository _stockAdjustmentRepository;
        private readonly StockAdjustmentManager _stockAdjustmentManager;
        private readonly IStockAdjustmentDetailRepository _stockAdjustmentDetailRepository;
        private readonly StockAdjustmentDetailManager _stockAdjustmentDetailManager;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly DepartmentManager _departmentManager;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly EmployeeManager _employeeManager;
        private readonly IExpenseTypeRepository _expenseTypeRepository;
        private readonly ExpenseTypeManager _expenseTypeManager;
        private readonly IResourceRepository _resourceRepository;
        private readonly ResourceManager _resourceManager;
        private readonly IActivityRepository _activityRepository;
        private readonly ActivityManager _activityManager;
        private readonly ILeadRatingRepository _leadRatingRepository;
        private readonly LeadRatingManager _leadRatingManager;
        private readonly ILeadSourceRepository _leadSourceRepository;
        private readonly LeadSourceManager _leadSourceManager;
        private readonly ICashAndBankRepository _cashAndBankRepository;
        private readonly CashAndBankManager _cashAndBankManager;
        private readonly CustomerInvoiceManager _customerInvoiceManager;
        private readonly ICustomerInvoiceRepository _customerInvoiceRepository;
        private readonly CustomerPaymentManager _customerPaymentManager;
        private readonly ICustomerPaymentRepository _customerPaymentRepository;
        private readonly VendorBillManager _vendorBillManager;
        private readonly IVendorBillRepository _vendorBillRepository;
        private readonly VendorPaymentManager _vendorPaymentManager;
        private readonly IVendorPaymentRepository _vendorPaymentRepository;
        private readonly IServiceQuotationRepository _serviceQuotationRepository;
        private readonly ServiceQuotationManager _serviceQuotationManager;
        private readonly ISalesQuotationRepository _salesQuotationRepository;
        private readonly SalesQuotationManager _salesQuotationManager;



        public DataSeedContributor(
            IStockAdjustmentRepository stockAdjustmentRepository,
            StockAdjustmentManager stockAdjustmentManager,
            IStockAdjustmentDetailRepository stockAdjustmentDetailRepository,
            StockAdjustmentDetailManager stockAdjustmentDetailManager,
            ICompanyRepository companyRepository,
            CompanyManager companyManager,
            IUomRepository uomRepository,
            UomManager uomManager,
            ICurrencyRepository currencyRepository,
            CurrencyManager currencyManager,
            ICustomerRepository customerRepository,
            CustomerManager customerManager,
            IServiceRepository serviceRepository,
            ServiceManager serviceManager,
            IProjectOrderRepository projectOrderRepository,
            IProjectOrderDetailRepository projectOrderDetailRepository,
            ProjectOrderManager projectOrderManager,
            ProjectOrderDetailManager projectOrderDetailManager,
            IServiceOrderRepository serviceOrderRepository,
            IServiceOrderDetailRepository serviceOrderDetailRepository,
            ServiceOrderManager serviceOrderManager,
            ServiceOrderDetailManager serviceOrderDetailManager,
            IGuidGenerator guidGenerator,
            INumberSequenceRepository numberSequenceRepository,
            NumberSequenceManager numberSequenceManager,
            IWarehouseRepository warehouseRepository,
            WarehouseManager warehouseManager,
            IProductRepository productRepository,
            ProductManager productManager,
            IPurchaseOrderRepository purchaseOrderRepository,
            PurchaseOrderManager purchaseOrderManager,
            IPurchaseReceiptRepository purchaseReceiptRepository,
            PurchaseReceiptManager purchaseReceiptManager,
            IVendorRepository vendorRepository,
            VendorManager vendorManager,
            IPurchaseOrderDetailRepository purchaseOrderDetailRepository,
            PurchaseOrderDetailManager purchaseOrderDetailManager,
            IPurchaseReceiptDetailRepository purchaseReceiptDetailRepository,
            PurchaseReceiptDetailManager purchaseReceiptDetailManager,
            ISalesOrderRepository salesOrderRepository,
            SalesOrderManager salesOrderManager,
            ISalesOrderDetailRepository salesOrderDetailRepository,
            SalesOrderDetailManager salesOrderDetailManager,
            ISalesDeliveryRepository salesDeliveryRepository,
            SalesDeliveryManager salesDeliveryManager,
            ITransferOrderRepository transferOrderRepository,
            TransferOrderManager transferOrderManager,
            ITransferOrderDetailRepository transferOrderDetailRepository,
            TransferOrderDetailManager transferOrderDetailManager,
            DeliveryOrderManager deliveryOrderManager,
            GoodsReceiptManager goodsReceiptManager,
            IDepartmentRepository departmentRepository,
            DepartmentManager departmentManager,
            IEmployeeRepository employeeRepository,
            EmployeeManager employeeManager,
            IExpenseTypeRepository expenseTypeRepository,
            ExpenseTypeManager expenseTypeManager,
            IResourceRepository resourceRepository,
            ResourceManager resourceManager,
            IActivityRepository activityRepository,
            ActivityManager activityManager,
            ILeadRatingRepository leadRatingRepository,
            LeadRatingManager leadRatingManager,
            ILeadSourceRepository leadSourceRepository,
            LeadSourceManager leadSourceManager,
            ICashAndBankRepository cashAndBankRepository,
            CashAndBankManager cashAndBankManager,
            CustomerInvoiceManager customerInvoiceManager,
            ICustomerInvoiceRepository customerInvoiceRepository,
            CustomerPaymentManager customerPaymentManager,
            ICustomerPaymentRepository customerPaymentRepository,
            VendorBillManager vendorBillManager,
            IVendorBillRepository vendorBillRepository,
            VendorPaymentManager vendorPaymentManager,
            IVendorPaymentRepository vendorPaymentRepository,
            IServiceQuotationRepository serviceQuotationRepository,
            ServiceQuotationManager servcieQuotationManager,
            ISalesQuotationRepository salesQuotationRepository,
            SalesQuotationManager salesQuotationManager
            )
        {
            _companyRepository = companyRepository;
            _companyManager = companyManager;
            _uomRepository = uomRepository;
            _uomManager = uomManager;
            _currencyRepository = currencyRepository;
            _currencyManager = currencyManager;
            _customerRepository = customerRepository;
            _customerManager = customerManager;
            _serviceRepository = serviceRepository;
            _serviceManager = serviceManager;
            _projectOrderRepository = projectOrderRepository;
            _projectOrderDetailRepository = projectOrderDetailRepository;
            _serviceOrderManager = serviceOrderManager;
            _serviceOrderDetailManager = serviceOrderDetailManager;
            _serviceOrderRepository = serviceOrderRepository;
            _serviceOrderDetailRepository = serviceOrderDetailRepository;
            _guidGenerator = guidGenerator;
            _numberSequenceRepository = numberSequenceRepository;
            _numberSequenceManager = numberSequenceManager;
            _warehouseRepository = warehouseRepository;
            _warehouseManager = warehouseManager;
            _projectOrderManager = projectOrderManager;
            _projectOrderDetailManager = projectOrderDetailManager;
            _productRepository = productRepository;
            _productManager = productManager;
            _purchaseOrderRepository = purchaseOrderRepository;
            _purchaseOrderManager = purchaseOrderManager;
            _purchaseReceiptRepository = purchaseReceiptRepository;
            _purchaseReceiptManager = purchaseReceiptManager;
            _vendorRepository = vendorRepository;
            _vendorManager = vendorManager;
            _purchaseOrderDetailRepository = purchaseOrderDetailRepository;
            _purchaseOrderDetailManager = purchaseOrderDetailManager;
            _purchaseReceiptDetailRepository = purchaseReceiptDetailRepository;
            _purchaseReceiptDetailManager = purchaseReceiptDetailManager;
            _salesOrderRepository = salesOrderRepository;
            _salesOrderManager = salesOrderManager;
            _salesOrderDetailRepository = salesOrderDetailRepository;
            _salesOrderDetailManager = salesOrderDetailManager;
            _salesDeliveryRepository = salesDeliveryRepository;
            _salesDeliveryManager = salesDeliveryManager;
            _transferOrderRepository = transferOrderRepository;
            _transferOrderManager = transferOrderManager;
            _transferOrderDetailRepository = transferOrderDetailRepository;
            _transferOrderDetailManager = transferOrderDetailManager;
            _deliveryOrderManager = deliveryOrderManager;
            _goodsReceiptManager = goodsReceiptManager;
            _stockAdjustmentDetailManager = stockAdjustmentDetailManager;
            _stockAdjustmentDetailRepository = stockAdjustmentDetailRepository;
            _stockAdjustmentManager = stockAdjustmentManager;
            _stockAdjustmentRepository = stockAdjustmentRepository;
            _departmentRepository = departmentRepository;
            _departmentManager = departmentManager;
            _employeeRepository = employeeRepository;
            _employeeManager = employeeManager;
            _expenseTypeRepository = expenseTypeRepository;
            _expenseTypeManager = expenseTypeManager;
            _resourceRepository = resourceRepository;
            _resourceManager = resourceManager;
            _activityRepository = activityRepository;
            _activityManager = activityManager;
            _leadRatingRepository = leadRatingRepository;
            _leadRatingManager = leadRatingManager;
            _leadSourceRepository = leadSourceRepository;
            _leadSourceManager = leadSourceManager;
            _cashAndBankRepository = cashAndBankRepository;
            _cashAndBankManager = cashAndBankManager;
            _customerInvoiceManager = customerInvoiceManager;
            _customerInvoiceRepository = customerInvoiceRepository;
            _customerPaymentManager = customerPaymentManager;
            _customerPaymentRepository = customerPaymentRepository;
            _vendorBillManager = vendorBillManager;
            _vendorBillRepository = vendorBillRepository;
            _vendorPaymentManager = vendorPaymentManager;
            _vendorPaymentRepository = vendorPaymentRepository;
            _serviceQuotationRepository = serviceQuotationRepository;
            _serviceQuotationManager = servcieQuotationManager;
            _salesQuotationRepository = salesQuotationRepository;
            _salesQuotationManager = salesQuotationManager;
        }
        public async Task SeedAsync(DataSeedContext context)
        {
            try
            {

                if (await _companyRepository.GetCountAsync() > 0)
                {
                    return;
                }

                /*NumberSequence*/
                var numSeqServiceOrder = await _numberSequenceManager.CreateAsync("SVO");
                numSeqServiceOrder.Module = NumberSequenceModules.ServiceOrder;
                await _numberSequenceRepository.InsertAsync(numSeqServiceOrder, autoSave: true);

                var numSeqProjectOrder = await _numberSequenceManager.CreateAsync("PMO");
                numSeqProjectOrder.Module = NumberSequenceModules.ProjectOrder;
                await _numberSequenceRepository.InsertAsync(numSeqProjectOrder, autoSave: true);

                var numSeqPurchaseOrder = await _numberSequenceManager.CreateAsync("PO");
                numSeqPurchaseOrder.Module = NumberSequenceModules.PurchaseOrder;
                await _numberSequenceRepository.InsertAsync(numSeqPurchaseOrder, autoSave: true);

                var numSeqPurchaseReceipt = await _numberSequenceManager.CreateAsync("GR");
                numSeqPurchaseReceipt.Module = NumberSequenceModules.PurchaseReceipt;
                await _numberSequenceRepository.InsertAsync(numSeqPurchaseReceipt, autoSave: true);

                var numSeqSalesOrder = await _numberSequenceManager.CreateAsync("SO");
                numSeqSalesOrder.Module = NumberSequenceModules.SalesOrder;
                await _numberSequenceRepository.InsertAsync(numSeqSalesOrder, autoSave: true);

                var numSeqSalesDelivery = await _numberSequenceManager.CreateAsync("DO");
                numSeqSalesDelivery.Module = NumberSequenceModules.SalesDelivery;
                await _numberSequenceRepository.InsertAsync(numSeqSalesDelivery, autoSave: true);

                var numSeqTransferOrder = await _numberSequenceManager.CreateAsync("TO");
                numSeqTransferOrder.Module = NumberSequenceModules.TransferOrder;
                await _numberSequenceRepository.InsertAsync(numSeqTransferOrder, autoSave: true);

                var numSeqDeliveryOrder = await _numberSequenceManager.CreateAsync("WHDO");
                numSeqDeliveryOrder.Module = NumberSequenceModules.DeliveryOrder;
                await _numberSequenceRepository.InsertAsync(numSeqDeliveryOrder, autoSave: true);

                var numSeqGoodsReceipt = await _numberSequenceManager.CreateAsync("WHGR");
                numSeqGoodsReceipt.Module = NumberSequenceModules.GoodsReceipt;
                await _numberSequenceRepository.InsertAsync(numSeqGoodsReceipt, autoSave: true);

                var numSeqStockAdjustment = await _numberSequenceManager.CreateAsync("ADJ");
                numSeqStockAdjustment.Module = NumberSequenceModules.StockAdjustment;
                await _numberSequenceRepository.InsertAsync(numSeqStockAdjustment, autoSave: true);

                var numSeqMovement = await _numberSequenceManager.CreateAsync("MOV");
                numSeqMovement.Module = NumberSequenceModules.Movement;
                await _numberSequenceRepository.InsertAsync(numSeqMovement, autoSave: true);

                var numBill = await _numberSequenceManager.CreateAsync("BILL");
                numBill.Module = NumberSequenceModules.Bill;
                await _numberSequenceRepository.InsertAsync(numBill, autoSave: true);

                var numDebitNote = await _numberSequenceManager.CreateAsync("DNBILL");
                numDebitNote.Module = NumberSequenceModules.DebitNote;
                await _numberSequenceRepository.InsertAsync(numDebitNote, autoSave: true);

                var numInvoice = await _numberSequenceManager.CreateAsync("INVC");
                numInvoice.Module = NumberSequenceModules.Invoice;
                await _numberSequenceRepository.InsertAsync(numInvoice, autoSave: true);

                var numCreditNote = await _numberSequenceManager.CreateAsync("CNINVC");
                numCreditNote.Module = NumberSequenceModules.CreditNote;
                await _numberSequenceRepository.InsertAsync(numCreditNote, autoSave: true);

                var numExpense = await _numberSequenceManager.CreateAsync("EXPN");
                numExpense.Module = NumberSequenceModules.Expense;
                await _numberSequenceRepository.InsertAsync(numExpense, autoSave: true);

                var numCalendar = await _numberSequenceManager.CreateAsync("CALN");
                numCalendar.Module = NumberSequenceModules.Calendar;
                await _numberSequenceRepository.InsertAsync(numCalendar, autoSave: true);

                var numBooking = await _numberSequenceManager.CreateAsync("BOOK");
                numBooking.Module = NumberSequenceModules.Booking;
                await _numberSequenceRepository.InsertAsync(numBooking, autoSave: true);

                var numTodoList = await _numberSequenceManager.CreateAsync("TODO");
                numTodoList.Module = NumberSequenceModules.TodoList;
                await _numberSequenceRepository.InsertAsync(numTodoList, autoSave: true);

                var numBillPayment = await _numberSequenceManager.CreateAsync("VPAY");
                numBillPayment.Module = NumberSequenceModules.BillPayment;
                await _numberSequenceRepository.InsertAsync(numBillPayment, autoSave: true);

                var numInvoicePayment = await _numberSequenceManager.CreateAsync("CPAY");
                numInvoicePayment.Module = NumberSequenceModules.InvoicePayment;
                await _numberSequenceRepository.InsertAsync(numInvoicePayment, autoSave: true);

                var numCustomerRootFolder = await _numberSequenceManager.CreateAsync("DISK");
                numCustomerRootFolder.Module = NumberSequenceModules.CustomerRootFolder;
                await _numberSequenceRepository.InsertAsync(numCustomerRootFolder, autoSave: true);

                var numImportantDate = await _numberSequenceManager.CreateAsync("DATE");
                numImportantDate.Module = NumberSequenceModules.ImportantDate;
                await _numberSequenceRepository.InsertAsync(numImportantDate, autoSave: true);

                var numTask = await _numberSequenceManager.CreateAsync("TASK");
                numTask.Module = NumberSequenceModules.Task;
                await _numberSequenceRepository.InsertAsync(numTask, autoSave: true);

                var numServiceQuotation = await _numberSequenceManager.CreateAsync("SVCQ");
                numServiceQuotation.Module = NumberSequenceModules.ServiceQuotation;
                await _numberSequenceRepository.InsertAsync(numServiceQuotation, autoSave: true);

                var numSalesQuotation = await _numberSequenceManager.CreateAsync("SLSQ");
                numSalesQuotation.Module = NumberSequenceModules.SalesQuotation;
                await _numberSequenceRepository.InsertAsync(numSalesQuotation, autoSave: true);

                /*Warehouse*/
                var customerWH = await _warehouseManager.CreateAsync("Customer");
                customerWH.Virtual = true;
                customerWH.DefaultConfig = true;
                await _warehouseRepository.InsertAsync(customerWH, autoSave: true);

                var vendorWH = await _warehouseManager.CreateAsync("Vendor");
                vendorWH.Virtual = true;
                vendorWH.DefaultConfig = true;
                await _warehouseRepository.InsertAsync(vendorWH, autoSave: true);

                var intransitWH = await _warehouseManager.CreateAsync("InTransit");
                intransitWH.Virtual = true;
                intransitWH.DefaultConfig = true;
                await _warehouseRepository.InsertAsync(intransitWH, autoSave: true);

                var adjustmentWH = await _warehouseManager.CreateAsync("Adjustment");
                adjustmentWH.Virtual = true;
                adjustmentWH.DefaultConfig = true;
                await _warehouseRepository.InsertAsync(adjustmentWH, autoSave: true);

                var mainWH = await _warehouseManager.CreateAsync("Main");
                mainWH.Virtual = false;
                mainWH.DefaultConfig = true;
                await _warehouseRepository.InsertAsync(mainWH, autoSave: true);

                var branchWH = await _warehouseManager.CreateAsync("Branch");
                branchWH.Virtual = false;
                branchWH.DefaultConfig = false;
                await _warehouseRepository.InsertAsync(branchWH, autoSave: true);

                var storeWH = await _warehouseManager.CreateAsync("Store");
                storeWH.Virtual = false;
                storeWH.DefaultConfig = false;
                await _warehouseRepository.InsertAsync(storeWH, autoSave: true);


                /*Uom*/
                var uomUnit = await _uomRepository.InsertAsync(
                   await _uomManager.CreateAsync("Unit"), autoSave: true
                );

                var uomMandays = await _uomRepository.InsertAsync(
                   await _uomManager.CreateAsync("Mandays"), autoSave: true
                );

                var uomFTE = await _uomRepository.InsertAsync(
                   await _uomManager.CreateAsync("FTE"), autoSave: true
                );

                /*Currency*/
                var usd = await _currencyRepository.InsertAsync(
                   await _currencyManager.CreateAsync("USD", "USD"), autoSave: true
                );

                /*Department*/
                var deptIT = await _departmentRepository.InsertAsync(
                    await _departmentManager.CreateAsync("IT"), autoSave: true
                );

                var deptHR = await _departmentRepository.InsertAsync(
                    await _departmentManager.CreateAsync("HR"), autoSave: true
                );

                var deptFinance = await _departmentRepository.InsertAsync(
                    await _departmentManager.CreateAsync("Finance"), autoSave: true
                );

                var deptOperation = await _departmentRepository.InsertAsync(
                    await _departmentManager.CreateAsync("Operation"), autoSave: true
                );

                var deptSales = await _departmentRepository.InsertAsync(
                    await _departmentManager.CreateAsync("Sales"), autoSave: true
                );

                var deptScm = await _departmentRepository.InsertAsync(
                    await _departmentManager.CreateAsync("Supply Chain"), autoSave: true
                );

                var deptWarehouse = await _departmentRepository.InsertAsync(
                    await _departmentManager.CreateAsync("Warehouse"), autoSave: true
                );

                /*Expense Type*/
                var expGA = await _expenseTypeManager.CreateAsync("General and Administrative");
                await _expenseTypeRepository.InsertAsync(expGA, autoSave: true);

                var expSales = await _expenseTypeManager.CreateAsync("Marketing and Distribution");
                await _expenseTypeRepository.InsertAsync(expSales, autoSave: true);

                var expSalaries = await _expenseTypeManager.CreateAsync("Salaries and Benefits");
                await _expenseTypeRepository.InsertAsync(expSalaries, autoSave: true);

                var expUtilities = await _expenseTypeManager.CreateAsync("Cost of Utilities");
                await _expenseTypeRepository.InsertAsync(expUtilities, autoSave: true);

                var expOperation = await _expenseTypeManager.CreateAsync("Operations");
                await _expenseTypeRepository.InsertAsync(expOperation, autoSave: true);

                var expOthers = await _expenseTypeManager.CreateAsync("Others");
                await _expenseTypeRepository.InsertAsync(expOthers, autoSave: true);

                /*Resource*/
                var resMeetingRoom1 = await _resourceManager.CreateAsync("Meeting Room#1");
                await _resourceRepository.InsertAsync(resMeetingRoom1, autoSave: true);

                var resMeetingRoom2 = await _resourceManager.CreateAsync("Meeting Room#2");
                await _resourceRepository.InsertAsync(resMeetingRoom2, autoSave: true);

                var resMeetingRoom3 = await _resourceManager.CreateAsync("Meeting Room#3");
                await _resourceRepository.InsertAsync(resMeetingRoom3, autoSave: true);

                var resCar1 = await _resourceManager.CreateAsync("Car#1");
                await _resourceRepository.InsertAsync(resCar1, autoSave: true);

                var resCar2 = await _resourceManager.CreateAsync("Car#2");
                await _resourceRepository.InsertAsync(resCar2, autoSave: true);

                var resCar3 = await _resourceManager.CreateAsync("Car#3");
                await _resourceRepository.InsertAsync(resCar3, autoSave: true);

                /* Activity */

                var actPhone = await _activityManager.CreateAsync("Phone");
                await _activityRepository.InsertAsync(actPhone, autoSave: true);

                var actEmail = await _activityManager.CreateAsync("Email");
                await _activityRepository.InsertAsync(actEmail, autoSave: true);

                var actMeeting = await _activityManager.CreateAsync("Meeting");
                await _activityRepository.InsertAsync(actMeeting, autoSave: true);

                var actDemo = await _activityManager.CreateAsync("Demo");
                await _activityRepository.InsertAsync(actDemo, autoSave: true);

                var actClosing = await _activityManager.CreateAsync("Closing");
                await _activityRepository.InsertAsync(actClosing, autoSave: true);

                /* Lead Rating */

                var lrHot = await _leadRatingManager.CreateAsync("Hot");
                await _leadRatingRepository.InsertAsync(lrHot, autoSave: true);

                var lrWarm = await _leadRatingManager.CreateAsync("Warm");
                await _leadRatingRepository.InsertAsync(lrWarm, autoSave: true);

                var lrCold = await _leadRatingManager.CreateAsync("Cold");
                await _leadRatingRepository.InsertAsync(lrCold, autoSave: true);

                /* Lead Source */

                var lsGoogle = await _leadSourceManager.CreateAsync("Google");
                await _leadSourceRepository.InsertAsync(lsGoogle, autoSave: true);

                var lsFacebook = await _leadSourceManager.CreateAsync("Facebook");
                await _leadSourceRepository.InsertAsync(lsFacebook, autoSave: true);

                var lsLinkedIn = await _leadSourceManager.CreateAsync("LinkedIn");
                await _leadSourceRepository.InsertAsync(lsLinkedIn, autoSave: true);

                var lsWebsite = await _leadSourceManager.CreateAsync("Website");
                await _leadSourceRepository.InsertAsync(lsWebsite, autoSave: true);

                var lsFriendFamily = await _leadSourceManager.CreateAsync("Friend & Family");
                await _leadSourceRepository.InsertAsync(lsFriendFamily, autoSave: true);

                var lsYellowPages = await _leadSourceManager.CreateAsync("Yellow Pages");
                await _leadSourceRepository.InsertAsync(lsYellowPages, autoSave: true);

                var lsGenerationTools = await _leadSourceManager.CreateAsync("Generation Tools");
                await _leadSourceRepository.InsertAsync(lsGenerationTools, autoSave: true);

                /*CashAndBank*/
                var cbDefaulPettyCash = await _cashAndBankRepository.InsertAsync(
                    await _cashAndBankManager.CreateAsync("Default Petty Cash"), autoSave: true
                );

                var cbDefaultBank = await _cashAndBankRepository.InsertAsync(
                    await _cashAndBankManager.CreateAsync("Default Bank"), autoSave: true
                );

                var cbDefaultEWallet = await _cashAndBankRepository.InsertAsync(
                    await _cashAndBankManager.CreateAsync("Default E-Wallet"), autoSave: true
                );

                /*Company*/
                var dreamCompany = await _companyManager.CreateAsync("Dream Team Company Inc", usd.Id, mainWH.Id);
                dreamCompany.Phone = "6221-444-5555";
                dreamCompany.Email = "info@dreamcompany.com";
                dreamCompany.Street = "JL. Sudirman";
                dreamCompany.City = "Jakarta Selatan";
                dreamCompany.State = "DKI";
                dreamCompany.ZipCode = "12190";
                await _companyRepository.InsertAsync(dreamCompany, autoSave: true);




                if (Demo.DemoConsts.IsEnabled)
                {

                    /*Buyer*/
                    var empNick = await _employeeManager.CreateAsync("Nick Swinmurn", "EMP#0003");
                    empNick.EmployeeGroup = EmployeeGroup.Buyer;
                    empNick.DepartmentId = deptScm.Id;
                    empNick.Street = "Home Sweet Home 55";
                    empNick.City = "Redmond";
                    empNick.State = "Washington";
                    empNick.ZipCode = "98052";
                    empNick.Phone = "334889800";
                    empNick.Email = "nick@employee.com";
                    await _employeeRepository.InsertAsync(empNick, autoSave: true);

                    var empTony = await _employeeManager.CreateAsync("Tony Hsieh", "EMP#0004");
                    empTony.EmployeeGroup = EmployeeGroup.Buyer;
                    empTony.DepartmentId = deptScm.Id;
                    empTony.Street = "Home Sweet Home 55";
                    empTony.City = "Redmond";
                    empTony.State = "Washington";
                    empTony.ZipCode = "98052";
                    empTony.Phone = "334889800";
                    empTony.Email = "tony@employee.com";
                    await _employeeRepository.InsertAsync(empTony, autoSave: true);

                    var empMike = await _employeeManager.CreateAsync("Mike Brookes", "EMP#0005");
                    empMike.EmployeeGroup = EmployeeGroup.Buyer;
                    empMike.DepartmentId = deptScm.Id;
                    empMike.Street = "Home Sweet Home 55";
                    empMike.City = "Redmond";
                    empMike.State = "Washington";
                    empMike.ZipCode = "98052";
                    empMike.Phone = "334889800";
                    empMike.Email = "mike@employee.com";
                    await _employeeRepository.InsertAsync(empMike, autoSave: true);


                    /*Vendor*/
                    var samsung = await _vendorManager.CreateAsync("Samsung International");
                    samsung.Street = "One Samsung Way";
                    samsung.City = "Redmond";
                    samsung.State = "Washington";
                    samsung.ZipCode = "98052";
                    samsung.Phone = "3343478756";
                    samsung.Email = "info@samsung.com";
                    await _vendorRepository.InsertAsync(samsung, autoSave: true);

                    var lenovo = await _vendorManager.CreateAsync("Lenovo International");
                    lenovo.Street = "2300 Lenovo Way";
                    lenovo.City = "Austin";
                    lenovo.State = "Texas";
                    lenovo.ZipCode = "78741";
                    lenovo.Phone = "4343488890";
                    lenovo.Email = "info@lenovo.com";
                    await _vendorRepository.InsertAsync(lenovo, autoSave: true);

                    var dell = await _vendorManager.CreateAsync("Dell International");
                    dell.Street = "666 Terry Ave. North";
                    dell.City = "Seattle";
                    dell.State = "Washington";
                    dell.ZipCode = "98109";
                    dell.Phone = "65656889990";
                    dell.Email = "info@dell.com";
                    await _vendorRepository.InsertAsync(dell, autoSave: true);

                    var asus = await _vendorManager.CreateAsync("Asus International");
                    asus.Street = "5600 Amphitheatre Parkway";
                    asus.City = "Mountain View";
                    asus.State = "California";
                    asus.ZipCode = "98109";
                    asus.Phone = "7867567587";
                    asus.Email = "info@asus.com";
                    await _vendorRepository.InsertAsync(asus, autoSave: true);

                    var hp = await _vendorManager.CreateAsync("Hawlett-Packard International");
                    hp.Street = " One HP Park Way";
                    hp.City = "Cupertino";
                    hp.State = "California";
                    hp.ZipCode = "95014";
                    hp.Phone = "4676769990";
                    hp.Email = "info@hp.com";
                    await _vendorRepository.InsertAsync(hp, autoSave: true);

                    /*SalesExecutive*/

                    var empJeff = await _employeeManager.CreateAsync("Jeff Bezos", "EMP#0006");
                    empJeff.EmployeeGroup = EmployeeGroup.Sales;
                    empJeff.DepartmentId = deptScm.Id;
                    empJeff.Street = "Home Sweet Home 55";
                    empJeff.City = "Redmond";
                    empJeff.State = "Washington";
                    empJeff.ZipCode = "98052";
                    empJeff.Phone = "334889800";
                    empJeff.Email = "jeff@employee.com";
                    await _employeeRepository.InsertAsync(empJeff, autoSave: true);


                    /*Customer*/
                    var microsoft = await _customerManager.CreateAsync("Microsoft Inc");
                    microsoft.Street = "One Microsoft Way";
                    microsoft.City = "Redmond";
                    microsoft.State = "Washington";
                    microsoft.ZipCode = "98052";
                    microsoft.Phone = "123666789";
                    microsoft.Email = "info@microsoft.com";
                    microsoft.Status = CustomerStatus.Customer;
                    microsoft.Stage = CustomerStage.SalesQualified;
                    microsoft.RootFolder = await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.CustomerRootFolder);
                    await _customerRepository.InsertAsync(microsoft, autoSave: true);

                    var oracle = await _customerManager.CreateAsync("Oracle Inc");
                    oracle.Street = "2300 Oracle Way";
                    oracle.City = "Austin";
                    oracle.State = "Texas";
                    oracle.ZipCode = "78741";
                    oracle.Phone = "4353463445";
                    oracle.Email = "info@oracle.com";
                    oracle.Status = CustomerStatus.Customer;
                    oracle.Stage = CustomerStage.SalesQualified;
                    oracle.RootFolder = await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.CustomerRootFolder);
                    await _customerRepository.InsertAsync(oracle, autoSave: true);

                    var amazon = await _customerManager.CreateAsync("Amazon Inc");
                    amazon.Street = "410 Terry Ave. North";
                    amazon.City = "Seattle";
                    amazon.State = "Washington";
                    amazon.ZipCode = "98109";
                    amazon.Phone = "3453454356";
                    amazon.Email = "info@amazon.com";
                    amazon.Status = CustomerStatus.Customer;
                    amazon.Stage = CustomerStage.SalesQualified;
                    amazon.RootFolder = await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.CustomerRootFolder);
                    await _customerRepository.InsertAsync(amazon, autoSave: true);

                    var google = await _customerManager.CreateAsync("Google Inc");
                    google.Street = "1600 Amphitheatre Parkway";
                    google.City = "Mountain View";
                    google.State = "California";
                    google.ZipCode = "98109";
                    google.Phone = "7867567587";
                    google.Email = "info@google.com";
                    google.Status = CustomerStatus.Customer;
                    google.Stage = CustomerStage.SalesQualified;
                    google.RootFolder = await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.CustomerRootFolder);
                    await _customerRepository.InsertAsync(google, autoSave: true);

                    var apple = await _customerManager.CreateAsync("Apple Inc");
                    apple.Street = " One Apple Park Way";
                    apple.City = "Cupertino";
                    apple.State = "California";
                    apple.ZipCode = "95014";
                    apple.Phone = "2347866567";
                    apple.Email = "info@apple.com";
                    apple.Status = CustomerStatus.Customer;
                    apple.Stage = CustomerStage.SalesQualified;
                    apple.RootFolder = await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.CustomerRootFolder);
                    await _customerRepository.InsertAsync(apple, autoSave: true);

                    /*Service*/
                    var sd = await _serviceManager.CreateAsync("Software Developer", uomFTE.Id);
                    sd.Price = 500;
                    sd.TaxRate = 10;
                    await _serviceRepository.InsertAsync(sd, autoSave: true);

                    var pm = await _serviceManager.CreateAsync("Project Manager", uomFTE.Id);
                    pm.Price = 700;
                    pm.TaxRate = 10;
                    await _serviceRepository.InsertAsync(pm, autoSave: true);

                    var db = await _serviceManager.CreateAsync("Database Administrator", uomFTE.Id);
                    db.Price = 600;
                    db.TaxRate = 10;
                    await _serviceRepository.InsertAsync(db, autoSave: true);

                    /*Product*/
                    var laptop = await _productManager.CreateAsync("Laptop", uomUnit.Id);
                    laptop.Price = 2000;
                    laptop.RetailPrice = (30 / 100f * laptop.Price) + laptop.Price;
                    laptop.TaxRate = 10;
                    await _productRepository.InsertAsync(laptop, autoSave: true);

                    var monitor = await _productManager.CreateAsync("Monitor", uomUnit.Id);
                    monitor.Price = 1000;
                    monitor.RetailPrice = (30 / 100f * monitor.Price) + monitor.Price;
                    monitor.TaxRate = 10;
                    await _productRepository.InsertAsync(monitor, autoSave: true);

                    var speaker = await _productManager.CreateAsync("Speaker", uomUnit.Id);
                    speaker.Price = 500;
                    speaker.RetailPrice = (30 / 100f * speaker.Price) + speaker.Price;
                    speaker.TaxRate = 10;
                    await _productRepository.InsertAsync(speaker, autoSave: true);

                    var keyobard = await _productManager.CreateAsync("Keyboard", uomUnit.Id);
                    keyobard.Price = 400;
                    keyobard.RetailPrice = (30 / 100f * keyobard.Price) + keyobard.Price;
                    keyobard.TaxRate = 10;
                    await _productRepository.InsertAsync(keyobard, autoSave: true);

                    var mouse = await _productManager.CreateAsync("Mouse", uomUnit.Id);
                    mouse.Price = 200;
                    mouse.RetailPrice = (30 / 100f * mouse.Price) + mouse.Price;
                    mouse.TaxRate = 10;
                    await _productRepository.InsertAsync(mouse, autoSave: true);

                    var headphone = await _productManager.CreateAsync("Headphone", uomUnit.Id);
                    headphone.Price = 600;
                    headphone.RetailPrice = (30 / 100f * headphone.Price) + headphone.Price;
                    headphone.TaxRate = 10;
                    await _productRepository.InsertAsync(headphone, autoSave: true);

                    var mobilephone = await _productManager.CreateAsync("Mobile Phone", uomUnit.Id);
                    mobilephone.Price = 800;
                    mobilephone.RetailPrice = (30 / 100f * mobilephone.Price) + mobilephone.Price;
                    mobilephone.TaxRate = 10;
                    await _productRepository.InsertAsync(mobilephone, autoSave: true);

                    var tablet = await _productManager.CreateAsync("Tablet", uomUnit.Id);
                    tablet.Price = 1100;
                    tablet.RetailPrice = (30 / 100f * tablet.Price) + tablet.Price;
                    tablet.TaxRate = 10;
                    await _productRepository.InsertAsync(tablet, autoSave: true);

                    /*Dummy Transaction*/
                    var rnd = new Random();
                    var endDate = DateTime.Now;
                    var startDate = new DateTime(endDate.Year, endDate.AddMonths(-6).Month, 1);
                    var employees = await _employeeRepository.GetListAsync();
                    var customers = await _customerRepository.GetListAsync();
                    var executives = new List<Employee>();
                    foreach (var item in employees)
                    {
                        if (item.EmployeeGroup == EmployeeGroup.Sales)
                        {
                            executives.Add(item);
                        }
                    }
                    var vendors = await _vendorRepository.GetListAsync();
                    var buyers = new List<Employee>();
                    foreach (var item in employees)
                    {
                        if (item.EmployeeGroup == EmployeeGroup.Buyer)
                        {
                            buyers.Add(item);
                        }
                    }
                    var services = await _serviceRepository.GetListAsync();
                    var products = await _productRepository.GetListAsync();
                    var warehouses = new List<Warehouse>
                {
                    branchWH,
                    storeWH
                };
                    var tasks = new List<string>()
                {
                    "Business Case",
                    "System Analysis",
                    "UI/UX Design",
                    "Quality Assurance",
                    "Support",
                    "Infrastructure"
                };
                    var quantities = new List<float>()
                {
                    1,
                    2,
                    3,
                    4,
                    5,
                    6,
                    7,
                    8,
                    9,
                    10
                };
                    var purchaseQuantities = new List<float>()
                {
                    10,
                    20,
                    30,
                    40,
                    50
                };
                    var transferQuantities = new List<float>()
                {
                    2,
                    5,
                    10
                };
                    var prices = new List<float>()
                {
                    1200,
                    2700,
                    3100,
                    4500,
                    4900,
                    5300,
                    5900,
                    6000,
                    6200,
                    7000,
                    8000
                };
                    var discs = new List<float>()
                {
                    0,
                    100,
                    200,
                    300
                };
                    var days = new List<int>();
                    for (int i = 0; i < 5; i++)
                    {
                        days.Add(rnd.Next(1, 25));
                    }

                    /*ServiceOrder*/
                    int number = 0;
                    foreach (var item in EachDay(startDate, endDate))
                    {
                        if (days.Contains(item.Day))
                        {
                            number++;
                            var order = await _serviceOrderManager.CreateAsync(
                                    await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.ServiceOrder),
                                    customers[rnd.Next(0, customers.Count)].Id,
                                    executives[rnd.Next(0, executives.Count)].Id,
                                    item
                                );
                            order.Status = (ServiceOrderStatus)rnd.Next(1, 4);
                            if (order.Status == ServiceOrderStatus.Cancelled && item.Day <= 20)
                            {
                                order.Status = ServiceOrderStatus.Confirm;
                            }
                            var result = await _serviceOrderRepository.InsertAsync(order, autoSave: true);
                            var details = new List<ServiceOrderDetail>();
                            for (int i = 0; i < 3; i++)
                            {
                                var service = services[rnd.Next(0, services.Count)];
                                var detail = await _serviceOrderDetailManager.CreateAsync(
                                        result.Id,
                                        service.Id,
                                        quantities[rnd.Next(0, quantities.Count)],
                                        discs[rnd.Next(0, discs.Count)]
                                    );
                                details.Add(detail);
                            }
                            await _serviceOrderDetailRepository.InsertManyAsync(details, autoSave: true);

                            if (order.Status == ServiceOrderStatus.Confirm)
                            {
                                var invoice = await _serviceOrderManager.GenerateInvoice(order.Id);
                                invoice.InvoiceDate = order.OrderDate;
                                invoice.InvoiceDueDate = invoice.InvoiceDate.AddDays(14);
                                invoice.Status = CustomerInvoiceStatus.Confirm;
                                await _customerInvoiceRepository.UpdateAsync(invoice, true);

                                var payment = await _customerInvoiceManager.GeneratePayment(invoice.Id);
                                payment.PaymentDate = invoice.InvoiceDate;
                                payment.Status = CustomerPaymentStatus.Confirm;
                                await _customerPaymentRepository.UpdateAsync(payment, true);
                            }


                        }
                    }


                    /*ProjectOrder*/
                    number = 0;
                    foreach (var item in EachDay(startDate, endDate))
                    {
                        if (days.Contains(item.Day))
                        {
                            number++;
                            var order = await _projectOrderManager.CreateAsync(
                                    await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.ProjectOrder),
                                    customers[rnd.Next(0, customers.Count)].Id,
                                    executives[rnd.Next(0, executives.Count)].Id,
                                    item
                                );
                            order.Status = (ProjectOrderStatus)rnd.Next(1, 4);
                            if (order.Status == ProjectOrderStatus.Cancelled && item.Day <= 20)
                            {
                                order.Status = ProjectOrderStatus.Confirm;
                            }
                            order.Rating = (ProjectOrderRating)rnd.Next(1, 5);
                            var result = await _projectOrderRepository.InsertAsync(order, autoSave: true);
                            var details = new List<ProjectOrderDetail>();
                            for (int i = 0; i < 3; i++)
                            {
                                var detail = await _projectOrderDetailManager.CreateAsync(
                                        result.Id,
                                        tasks[rnd.Next(0, tasks.Count)],
                                        quantities[rnd.Next(0, quantities.Count)],
                                        prices[rnd.Next(0, prices.Count)]
                                    );
                                details.Add(detail);
                            }
                            await _projectOrderDetailRepository.InsertManyAsync(details, autoSave: true);

                            if (order.Status == ProjectOrderStatus.Confirm)
                            {
                                var invoice = await _projectOrderManager.GenerateInvoice(order.Id);
                                invoice.InvoiceDate = order.OrderDate;
                                invoice.InvoiceDueDate = invoice.InvoiceDate.AddDays(14);
                                invoice.Status = CustomerInvoiceStatus.Confirm;
                                await _customerInvoiceRepository.UpdateAsync(invoice, true);

                                var payment = await _customerInvoiceManager.GeneratePayment(invoice.Id);
                                payment.PaymentDate = invoice.InvoiceDate;
                                payment.Status = CustomerPaymentStatus.Confirm;
                                await _customerPaymentRepository.UpdateAsync(payment, true);
                            }

                        }
                    }

                    /*PurchaseOrder*/
                    number = 0;
                    foreach (var item in EachDay(startDate, endDate))
                    {
                        if (days.Contains(item.Day))
                        {
                            number++;
                            var order = await _purchaseOrderManager.CreateAsync(
                                    await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.PurchaseOrder),
                                    vendors[rnd.Next(0, vendors.Count)].Id,
                                    buyers[rnd.Next(0, buyers.Count)].Id,
                                    item
                                );

                            var result = await _purchaseOrderRepository.InsertAsync(order, autoSave: true);
                            var details = new List<PurchaseOrderDetail>();
                            for (int i = 0; i < products.Count; i++)
                            {
                                var product = products[i];
                                var detail = await _purchaseOrderDetailManager.CreateAsync(
                                        result.Id,
                                        product.Id,
                                        purchaseQuantities[rnd.Next(0, purchaseQuantities.Count)],
                                        discs[rnd.Next(0, discs.Count)]
                                    );
                                details.Add(detail);
                            }
                            await _purchaseOrderDetailRepository.InsertManyAsync(details, autoSave: true);

                            var nextStatus = (PurchaseOrderStatus)rnd.Next(1, 4);
                            if (order.Status == PurchaseOrderStatus.Cancelled && item.Day <= 20)
                            {
                                order.Status = PurchaseOrderStatus.Confirm;
                            }
                            if (nextStatus == PurchaseOrderStatus.Confirm)
                            {
                                order.Status = nextStatus;
                                await _purchaseOrderRepository.UpdateAsync(order, autoSave: true);
                                var receipt = await _purchaseReceiptManager.GeneratePurchaseReceiptFromPurchaseAsync(result.Id);
                                await _purchaseReceiptManager.ConfirmPurchaseReceiptAsync(receipt.Id);


                            }
                            if (nextStatus == PurchaseOrderStatus.Cancelled)
                            {
                                order.Status = nextStatus;
                                await _purchaseOrderRepository.UpdateAsync(order, autoSave: true);
                                var receipt = await _purchaseReceiptManager.GeneratePurchaseReceiptFromPurchaseAsync(result.Id);
                                await _purchaseReceiptManager.ConfirmPurchaseReceiptAsync(receipt.Id);
                                var returned = await _purchaseReceiptManager.GeneratePurchaseReceiptReturnFromReceiptAsync(receipt.Id);
                                await _purchaseReceiptManager.ConfirmPurchaseReceiptReturnAsync(returned.Id);

                            }

                            if (order.Status == PurchaseOrderStatus.Confirm)
                            {
                                var bill = await _purchaseOrderManager.GenerateBill(order.Id);
                                bill.BillDate = order.OrderDate;
                                bill.BillDueDate = bill.BillDate.AddDays(14);
                                bill.Status = VendorBillStatus.Confirm;
                                await _vendorBillRepository.UpdateAsync(bill, true);

                                var payment = await _vendorBillManager.GeneratePayment(bill.Id);
                                payment.PaymentDate = bill.BillDate;
                                payment.Status = VendorPaymentStatus.Confirm;
                                await _vendorPaymentRepository.UpdateAsync(payment, true);
                            }
                        }
                    }

                    /*SalesOrder*/
                    number = 0;
                    foreach (var item in EachDay(startDate, endDate))
                    {
                        if (days.Contains(item.Day))
                        {
                            number++;
                            var order = await _salesOrderManager.CreateAsync(
                                    await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.SalesOrder),
                                    customers[rnd.Next(0, customers.Count)].Id,
                                    executives[rnd.Next(0, executives.Count)].Id,
                                    item
                                );

                            var result = await _salesOrderRepository.InsertAsync(order, autoSave: true);
                            var details = new List<SalesOrderDetail>();
                            for (int i = 0; i < 3; i++)
                            {
                                var product = products[rnd.Next(0, products.Count)];
                                var detail = await _salesOrderDetailManager.CreateAsync(
                                        result.Id,
                                        product.Id,
                                        quantities[rnd.Next(0, quantities.Count)],
                                        discs[rnd.Next(0, discs.Count)]
                                    );
                                details.Add(detail);
                            }
                            await _salesOrderDetailRepository.InsertManyAsync(details, autoSave: true);

                            var nextStatus = (SalesOrderStatus)rnd.Next(1, 4);
                            if (order.Status == SalesOrderStatus.Cancelled && item.Day <= 20)
                            {
                                order.Status = SalesOrderStatus.Confirm;
                            }
                            if (nextStatus == SalesOrderStatus.Confirm)
                            {
                                order.Status = nextStatus;
                                await _salesOrderRepository.UpdateAsync(order, autoSave: true);
                                var delivery = await _salesDeliveryManager.GenerateSalesDeliveryFromSalesAsync(result.Id);
                                await _salesDeliveryManager.ConfirmSalesDeliveryAsync(delivery.Id);


                            }
                            if (nextStatus == SalesOrderStatus.Cancelled)
                            {
                                order.Status = nextStatus;
                                await _salesOrderRepository.UpdateAsync(order, autoSave: true);
                                var delivery = await _salesDeliveryManager.GenerateSalesDeliveryFromSalesAsync(result.Id);
                                await _salesDeliveryManager.ConfirmSalesDeliveryAsync(delivery.Id);
                                var returned = await _salesDeliveryManager.GenerateSalesDeliveryReturnFromDeliveryAsync(delivery.Id);
                                await _salesDeliveryManager.ConfirmSalesDeliveryReturnAsync(returned.Id);

                            }
                            if (order.Status == SalesOrderStatus.Confirm)
                            {
                                var invoice = await _salesOrderManager.GenerateInvoice(order.Id);
                                invoice.InvoiceDate = order.OrderDate;
                                invoice.InvoiceDueDate = invoice.InvoiceDate.AddDays(14);
                                invoice.Status = CustomerInvoiceStatus.Confirm;
                                await _customerInvoiceRepository.UpdateAsync(invoice, true);

                                var payment = await _customerInvoiceManager.GeneratePayment(invoice.Id);
                                payment.PaymentDate = invoice.InvoiceDate;
                                payment.Status = CustomerPaymentStatus.Confirm;
                                await _customerPaymentRepository.UpdateAsync(payment, true);
                            }
                        }
                    }

                    /*TransferOrder*/
                    number = 0;
                    foreach (var item in EachDay(startDate, endDate))
                    {
                        if (days.Contains(item.Day))
                        {
                            number++;
                            var order = await _transferOrderManager.CreateAsync(
                                    await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.TransferOrder),
                                    mainWH.Id,
                                    warehouses[rnd.Next(0, warehouses.Count)].Id,
                                    item
                                );

                            var result = await _transferOrderRepository.InsertAsync(order, autoSave: true);
                            var details = new List<TransferOrderDetail>();
                            for (int i = 0; i < 3; i++)
                            {
                                var product = products[rnd.Next(0, products.Count)];
                                var detail = await _transferOrderDetailManager.CreateAsync(
                                        result.Id,
                                        product.Id,
                                        transferQuantities[rnd.Next(0, transferQuantities.Count)]
                                    );
                                details.Add(detail);
                            }
                            await _transferOrderDetailRepository.InsertManyAsync(details, autoSave: true);

                            var nextStatus = (TransferOrderStatus)rnd.Next(1, 4);
                            if (nextStatus == TransferOrderStatus.Confirm)
                            {
                                order.Status = nextStatus;
                                await _transferOrderRepository.UpdateAsync(order, autoSave: true);

                                var delivery = await _deliveryOrderManager.GenerateDeliveryOrderFromTransferAsync(result.Id);
                                await _deliveryOrderManager.ConfirmDeliveryOrderAsync(delivery.Id);

                                var receipt = await _goodsReceiptManager.GenerateGoodsReceiptFromDeliveryAsync(delivery.Id);
                                await _goodsReceiptManager.ConfirmGoodsReceiptAsync(receipt.Id);

                            }
                            if (nextStatus == TransferOrderStatus.Returned)
                            {
                                var origin = await _transferOrderRepository.GetAsync(order.Id);
                                origin.Status = TransferOrderStatus.Confirm;
                                await _transferOrderRepository.UpdateAsync(origin, autoSave: true);

                                var delivery = await _deliveryOrderManager.GenerateDeliveryOrderFromTransferAsync(origin.Id);
                                await _deliveryOrderManager.ConfirmDeliveryOrderAsync(delivery.Id);

                                var receipt = await _goodsReceiptManager.GenerateGoodsReceiptFromDeliveryAsync(delivery.Id);
                                await _goodsReceiptManager.ConfirmGoodsReceiptAsync(receipt.Id);

                                var returned = await _transferOrderManager.GenerateReturn(origin.Id);
                                returned.Status = TransferOrderStatus.Confirm;
                                await _transferOrderRepository.UpdateAsync(returned, autoSave: true);

                                var deliveryReturned = await _deliveryOrderManager.GenerateDeliveryOrderFromTransferAsync(returned.Id);
                                await _deliveryOrderManager.ConfirmDeliveryOrderAsync(deliveryReturned.Id);

                                var receiptReturned = await _goodsReceiptManager.GenerateGoodsReceiptFromDeliveryAsync(deliveryReturned.Id);
                                await _goodsReceiptManager.ConfirmGoodsReceiptAsync(receiptReturned.Id);

                                origin.ReturnId = returned.Id;
                                origin.Status = nextStatus;
                                await _transferOrderRepository.UpdateAsync(origin, autoSave: true);

                            }
                        }
                    }

                    /*StockAdjustment*/
                    number = 0;
                    foreach (var item in EachDay(startDate, endDate))
                    {
                        if (days.Contains(item.Day))
                        {
                            number++;
                            var adj = await _stockAdjustmentManager.CreateAsync(
                                    await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.StockAdjustment),
                                    warehouses[rnd.Next(0, warehouses.Count)].Id,
                                    (StockAdjustmentType)rnd.Next(1, 3),
                                    item
                                );

                            var result = await _stockAdjustmentRepository.InsertAsync(adj, autoSave: true);
                            var details = new List<StockAdjustmentDetail>();
                            for (int i = 0; i < 3; i++)
                            {
                                var product = products[rnd.Next(0, products.Count)];
                                var detail = await _stockAdjustmentDetailManager.CreateAsync(
                                        result.Id,
                                        product.Id,
                                        quantities[rnd.Next(0, quantities.Count)]
                                    );
                                details.Add(detail);
                            }
                            await _stockAdjustmentDetailRepository.InsertManyAsync(details, autoSave: true);

                            var nextStatus = (StockAdjustmentStatus)rnd.Next(1, 4);
                            if (nextStatus == StockAdjustmentStatus.Confirm)
                            {
                                await _stockAdjustmentManager.ConfirmStockAdjustmentAsync(result.Id);
                            }
                            if (nextStatus == StockAdjustmentStatus.Returned)
                            {
                                var origin = await _stockAdjustmentRepository.GetAsync(result.Id);
                                await _stockAdjustmentManager.ConfirmStockAdjustmentAsync(origin.Id);
                                var returned = await _stockAdjustmentManager.GenerateReturn(origin.Id);
                                await _stockAdjustmentManager.ConfirmStockAdjustmentAsync(returned.Id);
                                origin.ReturnId = returned.Id;
                                origin.Status = nextStatus;
                                await _stockAdjustmentRepository.UpdateAsync(origin, autoSave: true);
                            }

                        }
                    }



                    /*Lead*/
                    string[] names = { "Olive Yew", "Aida Bugg", "Teri Dactyl", "Allie Grater", "Constance Noring", "Isabelle Ringing", "Eileen Sideways", "Rita Book", "Paige Turner", "Rhoda Report", "Chris Anthemum", "Anita Bath", "Harriet Upp", "Ben Dover" };
                    var ratings = await _leadRatingRepository.ToListAsync();
                    var sources = await _leadSourceRepository.ToListAsync();
                    var leads = new List<Customer>();

                    foreach (var item in names)
                    {
                        var lead = await _customerManager.CreateAsync(item);
                        lead.LeadRatingId = ratings[rnd.Next(0, ratings.Count)].Id;
                        lead.LeadSourceId = sources[rnd.Next(0, sources.Count)].Id;
                        lead.Status = CustomerStatus.Lead;
                        lead.Stage = (CustomerStage)rnd.Next(1, 4);
                        lead.Street = $"One {item} Street";
                        lead.Phone = "9118888234";
                        lead.Email = $"{item.Replace(" ", ".")}@gmail.com";
                        lead.RootFolder = await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.CustomerRootFolder);
                        leads.Add(lead);

                    }

                    await _customerRepository.InsertManyAsync(leads, true);

                    foreach (var item in leads)
                    {
                        var serviceQuotation = await _serviceQuotationManager.CreateAsync(
                                await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.ServiceQuotation),
                                item.Id,
                                executives[rnd.Next(0, executives.Count)].Id,
                                DateTime.Now,
                                DateTime.Now.AddDays(14)
                            );
                        serviceQuotation.Pipeline = (ServiceQuotationPipeline)rnd.Next(1, 6);
                        await _serviceQuotationRepository.InsertAsync(serviceQuotation, true);

                        var salesQuotation = await _salesQuotationManager.CreateAsync(
                                await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.SalesQuotation),
                                item.Id,
                                executives[rnd.Next(0, executives.Count)].Id,
                                DateTime.Now,
                                DateTime.Now.AddDays(14)
                            );
                        salesQuotation.Pipeline = (SalesQuotationPipeline)rnd.Next(1, 6);
                        await _salesQuotationRepository.InsertAsync(salesQuotation, true);
                    }



                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private  IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

    }
}
