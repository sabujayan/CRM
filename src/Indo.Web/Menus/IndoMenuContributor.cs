using System.Threading.Tasks;
using Indo.Localization;
using Indo.MultiTenancy;
using Indo.Permissions;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;

namespace Indo.Web.Menus
{
    public class IndoMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            var administration = context.Menu.GetAdministration();
            var l = context.GetLocalizer<IndoResource>();

            if (await context.IsGrantedAsync(IndoPermissions.Dashboard.Default))
            {
                var dashboardMenu = new ApplicationMenuItem("Dashboards", l["Menu:Dashboard"], icon: "icon-home", order: 100);
                context.Menu.AddItem(dashboardMenu);

                if (await context.IsGrantedAsync(IndoPermissions.Dashboard.Crm))
                {
                    dashboardMenu.AddItem(new ApplicationMenuItem(name: IndoMenus.Dashboard, displayName: l["Menu:DashboardCrm"], url: "~/Dashboard", order: 101));
                }
                if (await context.IsGrantedAsync(IndoPermissions.Dashboard.Inventory))
                {
                    dashboardMenu.AddItem(new ApplicationMenuItem(name: IndoMenus.DashboardInventory, displayName: l["Menu:DashboardInventory"], url: "~/DashboardInventory", order: 102));
                }

            }

            if (await context.IsGrantedAsync(IndoPermissions.CustomerManagement.Default))
            {
                var customerManagementMenu = new ApplicationMenuItem("Customer Management", l["Menu:CustomerManagement"], icon: "icon-star", order: 150);
                context.Menu.AddItem(customerManagementMenu);

                var master = new ApplicationMenuItem("Data Master", l["Menu:CustomerManagementDataMaster"], order: 160);
                customerManagementMenu.AddItem(master);

                if (await context.IsGrantedAsync(IndoPermissions.CustomerManagement.ActivityMaster))
                {
                    master.AddItem(new ApplicationMenuItem(name: IndoMenus.Activity, displayName: l["Menu:CustomerManagementDataMasterActivity"], url: "~/Activity", order: 161));
                }

                if (await context.IsGrantedAsync(IndoPermissions.CustomerManagement.LeadSourceMaster))
                {
                    master.AddItem(new ApplicationMenuItem(name: IndoMenus.LeadSource, displayName: l["Menu:CustomerManagementDataMasterLeadSource"], url: "~/LeadSource", order: 162));
                }

                if (await context.IsGrantedAsync(IndoPermissions.CustomerManagement.LeadRatingMaster))
                {
                    master.AddItem(new ApplicationMenuItem(name: IndoMenus.LeadRating, displayName: l["Menu:CustomerManagementDataMasterLeadRating"], url: "~/LeadRating", order: 163));
                }

                if (await context.IsGrantedAsync(IndoPermissions.CustomerManagement.ExpenseTypeMaster))
                {
                    master.AddItem(new ApplicationMenuItem(name: IndoMenus.ExpenseType, displayName: l["Menu:CustomerManagementDataMasterExpenseType"], url: "~/ExpenseType", order: 164));
                }


                var transaction = new ApplicationMenuItem("Transactions", l["Menu:CustomerManagementTransaction"], order: 170);
                customerManagementMenu.AddItem(transaction);

                if (await context.IsGrantedAsync(IndoPermissions.CustomerManagement.LeadTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.Lead, displayName: l["Menu:CustomerManagementTransactionLead"], url: "~/Lead", order: 171));
                }

