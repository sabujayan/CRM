using Indo.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Indo.Permissions
{
    public class IndoPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var crmGroup = context.AddGroup(IndoPermissions.GroupName, L("Permission:AppCrm"));


            var dashboardPermission = crmGroup.AddPermission(IndoPermissions.Dashboard.Default, L("Permission:Dashboard"));
            dashboardPermission.AddChild(IndoPermissions.Dashboard.Crm, L("Permission:Dashboard.Crm"));
            dashboardPermission.AddChild(IndoPermissions.Dashboard.Inventory, L("Permission:Dashboard.Inventory"));

            var customerManagementPermission = crmGroup.AddPermission(IndoPermissions.CustomerManagement.Default, L("Permission:CustomerManagement"));
            customerManagementPermission.AddChild(IndoPermissions.CustomerManagement.ActivityMaster, L("Permission:CustomerManagement.ActivityMaster"));
            customerManagementPermission.AddChild(IndoPermissions.CustomerManagement.LeadSourceMaster, L("Permission:CustomerManagement.LeadSourceMaster"));
            customerManagementPermission.AddChild(IndoPermissions.CustomerManagement.LeadRatingMaster, L("Permission:CustomerManagement.LeadRatingMaster"));
            customerManagementPermission.AddChild(IndoPermissions.CustomerManagement.ExpenseTypeMaster, L("Permission:CustomerManagement.ExpenseTypeMaster"));
            customerManagementPermission.AddChild(IndoPermissions.CustomerManagement.LeadTransaction, L("Permission:CustomerManagement.LeadTransaction"));
            customerManagementPermission.AddChild(IndoPermissions.CustomerManagement.CustomerTransaction, L("Permission:CustomerManagement.CustomerTransaction"));
            customerManagementPermission.AddChild(IndoPermissions.CustomerManagement.ContactTransaction, L("Permission:CustomerManagement.ContactTransaction"));
            customerManagementPermission.AddChild(IndoPermissions.CustomerManagement.NoteTransaction, L("Permission:CustomerManagement.NoteTransaction"));
            customerManagementPermission.AddChild(IndoPermissions.CustomerManagement.ServiceQuotationTransaction, L("Permission:CustomerManagement.ServiceQuotationTransaction"));
            customerManagementPermission.AddChild(IndoPermissions.CustomerManagement.SalesQuotationTransaction, L("Permission:CustomerManagement.SalesQuotationTransaction"));
            customerManagementPermission.AddChild(IndoPermissions.CustomerManagement.ExpenseTransaction, L("Permission:CustomerManagement.ExpenseTransaction"));
            customerManagementPermission.AddChild(IndoPermissions.CustomerManagement.TaskTransaction, L("Permission:CustomerManagement.TaskTransaction"));
            customerManagementPermission.AddChild(IndoPermissions.CustomerManagement.ImportantDateTransaction, L("Permission:CustomerManagement.ImportantDateTransaction"));
            customerManagementPermission.AddChild(IndoPermissions.CustomerManagement.FileManagerTransaction, L("Permission:CustomerManagement.FileManagerTransaction"));

            var projectPermission = crmGroup.AddPermission(IndoPermissions.Project.Default, L("Permission:Project"));
            projectPermission.AddChild(IndoPermissions.Project.ProjectOrderTransaction, L("Permission:Project.ProjectOrderTransaction"));
            projectPermission.AddChild(IndoPermissions.Project.ProjectOrderReport, L("Permission:Project.ProjectOrderReport"));
            projectPermission.AddChild(IndoPermissions.Project.ProjectOrderByCustomerReport, L("Permission:Project.ProjectOrderByCustomerReport"));
            projectPermission.AddChild(IndoPermissions.Project.ProjectOrderBySalesExecutiveReport, L("Permission:Project.ProjectOrderBySalesExecutiveReport"));

            var servicePermission = crmGroup.AddPermission(IndoPermissions.Services.Default, L("Permission:Service"));
            servicePermission.AddChild(IndoPermissions.Services.ServiceMaster, L("Permission:Service.ServiceMaster"));
            servicePermission.AddChild(IndoPermissions.Services.ServiceOrderTransaction, L("Permission:Service.ServiceOrderTransaction"));
            servicePermission.AddChild(IndoPermissions.Services.ServiceOrderReport, L("Permission:Service.ServiceOrderReport"));
            servicePermission.AddChild(IndoPermissions.Services.ServiceOrderByCustomerReport, L("Permission:Service.ServiceOrderByCustomerReport"));
            servicePermission.AddChild(IndoPermissions.Services.ServiceOrderBySalesExecutiveReport, L("Permission:Service.ServiceOrderBySalesExecutiveReport"));

            var purchasePermission = crmGroup.AddPermission(IndoPermissions.Purchase.Default, L("Permission:Purchase"));
            purchasePermission.AddChild(IndoPermissions.Purchase.BuyerMaster, L("Permission:Purchase.BuyerMaster"));
            purchasePermission.AddChild(IndoPermissions.Purchase.VendorMaster, L("Permission:Purchase.VendorMaster"));
            purchasePermission.AddChild(IndoPermissions.Purchase.PurchaseOrderTransaction, L("Permission:Purchase.PurchaseOrderTransaction"));
            purchasePermission.AddChild(IndoPermissions.Purchase.PurchaseReceiptTransaction, L("Permission:Purchase.PurchaseReceiptTransaction"));
            purchasePermission.AddChild(IndoPermissions.Purchase.PurchaseOrderReport, L("Permission:Purchase.PurchaseOrderReport"));
            purchasePermission.AddChild(IndoPermissions.Purchase.PurchaseReceiptReport, L("Permission:Purchase.PurchaseReceiptReport"));
            purchasePermission.AddChild(IndoPermissions.Purchase.PurchaseOrderByVendorReport, L("Permission:Purchase.PurchaseOrderByVendorReport"));
            purchasePermission.AddChild(IndoPermissions.Purchase.PurchaseOrderByBuyerReport, L("Permission:Purchase.PurchaseOrderByBuyerReport"));

            var salesPermission = crmGroup.AddPermission(IndoPermissions.Sales.Default, L("Permission:Sales"));
            salesPermission.AddChild(IndoPermissions.Sales.SalesExecutiveMaster, L("Permission:Sales.SalesExecutiveMaster"));
            salesPermission.AddChild(IndoPermissions.Sales.SalesOrderTransaction, L("Permission:Sales.SalesOrderTransaction"));
            salesPermission.AddChild(IndoPermissions.Sales.SalesDeliveryTransaction, L("Permission:Sales.SalesDeliveryTransaction"));
            salesPermission.AddChild(IndoPermissions.Sales.SalesOrderReport, L("Permission:Sales.SalesOrderReport"));
            salesPermission.AddChild(IndoPermissions.Sales.SalesDeliveryReport, L("Permission:Sales.SalesDeliveryReport"));
            salesPermission.AddChild(IndoPermissions.Sales.SalesOrderByCustomerReport, L("Permission:Sales.SalesOrderByCustomerReport"));
            salesPermission.AddChild(IndoPermissions.Sales.SalesOrderBySalesExecutiveReport, L("Permission:Sales.SalesOrderBySalesExecutiveReport"));

            var financePermission = crmGroup.AddPermission(IndoPermissions.Finance.Default, L("Permission:Finance"));
            financePermission.AddChild(IndoPermissions.Finance.CashAndBankMaster, L("Permission:Finance.CashAndBankMaster"));
            financePermission.AddChild(IndoPermissions.Finance.VendorBillTransaction, L("Permission:Finance.VendorBillTransaction"));
            financePermission.AddChild(IndoPermissions.Finance.VendorDebitNoteTransaction, L("Permission:Finance.VendorDebitNoteTransaction"));
            financePermission.AddChild(IndoPermissions.Finance.VendorPaymentTransaction, L("Permission:Finance.VendorPaymentTransaction"));
            financePermission.AddChild(IndoPermissions.Finance.CustomerInvoiceTransaction, L("Permission:Finance.CustomerInvoiceTransaction"));
            financePermission.AddChild(IndoPermissions.Finance.CustomerCreditNoteTransaction, L("Permission:Finance.CustomerCreditNoteTransaction"));
            financePermission.AddChild(IndoPermissions.Finance.CustomerPaymentTransaction, L("Permission:Finance.CustomerPaymentTransaction"));
            financePermission.AddChild(IndoPermissions.Finance.VendorBillReport, L("Permission:Finance.VendorBillReport"));
            financePermission.AddChild(IndoPermissions.Finance.VendorDebitNoteReport, L("Permission:Finance.VendorDebitNoteReport"));
            financePermission.AddChild(IndoPermissions.Finance.VendorPaymentReport, L("Permission:Finance.VendorPaymentReport"));
            financePermission.AddChild(IndoPermissions.Finance.CustomerInvoiceReport, L("Permission:Finance.CustomerInvoiceReport"));
            financePermission.AddChild(IndoPermissions.Finance.CustomerCreditNoteReport, L("Permission:Finance.CustomerCreditNoteReport"));
            financePermission.AddChild(IndoPermissions.Finance.CustomerPaymentReport, L("Permission:Finance.CustomerPaymentReport"));
            financePermission.AddChild(IndoPermissions.Finance.CashAndBankReport, L("Permission:Finance.CashAndBankReport"));

            var transferPermission = crmGroup.AddPermission(IndoPermissions.Transfer.Default, L("Permission:Transfer"));
            transferPermission.AddChild(IndoPermissions.Transfer.InterWarehouseTransferTransaction, L("Permission:Transfer.InterWarehouseTransferTransaction"));
            transferPermission.AddChild(IndoPermissions.Transfer.DeliveryOrderTransaction, L("Permission:Transfer.DeliveryOrderTransaction"));
            transferPermission.AddChild(IndoPermissions.Transfer.GoodsReceiptTransaction, L("Permission:Transfer.GoodsReceiptTransaction"));
            transferPermission.AddChild(IndoPermissions.Transfer.DeliveryOrderReport, L("Permission:Transfer.DeliveryOrderReport"));
            transferPermission.AddChild(IndoPermissions.Transfer.GoodsReceiptReport, L("Permission:Transfer.GoodsReceiptReport"));

            var inventoryPermission = crmGroup.AddPermission(IndoPermissions.Inventory.Default, L("Permission:Inventory"));
            inventoryPermission.AddChild(IndoPermissions.Inventory.UomMaster, L("Permission:Inventory.UomMaster"));
            inventoryPermission.AddChild(IndoPermissions.Inventory.ProductMaster, L("Permission:Inventory.ProductMaster"));
            inventoryPermission.AddChild(IndoPermissions.Inventory.WarehouseMaster, L("Permission:Inventory.WarehouseMaster"));
            inventoryPermission.AddChild(IndoPermissions.Inventory.AdjustmentTransaction, L("Permission:Inventory.AdjustmentTransaction"));
            inventoryPermission.AddChild(IndoPermissions.Inventory.MovementReport, L("Permission:Inventory.MovementReport"));
            inventoryPermission.AddChild(IndoPermissions.Inventory.StockReport, L("Permission:Inventory.StockReport"));
            inventoryPermission.AddChild(IndoPermissions.Inventory.AdjustmentReport, L("Permission:Inventory.AdjustmentReport"));

            var settingsPermission = crmGroup.AddPermission(IndoPermissions.Settings.Default, L("Permission:Settings"));
            settingsPermission.AddChild(IndoPermissions.Settings.CompanyMaster, L("Permission:Settings.CompanyMaster"));
            settingsPermission.AddChild(IndoPermissions.Settings.CurrencyMaster, L("Permission:Settings.CurrencyMaster"));
            settingsPermission.AddChild(IndoPermissions.Settings.DepartmentMaster, L("Permission:Settings.DepartmentMaster"));
            settingsPermission.AddChild(IndoPermissions.Settings.EmployeeMaster, L("Permission:Settings.EmployeeMaster"));
            settingsPermission.AddChild(IndoPermissions.Settings.AddEmployee, L("Permission:Settings.AddEmployeeMaster"));
            settingsPermission.AddChild(IndoPermissions.Settings.NumberSequenceMaster, L("Permission:Settings.NumberSequenceMaster"));
            settingsPermission.AddChild(IndoPermissions.Settings.SkillMaster, L("Permission:Settings.SkillMaster"));

            var utilitiesPermission = crmGroup.AddPermission(IndoPermissions.Utilities.Default, L("Permission:Utilities"));
            utilitiesPermission.AddChild(IndoPermissions.Utilities.ResourceMaster, L("Permission:Utilities.ResourceMaster"));
            utilitiesPermission.AddChild(IndoPermissions.Utilities.BookingTransaction, L("Permission:Utilities.BookingTransaction"));
            utilitiesPermission.AddChild(IndoPermissions.Utilities.CalendarTransaction, L("Permission:Utilities.CalendarTransaction"));
            utilitiesPermission.AddChild(IndoPermissions.Utilities.TodoTransaction, L("Permission:Utilities.TodoTransaction"));
            utilitiesPermission.AddChild(IndoPermissions.Utilities.DocumentTransaction, L("Permission:Utilities.DocumentTransaction"));
            utilitiesPermission.AddChild(IndoPermissions.Utilities.ExpenseReport, L("Permission:Utilities.ExpenseReport"));
            utilitiesPermission.AddChild(IndoPermissions.Utilities.BookingReport, L("Permission:Utilities.BookingReport"));

            var clientsPermission = crmGroup.AddPermission(IndoPermissions.Client.Default, L("Permission:Client"));
            clientsPermission.AddChild(IndoPermissions.Client.ClientsMaster, L("Permission:Clients.ClientsMaster"));
            clientsPermission.AddChild(IndoPermissions.Client.AddClient, L("Permission:Clients.AddClient"));

            var ProjectsPermission = crmGroup.AddPermission(IndoPermissions.Projectes.Default, L("Permission:Projects"));
            ProjectsPermission.AddChild(IndoPermissions.Projectes.ProjectsMaster, L("Permission:Clients.ProjectsMaster"));

            var TechnologyPermission = crmGroup.AddPermission(IndoPermissions.Technologies.Default, L("Permission:Technologies"));
            TechnologyPermission.AddChild(IndoPermissions.Technologies.TechnologyMaster, L("Permission:Technologies.TechnologiesMaster"));

            var EmailInformationPermission = crmGroup.AddPermission(IndoPermissions.EmailInformation.Default, L("Permission:EmailInformation"));
            EmailInformationPermission.AddChild(IndoPermissions.EmailInformation.EmailInformationMaster, L("Permission:EmailInformation.EmailInformationMaster"));
            
            var LeadsPermission = crmGroup.AddPermission(IndoPermissions.Leads.Default, L("Permission:Leads"));
            LeadsPermission.AddChild(IndoPermissions.Leads.LeadsMaster, L("Permission:Leads.LeadsMaster"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<IndoResource>(name);
        }
    }
}
