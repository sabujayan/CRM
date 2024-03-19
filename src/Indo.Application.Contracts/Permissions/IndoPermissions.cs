namespace Indo.Permissions
{
    public static class IndoPermissions
    {
        public const string GroupName = "AppCrm";

        public static class Dashboard
        {
            public const string Default = GroupName + ".Dashboard";
            public const string Crm = Default + ".Crm";
            public const string Inventory = Default + ".Inventory";
        }
        public static class CustomerManagement
        {
            public const string Default = GroupName + ".CustomerManagement";
            public const string ActivityMaster = Default + ".ActivityMaster";
            public const string LeadSourceMaster = Default + ".LeadSourceMaster";
            public const string LeadRatingMaster = Default + ".LeadRatingMaster";
            public const string ExpenseTypeMaster = Default + ".ExpenseTypeMaster";
            public const string LeadTransaction = Default + ".LeadTransaction";
            public const string CustomerTransaction = Default + ".CustomerTransaction";
            public const string ContactTransaction = Default + ".ContactTransaction";
            public const string NoteTransaction = Default + ".NoteTransaction";
            public const string ServiceQuotationTransaction = Default + ".ServiceQuotationTransaction";
            public const string SalesQuotationTransaction = Default + ".SalesQuotationTransaction";
            public const string ExpenseTransaction = Default + ".ExpenseTransaction";
            public const string TaskTransaction = Default + ".TaskTransaction";
            public const string ImportantDateTransaction = Default + ".ImportantDateTransaction";
            public const string FileManagerTransaction = Default + ".FileManagerTransaction";
        }

        public static class Project
        {
            public const string Default = GroupName + ".Project";
            public const string ProjectOrderTransaction = Default + ".ProjectOrderTransaction";
            public const string ProjectOrderReport = Default + ".ProjectOrderReport";
            public const string ProjectOrderByCustomerReport = Default + ".ProjectOrderByCustomerReport";
            public const string ProjectOrderBySalesExecutiveReport = Default + ".ProjectOrderBySalesExecutiveReport";
        }
        public static class Services
        {
            public const string Default = GroupName + ".Service";
            public const string ServiceMaster = Default + ".ServiceMaster";
            public const string ServiceOrderTransaction = Default + ".ServiceOrderTransaction";
            public const string ServiceOrderReport = Default + ".ServiceOrderReport";
            public const string ServiceOrderByCustomerReport = Default + ".ServiceOrderByCustomerReport";
            public const string ServiceOrderBySalesExecutiveReport = Default + ".ServiceOrderBySalesExecutiveReport";
        }
        public static class Purchase
        {
            public const string Default = GroupName + ".Purchase";
            public const string BuyerMaster = Default + ".BuyerMaster";
            public const string VendorMaster = Default + ".VendorMaster";
            public const string PurchaseOrderTransaction = Default + ".PurchaseOrderTransaction";
            public const string PurchaseReceiptTransaction = Default + ".PurchaseReceiptTransaction";
            public const string PurchaseOrderReport = Default + ".PurchaseOrderReport";
            public const string PurchaseReceiptReport = Default + ".PurchaseReceiptReport";
            public const string PurchaseOrderByVendorReport = Default + ".PurchaseOrderByVendorReport";
            public const string PurchaseOrderByBuyerReport = Default + ".PurchaseOrderByBuyerReport";
        }
        public static class Sales
        {
            public const string Default = GroupName + ".Sales";
            public const string SalesExecutiveMaster = Default + ".SalesExecutiveMaster";
            public const string SalesOrderTransaction = Default + ".SalesOrderTransaction";
            public const string SalesDeliveryTransaction = Default + ".SalesDeliveryTransaction";
            public const string SalesOrderReport = Default + ".SalesOrderReport";
            public const string SalesDeliveryReport = Default + ".SalesDeliveryReport";
            public const string SalesOrderByCustomerReport = Default + ".SalesOrderByCustomerReport";
            public const string SalesOrderBySalesExecutiveReport = Default + ".SalesOrderBySalesExecutiveReport";
        }
        public static class Finance
        {
            public const string Default = GroupName + ".Finance";
            public const string CashAndBankMaster = Default + ".CashAndBankMaster";
            public const string VendorBillTransaction = Default + ".VendorBillTransaction";
            public const string VendorDebitNoteTransaction = Default + ".VendorDebitNoteTransaction";
            public const string VendorPaymentTransaction = Default + ".VendorPaymentTransaction";
            public const string CustomerInvoiceTransaction = Default + ".CustomerInvoiceTransaction";
            public const string CustomerCreditNoteTransaction = Default + ".CustomerCreditNoteTransaction";
            public const string CustomerPaymentTransaction = Default + ".CustomerPaymentTransaction";
            public const string VendorBillReport = Default + ".VendorBillReport";
            public const string VendorDebitNoteReport = Default + ".VendorDebitNoteReport";
            public const string VendorPaymentReport = Default + ".VendorPaymentReport";
            public const string CustomerInvoiceReport = Default + ".CustomerInvoiceReport";
            public const string CustomerCreditNoteReport = Default + ".CustomerCreditNoteReport";
            public const string CustomerPaymentReport = Default + ".CustomerPaymentReport";
            public const string CashAndBankReport = Default + ".CashAndBankReport";
        }
        public static class Transfer
        {
            public const string Default = GroupName + ".Transfer";
            public const string InterWarehouseTransferTransaction = Default + ".InterWarehouseTransferTransaction";
            public const string DeliveryOrderTransaction = Default + ".DeliveryOrderTransaction";
            public const string GoodsReceiptTransaction = Default + ".GoodsReceiptTransaction";
            public const string DeliveryOrderReport = Default + ".DeliveryOrderReport";
            public const string GoodsReceiptReport = Default + ".GoodsReceiptReport";
        }
        public static class Inventory
        {
            public const string Default = GroupName + ".Inventory";
            public const string UomMaster = Default + ".UomMaster";
            public const string ProductMaster = Default + ".ProductMaster";
            public const string WarehouseMaster = Default + ".WarehouseMaster";
            public const string AdjustmentTransaction = Default + ".AdjustmentTransaction";
            public const string MovementReport = Default + ".MovementReport";
            public const string StockReport = Default + ".StockReport";
            public const string AdjustmentReport = Default + ".AdjustmentReport";
        }
        public static class Settings
        {
            public const string Default = GroupName + ".Settings";
            public const string CompanyMaster = Default + ".CompanyMaster";
            public const string CurrencyMaster = Default + ".CurrencyMaster";
            public const string DepartmentMaster = Default + ".DepartmentMaster";
            public const string EmployeeMaster = Default + ".EmployeeMaster";
            public const string AddEmployee = Default + ".AddEmployee";
            public const string NumberSequenceMaster = Default + ".NumberSequenceMaster";
            public const string SkillMaster = Default + ".SkillMaster";
        }

        public static class Utilities
        {
            public const string Default = GroupName + ".Utilities";
            public const string ResourceMaster = Default + ".ResourceMaster";
            public const string BookingTransaction = Default + ".BookingTransaction";
            public const string CalendarTransaction = Default + ".CalendarTransaction";
            public const string TodoTransaction = Default + ".TodoTransaction";
            public const string DocumentTransaction = Default + ".DocumentTransaction";
            public const string ExpenseReport = Default + ".ExpenseReport";
            public const string BookingReport = Default + ".BookingReport";
        }
        public static class Client
        {
            public const string Default = GroupName + ".Client";
            public const string ClientsMaster = Default + ".ClientsMaster";
            public const string AddClient = Default + ".AddClient";
        }

        public static class Projectes
        {
            public const string Default = GroupName + ".Projects";
            public const string ProjectsMaster = Default + ".ProjectsMaster";
        }
        public static class Technologies
        {
            public const string Default = GroupName + ".Technology";
            public const string TechnologyMaster = Default + ".TechnologyMaster";
        }

        public static class EmailInformation
        {
            public const string Default = GroupName + ".EmailInformation";
            public const string EmailInformationMaster = Default + ".EmailInformationMaster";
        }
        
        public static class Leads
        {
            public const string Default = GroupName + ".Leads";
            public const string LeadsMaster = Default + ".LeadsMaster";
        }
    }
}