                if (await context.IsGrantedAsync(IndoPermissions.CustomerManagement.CustomerTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.Customer, displayName: l["Menu:CustomerManagementTransactionCustomer"], url: "~/Customer", order: 172));
                }

                if (await context.IsGrantedAsync(IndoPermissions.CustomerManagement.ContactTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.Contact, displayName: l["Menu:CustomerManagementTransactionContact"], url: "~/Contact", order: 173));
                }

                if (await context.IsGrantedAsync(IndoPermissions.CustomerManagement.NoteTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.Note, displayName: l["Menu:CustomerManagementTransactionNote"], url: "~/Note", order: 174));
                }

                if (await context.IsGrantedAsync(IndoPermissions.CustomerManagement.ServiceQuotationTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.ServiceQuotation, displayName: l["Menu:CustomerManagementTransactionServiceQuotation"], url: "~/ServiceQuotation", order: 175));
                }

                if (await context.IsGrantedAsync(IndoPermissions.CustomerManagement.SalesQuotationTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.SalesQuotation, displayName: l["Menu:CustomerManagementTransactionSalesQuotation"], url: "~/SalesQuotation", order: 176));
                }

                if (await context.IsGrantedAsync(IndoPermissions.CustomerManagement.ExpenseTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.Expense, displayName: l["Menu:CustomerManagementTransactionExpense"], url: "~/Expense", order: 177));
                }

                if (await context.IsGrantedAsync(IndoPermissions.CustomerManagement.TaskTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.Task, displayName: l["Menu:CustomerManagementTransactionTask"], url: "~/Task", order: 178));
                }

                if (await context.IsGrantedAsync(IndoPermissions.CustomerManagement.ImportantDateTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.ImportantDate, displayName: l["Menu:CustomerManagementTransactionImportantDate"], url: "~/ImportantDate", order: 179));
                }

            }

            if (await context.IsGrantedAsync(IndoPermissions.Project.Default))
            {
                var projectMenu = new ApplicationMenuItem("Projects", l["Menu:Project"], icon: "icon-award", order: 200);
                context.Menu.AddItem(projectMenu);

                if (await context.IsGrantedAsync(IndoPermissions.Project.ProjectOrderTransaction))
                {
                    projectMenu.AddItem(new ApplicationMenuItem("Transactions", l["Menu:ProjectTransaction"], order: 210)
                        .AddItem(new ApplicationMenuItem(name: IndoMenus.ProjectOrder, displayName: l["Menu:ProjectTransactionProjectOrder"], url: "~/ProjectOrder", order: 211)));
                }

                var report = new ApplicationMenuItem("Reports", l["Menu:ProjectReport"], order: 250);
                projectMenu.AddItem(report);

                if (await context.IsGrantedAsync(IndoPermissions.Project.ProjectOrderReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.ProjectOrderReport, displayName: l["Menu:ProjectReportProjectOrder"], url: "~/ProjectOrderReport", order: 251));
                }
                if (await context.IsGrantedAsync(IndoPermissions.Project.ProjectOrderByCustomerReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.ProjectOrderByCustomerReport, displayName: l["Menu:ProjectReportProjectOrderByCustomer"], url: "~/ProjectOrderByCustomerReport", order: 252));
                }
                if (await context.IsGrantedAsync(IndoPermissions.Project.ProjectOrderBySalesExecutiveReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.ProjectOrderBySalesExecutiveReport, displayName: l["Menu:ProjectReportProjectOrderBySalesExecutive"], url: "~/ProjectOrderBySalesExecutiveReport", order: 253));
                }

            }

            if (await context.IsGrantedAsync(IndoPermissions.Services.Default))
            {
                var serviceMenu = new ApplicationMenuItem("Services", l["Menu:Service"], icon: "icon-clock", order: 300);
                context.Menu.AddItem(serviceMenu);

                if (await context.IsGrantedAsync(IndoPermissions.Services.ServiceMaster))
                {
                    serviceMenu.AddItem(new ApplicationMenuItem("Data Master", l["Menu:ServiceDataMaster"], order: 310)
                            .AddItem(new ApplicationMenuItem(name: IndoMenus.Service, displayName: l["Menu:ServiceDataMasterService"], url: "~/Service", order: 311)));
                }

                if (await context.IsGrantedAsync(IndoPermissions.Services.ServiceOrderTransaction))
                {
                    serviceMenu.AddItem(new ApplicationMenuItem("Transactions", l["Menu:ServiceTransaction"], order: 320)
                            .AddItem(new ApplicationMenuItem(name: IndoMenus.ServiceOrder, displayName: l["Menu:ServiceTransactionServiceOrder"], url: "~/ServiceOrder", order: 321)));
                }

                var report = new ApplicationMenuItem("Reports", l["Menu:ServiceReport"], order: 350);
                serviceMenu.AddItem(report);

                if (await context.IsGrantedAsync(IndoPermissions.Services.ServiceOrderReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.ServiceOrderReport, displayName: l["Menu:ServiceReportServiceOrder"], url: "~/ServiceOrderReport", order: 351));
                }
                if (await context.IsGrantedAsync(IndoPermissions.Services.ServiceOrderByCustomerReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.ServiceOrderByCustomerReport, displayName: l["Menu:ServiceReportServiceOrderByCustomer"], url: "~/ServiceOrderByCustomerReport", order: 352));
                }
                if (await context.IsGrantedAsync(IndoPermissions.Services.ServiceOrderBySalesExecutiveReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.ServiceOrderBySalesExecutiveReport, displayName: l["Menu:ServiceReportServiceOrderBySalesExecutive"], url: "~/ServiceOrderBySalesExecutiveReport", order: 353));
                }

            }

            if (await context.IsGrantedAsync(IndoPermissions.Purchase.Default))
            {
                var purchaseMenu = new ApplicationMenuItem("Purchases", l["Menu:Purchase"], icon: "icon-tag", order: 400);
                context.Menu.AddItem(purchaseMenu);

                var master = new ApplicationMenuItem("Data Master", l["Menu:PurchaseDataMaster"], order: 410);
                purchaseMenu.AddItem(master);

                if (await context.IsGrantedAsync(IndoPermissions.Purchase.BuyerMaster))
                {
                    master.AddItem(new ApplicationMenuItem(name: IndoMenus.Buyer, displayName: l["Menu:PurchaseDataMasterBuyer"], url: "~/Buyer", order: 411));
                }

                if (await context.IsGrantedAsync(IndoPermissions.Purchase.VendorMaster))
                {
                    master.AddItem(new ApplicationMenuItem(name: IndoMenus.Vendor, displayName: l["Menu:PurchaseDataMasterVendor"], url: "~/Vendor", order: 412));
                }

                var transaction = new ApplicationMenuItem("Transactions", l["Menu:PurchaseTransaction"], order: 420);
                purchaseMenu.AddItem(transaction);

                if (await context.IsGrantedAsync(IndoPermissions.Purchase.PurchaseOrderTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.PurchaseOrder, displayName: l["Menu:PurchaseTransactionPurchaseOrder"], url: "~/PurchaseOrder", order: 421));
                }

                if (await context.IsGrantedAsync(IndoPermissions.Purchase.PurchaseReceiptTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.PurchaseReceipt, displayName: l["Menu:PurchaseTransactionPurchaseReceipt"], url: "~/PurchaseReceipt", order: 422));
                }

                var report = new ApplicationMenuItem("Reports", l["Menu:PurchaseReport"], order: 450);
                purchaseMenu.AddItem(report);

                if (await context.IsGrantedAsync(IndoPermissions.Purchase.PurchaseOrderReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.PurchaseOrderReport, displayName: l["Menu:PurchaseReportPurchaseOrder"], url: "~/PurchaseOrderReport", order: 451));
                }
                if (await context.IsGrantedAsync(IndoPermissions.Purchase.PurchaseOrderByVendorReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.PurchaseOrderByVendorReport, displayName: l["Menu:PurchaseReportPurchaseOrderByVendor"], url: "~/PurchaseOrderByVendorReport", order: 452));
                }
                if (await context.IsGrantedAsync(IndoPermissions.Purchase.PurchaseOrderByBuyerReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.PurchaseOrderByBuyerReport, displayName: l["Menu:PurchaseReportPurchaseOrderByBuyer"], url: "~/PurchaseOrderByBuyerReport", order: 453));
                }
                if (await context.IsGrantedAsync(IndoPermissions.Purchase.PurchaseReceiptReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.PurchaseReceiptReport, displayName: l["Menu:PurchaseReportPurchaseReceipt"], url: "~/PurchaseReceiptReport", order: 454));
                }

            }

            if (await context.IsGrantedAsync(IndoPermissions.Sales.Default))
            {
                var salesMenu = new ApplicationMenuItem("Sales", l["Menu:Sales"], icon: "icon-shopping-cart", order: 500);
                context.Menu.AddItem(salesMenu);

                var master = new ApplicationMenuItem("Data Master", l["Menu:SalesDataMaster"], order: 510);
                salesMenu.AddItem(master);

                if (await context.IsGrantedAsync(IndoPermissions.Sales.SalesExecutiveMaster))
                {
                    master.AddItem(new ApplicationMenuItem(name: IndoMenus.SalesExecutive, displayName: l["Menu:SalesDataMasterSalesExecutive"], url: "~/SalesExecutive", order: 511));
                }

                var transaction = new ApplicationMenuItem("Transactions", l["Menu:SalesTransaction"], order: 520);
                salesMenu.AddItem(transaction);

                if (await context.IsGrantedAsync(IndoPermissions.Sales.SalesOrderTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.SalesOrder, displayName: l["Menu:SalesTransactionSalesOrder"], url: "~/SalesOrder", order: 521));
                }

                if (await context.IsGrantedAsync(IndoPermissions.Sales.SalesDeliveryTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.SalesDelivery, displayName: l["Menu:SalesTransactionSalesDelivery"], url: "~/SalesDelivery", order: 522));
                }

                var report = new ApplicationMenuItem("Reports", l["Menu:SalesReport"], order: 550);
                salesMenu.AddItem(report);

                if (await context.IsGrantedAsync(IndoPermissions.Sales.SalesOrderReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.SalesOrderReport, displayName: l["Menu:SalesReportSalesOrder"], url: "~/SalesOrderReport", order: 551));
                }
                if (await context.IsGrantedAsync(IndoPermissions.Sales.SalesOrderByCustomerReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.SalesOrderByCustomerReport, displayName: l["Menu:SalesReportSalesOrderByCustomer"], url: "~/SalesOrderByCustomerReport", order: 552));
                }
                if (await context.IsGrantedAsync(IndoPermissions.Sales.SalesOrderBySalesExecutiveReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.SalesOrderBySalesExecutiveReport, displayName: l["Menu:SalesReportSalesOrderBySalesExecutive"], url: "~/SalesOrderBySalesExecutiveReport", order: 553));
                }
                if (await context.IsGrantedAsync(IndoPermissions.Sales.SalesDeliveryReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.SalesDeliveryReport, displayName: l["Menu:SalesReportSalesDelivery"], url: "~/SalesDeliveryReport", order: 554));
                }

            }

            if (await context.IsGrantedAsync(IndoPermissions.Finance.Default))
            {
                var financeMenu = new ApplicationMenuItem("Finance", l["Menu:Finance"], icon: "icon-book", order: 560);
                context.Menu.AddItem(financeMenu);

                var master = new ApplicationMenuItem("Data Master", l["Menu:FinanceDataMaster"], order: 560);
                financeMenu.AddItem(master);

                if (await context.IsGrantedAsync(IndoPermissions.Finance.CashAndBankMaster))
                {
                    master.AddItem(new ApplicationMenuItem(name: IndoMenus.CashAndBank, displayName: l["Menu:FinanceDataMasterCashAndBank"], url: "~/CashAndBank", order: 561));
                }

                var transaction = new ApplicationMenuItem("Transactions", l["Menu:FinanceTransaction"], order: 570);
                financeMenu.AddItem(transaction);

                if (await context.IsGrantedAsync(IndoPermissions.Finance.VendorBillTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.VendorBill, displayName: l["Menu:FinanceTransactionVendorBill"], url: "~/VendorBill", order: 571));
                }

                if (await context.IsGrantedAsync(IndoPermissions.Finance.VendorDebitNoteTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.VendorDebitNote, displayName: l["Menu:FinanceTransactionVendorDebitNote"], url: "~/VendorDebitNote", order: 572));
                }

                if (await context.IsGrantedAsync(IndoPermissions.Finance.VendorPaymentTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.VendorPayment, displayName: l["Menu:FinanceTransactionVendorPayment"], url: "~/VendorPayment", order: 573));
                }

                if (await context.IsGrantedAsync(IndoPermissions.Finance.CustomerInvoiceTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.CustomerInvoice, displayName: l["Menu:FinanceTransactionCustomerInvoice"], url: "~/CustomerInvoice", order: 574));
                }

                if (await context.IsGrantedAsync(IndoPermissions.Finance.CustomerCreditNoteTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.CustomerCreditNote, displayName: l["Menu:FinanceTransactionCustomerCreditNote"], url: "~/CustomerCreditNote", order: 575));
                }

                if (await context.IsGrantedAsync(IndoPermissions.Finance.CustomerPaymentTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.CustomerPayment, displayName: l["Menu:FinanceTransactionCustomerPayment"], url: "~/CustomerPayment", order: 576));
                }

                var report = new ApplicationMenuItem("Reports", l["Menu:FinanceReport"], order: 580);
                financeMenu.AddItem(report);

                if (await context.IsGrantedAsync(IndoPermissions.Finance.VendorBillReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.VendorBillReport, displayName: l["Menu:FinanceReportVendorBill"], url: "~/VendorBillReport", order: 581));
                }
                if (await context.IsGrantedAsync(IndoPermissions.Finance.VendorDebitNoteReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.VendorDebitNoteReport, displayName: l["Menu:FinanceReportVendorDebitNote"], url: "~/VendorDebitNoteReport", order: 582));
                }
                if (await context.IsGrantedAsync(IndoPermissions.Finance.VendorPaymentReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.VendorPaymentReport, displayName: l["Menu:FinanceReportVendorPayment"], url: "~/VendorPaymentReport", order: 583));
                }
                if (await context.IsGrantedAsync(IndoPermissions.Finance.CustomerInvoiceReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.CustomerInvoiceReport, displayName: l["Menu:FinanceReportCustomerInvoice"], url: "~/CustomerInvoiceReport", order: 584));
                }
                if (await context.IsGrantedAsync(IndoPermissions.Finance.CustomerCreditNoteReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.CustomerCreditNoteReport, displayName: l["Menu:FinanceReportCustomerCreditNote"], url: "~/CustomerCreditNoteReport", order: 585));
                }
                if (await context.IsGrantedAsync(IndoPermissions.Finance.CustomerPaymentReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.CustomerPaymentReport, displayName: l["Menu:FinanceReportCustomerPayment"], url: "~/CustomerPaymentReport", order: 586));
                }
                if (await context.IsGrantedAsync(IndoPermissions.Finance.CashAndBankReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.CashAndBankReport, displayName: l["Menu:FinanceReportCashAndBank"], url: "~/CashAndBankReport", order: 587));
                }

            }

            if (await context.IsGrantedAsync(IndoPermissions.Transfer.Default))
            {
                var transferMenu = new ApplicationMenuItem("Transfer", l["Menu:Transfer"], icon: "icon-layers", order: 600);
                context.Menu.AddItem(transferMenu);

                var transaction = new ApplicationMenuItem("Transactions", l["Menu:TransferTransaction"], order: 610);
                transferMenu.AddItem(transaction);

                if (await context.IsGrantedAsync(IndoPermissions.Transfer.InterWarehouseTransferTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.TransferOrder, displayName: l["Menu:TransferTransactionTransferOrder"], url: "~/TransferOrder", order: 611));
                }

                if (await context.IsGrantedAsync(IndoPermissions.Transfer.DeliveryOrderTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.DeliveryOrder, displayName: l["Menu:TransferTransactionDeliveryOrder"], url: "~/DeliveryOrder", order: 612));
                }

                if (await context.IsGrantedAsync(IndoPermissions.Transfer.GoodsReceiptTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.GoodsReceipt, displayName: l["Menu:TransferTransactionGoodsReceipt"], url: "~/GoodsReceipt", order: 613));
                }

                var report = new ApplicationMenuItem("Reports", l["Menu:TransferReport"], order: 650);
                transferMenu.AddItem(report);

                if (await context.IsGrantedAsync(IndoPermissions.Transfer.DeliveryOrderReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.DeliveryOrderReport, displayName: l["Menu:TransferReportDeliveryOrder"], url: "~/DeliveryOrderReport", order: 652));
                }

                if (await context.IsGrantedAsync(IndoPermissions.Transfer.GoodsReceiptReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.GoodsReceiptReport, displayName: l["Menu:TransferReportGoodsReceipt"], url: "~/GoodsReceiptReport", order: 653));
                }

            }

            if (await context.IsGrantedAsync(IndoPermissions.Inventory.Default))
            {
                var inventoryMenu = new ApplicationMenuItem("Inventory", l["Menu:Inventory"], icon: "icon-box", order: 700);
                context.Menu.AddItem(inventoryMenu);

                var master = new ApplicationMenuItem("Data Master", l["Menu:InventoryDataMaster"], order: 710);
                inventoryMenu.AddItem(master);

                if (await context.IsGrantedAsync(IndoPermissions.Inventory.UomMaster))
                {
                    master.AddItem(new ApplicationMenuItem(name: IndoMenus.Uom, displayName: l["Menu:InventoryDataMasterUom"], url: "~/Uom", order: 711));
                }

                if (await context.IsGrantedAsync(IndoPermissions.Inventory.ProductMaster))
                {
                    master.AddItem(new ApplicationMenuItem(name: IndoMenus.Product, displayName: l["Menu:InventoryDataMasterProduct"], url: "~/Product", order: 712));
                }

                if (await context.IsGrantedAsync(IndoPermissions.Inventory.WarehouseMaster))
                {
                    master.AddItem(new ApplicationMenuItem(name: IndoMenus.Warehouse, displayName: l["Menu:InventoryDataMasterWarehouse"], url: "~/Warehouse", order: 713));
                }

                var transaction = new ApplicationMenuItem("Transactions", l["Menu:InventoryTransaction"], order: 720);
                inventoryMenu.AddItem(transaction);

                if (await context.IsGrantedAsync(IndoPermissions.Inventory.AdjustmentTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.Adjustment, displayName: l["Menu:InventoryTransactionAdjustment"], url: "~/StockAdjustment", order: 721));
                }

                var report = new ApplicationMenuItem("Reports", l["Menu:InventoryReport"], order: 750);
                inventoryMenu.AddItem(report);

                if (await context.IsGrantedAsync(IndoPermissions.Inventory.MovementReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.Movement, displayName: l["Menu:InventoryReportMovement"], url: "~/Movement", order: 7501));
                }

                if (await context.IsGrantedAsync(IndoPermissions.Inventory.StockReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.Stock, displayName: l["Menu:InventoryReportStock"], url: "~/Stock", order: 7502));
                }

                if (await context.IsGrantedAsync(IndoPermissions.Inventory.AdjustmentReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.StockAdjustmentReport, displayName: l["Menu:InventoryReportAdjustment"], url: "~/StockAdjustmentReport", order: 7503));
                }
            }

            if (await context.IsGrantedAsync(IndoPermissions.Utilities.Default))
            {
                var utilitiesMenu = new ApplicationMenuItem("Utilities", l["Menu:Utilities"], icon: "icon-umbrella", order: 800);
                context.Menu.AddItem(utilitiesMenu);

                var master = new ApplicationMenuItem("Data Master", l["Menu:UtilitiesDataMaster"], order: 810);
                utilitiesMenu.AddItem(master);

                if (await context.IsGrantedAsync(IndoPermissions.Utilities.ResourceMaster))
                {
                    master.AddItem(new ApplicationMenuItem(name: IndoMenus.Resource, displayName: l["Menu:UtilitiesDataMasterResource"], url: "~/Resource", order: 812));
                }

                var transaction = new ApplicationMenuItem("Transactions", l["Menu:UtilitiesTransaction"], order: 820);
                utilitiesMenu.AddItem(transaction);


                if (await context.IsGrantedAsync(IndoPermissions.Utilities.CalendarTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.Calendar, displayName: l["Menu:UtilitiesTransactionCalendar"], url: "~/Calendar", order: 822));
                }

                if (await context.IsGrantedAsync(IndoPermissions.Utilities.BookingTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.Booking, displayName: l["Menu:UtilitiesTransactionBooking"], url: "~/Booking", order: 823));
                }

                if (await context.IsGrantedAsync(IndoPermissions.Utilities.TodoTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.Todo, displayName: l["Menu:UtilitiesTransactionTodo"], url: "~/Todo", order: 824));
                }

                if (await context.IsGrantedAsync(IndoPermissions.Utilities.DocumentTransaction))
                {
                    transaction.AddItem(new ApplicationMenuItem(name: IndoMenus.Document, displayName: l["Menu:UtilitiesTransactionDocument"], url: "~/Document", order: 825));
                }

                var report = new ApplicationMenuItem("Reports", l["Menu:UtilitiesReport"], order: 850);
                utilitiesMenu.AddItem(report);

                if (await context.IsGrantedAsync(IndoPermissions.Utilities.ExpenseReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.ExpenseReport, displayName: l["Menu:UtilitiesReportExpenseReport"], url: "~/ExpenseReport", order: 8501));
                }

                if (await context.IsGrantedAsync(IndoPermissions.Utilities.BookingReport))
                {
                    report.AddItem(new ApplicationMenuItem(name: IndoMenus.BookingReport, displayName: l["Menu:UtilitiesReportBookingReport"], url: "~/BookingReport", order: 8502));
                }
            }

            if (await context.IsGrantedAsync(IndoPermissions.Settings.Default))
            {
                var settingsMenu = new ApplicationMenuItem("Settings", l["Menu:Settings"], icon: "icon-settings", order: 900);
                context.Menu.AddItem(settingsMenu);

                if (await context.IsGrantedAsync(IndoPermissions.Settings.CompanyMaster))
                {
                    settingsMenu.AddItem(new ApplicationMenuItem(name: IndoMenus.Company, displayName: l["Menu:SettingsCompany"], url: "~/Company", order: 901));
                }

                if (await context.IsGrantedAsync(IndoPermissions.Settings.CurrencyMaster))
                {
                    settingsMenu.AddItem(new ApplicationMenuItem(name: IndoMenus.Currency, displayName: l["Menu:SettingsCurrency"], url: "~/Currency", order: 902));
                }

                if (await context.IsGrantedAsync(IndoPermissions.Settings.DepartmentMaster))
                {
                    settingsMenu.AddItem(new ApplicationMenuItem(name: IndoMenus.Department, displayName: l["Menu:SettingsDepartment"], url: "~/Department", order: 903));
                }
                if (await context.IsGrantedAsync(IndoPermissions.Settings.SkillMaster))
                {
                    settingsMenu.AddItem(new ApplicationMenuItem(name: IndoMenus.Skill, displayName: l["Menu:SettingsSkill"], url: "~/Skill", order: 903));
                }

                if (await context.IsGrantedAsync(IndoPermissions.Settings.EmployeeMaster))
                {
                    settingsMenu.AddItem(new ApplicationMenuItem(name: IndoMenus.Employee, displayName: l["Menu:SettingsEmployee"], url: "~/Employee", order: 904));
                }
                if (await context.IsGrantedAsync(IndoPermissions.Settings.AddEmployee))
                {
                    settingsMenu.AddItem(new ApplicationMenuItem(name: IndoMenus.AddEmployee, displayName: l["Menu:SettingsAddEmployee"], url: "~/Employee/Add", order: 908));
                }

                if (await context.IsGrantedAsync(IndoPermissions.Settings.NumberSequenceMaster))
                {
                    settingsMenu.AddItem(new ApplicationMenuItem(name: IndoMenus.NumberSequence, displayName: l["Menu:SettingsNumberSequence"], url: "~/NumberSequence", order: 905));
                }
            }


            //

            if (await context.IsGrantedAsync(IndoPermissions.Client.Default))
            {
                var clientsMenu = new ApplicationMenuItem("Client", l["Menu:Client"], icon: "icon-settings", order:8503);
                context.Menu.AddItem(clientsMenu);

                if (await context.IsGrantedAsync(IndoPermissions.Client.ClientsMaster))
                {
                    clientsMenu.AddItem(new ApplicationMenuItem(name: IndoMenus.Client, displayName: l["Menu:Client"], url: "~/Clients", order: 904));
                }
                if (await context.IsGrantedAsync(IndoPermissions.Client.AddClient))
                {
                   // clientsMenu.AddItem(new ApplicationMenuItem(name: IndoMenus.AddClient, displayName: l["Menu:AddClient"], url: "~/Clients/Add", order: 907));
                }
            }

            if (await context.IsGrantedAsync(IndoPermissions.Projectes.Default))
            {
                var projectsMenu = new ApplicationMenuItem("Projects", l["Menu:Projects"], icon: "icon-settings", order: 908);
                context.Menu.AddItem(projectsMenu);
                if (await context.IsGrantedAsync(IndoPermissions.Projectes.ProjectsMaster))
                {
                    projectsMenu.AddItem(new ApplicationMenuItem(name: IndoMenus.Projects, displayName: l["Menu:Projects"], url: "~/Project", order: 906));
                }
            }

            if (await context.IsGrantedAsync(IndoPermissions.Technologies.Default))
            {
                var technologyMenu = new ApplicationMenuItem("Technology", l["Menu:Technology"], icon: "icon-settings", order: 1001);
                context.Menu.AddItem(technologyMenu);
                if (await context.IsGrantedAsync(IndoPermissions.Technologies.TechnologyMaster))
                {
                    technologyMenu.AddItem(new ApplicationMenuItem(name: IndoMenus.Technology, displayName: l["Menu:Technology"], url: "~/Technologies", order: 1002));
                }
            }

            if (await context.IsGrantedAsync(IndoPermissions.EmailInformation.Default))
            {
                var emailInformationMenu = new ApplicationMenuItem("EmailInformation", l["Menu:EmailInformation"], icon: "icon-settings", order: 1005);
                context.Menu.AddItem(emailInformationMenu);
                if (await context.IsGrantedAsync(IndoPermissions.EmailInformation.EmailInformationMaster))
                {
                    emailInformationMenu.AddItem(new ApplicationMenuItem(name: IndoMenus.EmailInformation, displayName: l["Menu:EmailInformation"], url: "~/EmailInformation", order: 1006));
                }
            }
            
            if (await context.IsGrantedAsync(IndoPermissions.Leads.Default))
            {
                var leadsInformationMenu = new ApplicationMenuItem("Leads", l["Menu:Leads"], icon: "icon-profile", order: 1007);
                context.Menu.AddItem(leadsInformationMenu);
                if (await context.IsGrantedAsync(IndoPermissions.Leads.LeadsMaster))
                {
                    leadsInformationMenu.AddItem(new ApplicationMenuItem(name: IndoMenus.Leads, displayName: l["Menu:Leads"], url: "~/Leads", order: 1008));
                }
            }


            if (MultiTenancyConsts.IsEnabled)
            {
                administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 951);
            }
            else
            {
                administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
            }

            administration.Icon = "icon-life-buoy";
            administration.SetSubItemOrder(IdentityMenuNames.GroupName, 953);
            administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 954);

           




        }
    }
}
