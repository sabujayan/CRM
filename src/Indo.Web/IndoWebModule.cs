using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Indo.EntityFrameworkCore;
using Indo.Localization;
using Indo.MultiTenancy;
using Indo.Web.Menus;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Indo.Abp.AspNetCore.Mvc.UI.Theme.Gojazz;
using Indo.Abp.AspNetCore.Mvc.UI.Theme.Gojazz.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity.Web;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Web;
using Volo.Abp.SettingManagement.Web;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement.Web;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.UI;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Indo.Permissions;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace Indo.Web
{
    [DependsOn(
        typeof(IndoHttpApiModule),
        typeof(IndoApplicationModule),
        typeof(IndoEntityFrameworkCoreDbMigrationsModule),
        typeof(AbpAutofacModule),
        typeof(AbpIdentityWebModule),
        typeof(AbpSettingManagementWebModule),
        typeof(AbpAccountWebIdentityServerModule),
        typeof(AbpAspNetCoreMvcUIGojazzThemeModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(AbpTenantManagementWebModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpSwashbuckleModule)
        )]
    public class IndoWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(IndoResource),
                    typeof(IndoDomainModule).Assembly,
                    typeof(IndoDomainSharedModule).Assembly,
                    typeof(IndoApplicationModule).Assembly,
                    typeof(IndoApplicationContractsModule).Assembly,
                    typeof(IndoWebModule).Assembly
                );
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            ConfigureUrls(configuration);
            ConfigureBundles();
            ConfigureAuthentication(context, configuration);
            ConfigureAutoMapper();
            ConfigureVirtualFileSystem(hostingEnvironment);
            ConfigureLocalizationServices();
            ConfigureNavigationServices();
            ConfigureAutoApiControllers();
            ConfigureSwaggerServices(context.Services);



            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/Dashboard/Index", IndoPermissions.Dashboard.Crm);
                options.Conventions.AuthorizePage("/DashboardInventory/Index", IndoPermissions.Dashboard.Inventory);
            });
            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/Activity/Index", IndoPermissions.CustomerManagement.ActivityMaster);
                options.Conventions.AuthorizePage("/LeadSource/Index", IndoPermissions.CustomerManagement.LeadSourceMaster);
                options.Conventions.AuthorizePage("/LeadRating/Index", IndoPermissions.CustomerManagement.LeadRatingMaster);
                options.Conventions.AuthorizePage("/ExpenseType/Index", IndoPermissions.CustomerManagement.ExpenseTypeMaster);
                options.Conventions.AuthorizePage("/Lead/Index", IndoPermissions.CustomerManagement.LeadTransaction);
                options.Conventions.AuthorizePage("/Customer/Index", IndoPermissions.CustomerManagement.CustomerTransaction);
                options.Conventions.AuthorizePage("/Contact/Index", IndoPermissions.CustomerManagement.ContactTransaction);
                options.Conventions.AuthorizePage("/Note/Index", IndoPermissions.CustomerManagement.NoteTransaction);
                options.Conventions.AuthorizePage("/ServiceQuotation/Index", IndoPermissions.CustomerManagement.ServiceQuotationTransaction);
                options.Conventions.AuthorizePage("/SalesQuotation/Index", IndoPermissions.CustomerManagement.SalesQuotationTransaction);
                options.Conventions.AuthorizePage("/Expense/Index", IndoPermissions.CustomerManagement.ExpenseTransaction);
                options.Conventions.AuthorizePage("/Task/Index", IndoPermissions.CustomerManagement.TaskTransaction);
                options.Conventions.AuthorizePage("/ImportantDate/Index", IndoPermissions.CustomerManagement.ImportantDateTransaction);
            });
            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/ProjectOrder/Index", IndoPermissions.Project.ProjectOrderTransaction);
                options.Conventions.AuthorizePage("/ProjectOrderReport/Index", IndoPermissions.Project.ProjectOrderReport);
                options.Conventions.AuthorizePage("/ProjectOrderByCustomerReport/Index", IndoPermissions.Project.ProjectOrderByCustomerReport);
                options.Conventions.AuthorizePage("/ProjectOrderBySalesExecutiveReport/Index", IndoPermissions.Project.ProjectOrderBySalesExecutiveReport);
            });
            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/Service/Index", IndoPermissions.Services.ServiceMaster);
                options.Conventions.AuthorizePage("/ServiceOrder/Index", IndoPermissions.Services.ServiceOrderTransaction);
                options.Conventions.AuthorizePage("/ServiceOrderReport/Index", IndoPermissions.Services.ServiceOrderReport);
                options.Conventions.AuthorizePage("/ServiceOrderByCustomerReport/Index", IndoPermissions.Services.ServiceOrderByCustomerReport);
                options.Conventions.AuthorizePage("/ServiceOrderBySalesExecutiveReport/Index", IndoPermissions.Services.ServiceOrderBySalesExecutiveReport);
            });
            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/Buyer/Index", IndoPermissions.Purchase.BuyerMaster);
                options.Conventions.AuthorizePage("/Vendor/Index", IndoPermissions.Purchase.VendorMaster);
                options.Conventions.AuthorizePage("/PurchaseOrder/Index", IndoPermissions.Purchase.PurchaseOrderTransaction);
                options.Conventions.AuthorizePage("/PurchaseReceipt/Index", IndoPermissions.Purchase.PurchaseReceiptTransaction);
                options.Conventions.AuthorizePage("/PurchaseOrderReport/Index", IndoPermissions.Purchase.PurchaseOrderReport);
                options.Conventions.AuthorizePage("/PurchaseOrderByVendorReport/Index", IndoPermissions.Purchase.PurchaseOrderByVendorReport);
                options.Conventions.AuthorizePage("/PurchaseOrderByBuyerReport/Index", IndoPermissions.Purchase.PurchaseOrderByBuyerReport);
                options.Conventions.AuthorizePage("/PurchaseReceiptReport/Index", IndoPermissions.Purchase.PurchaseReceiptReport);
            });
            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/SalesExecutive/Index", IndoPermissions.Sales.SalesExecutiveMaster);
                options.Conventions.AuthorizePage("/SalesOrder/Index", IndoPermissions.Sales.SalesOrderTransaction);
                options.Conventions.AuthorizePage("/SalesDelivery/Index", IndoPermissions.Sales.SalesDeliveryTransaction);
                options.Conventions.AuthorizePage("/SalesOrderReport/Index", IndoPermissions.Sales.SalesOrderReport);
                options.Conventions.AuthorizePage("/SalesOrderByCustomerReport/Index", IndoPermissions.Sales.SalesOrderByCustomerReport);
                options.Conventions.AuthorizePage("/SalesOrderBySalesExecutiveReport/Index", IndoPermissions.Sales.SalesOrderBySalesExecutiveReport);
                options.Conventions.AuthorizePage("/SalesDeliveryReport/Index", IndoPermissions.Sales.SalesDeliveryReport);
            });
            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/CashAndBank/Index", IndoPermissions.Finance.CashAndBankMaster);
                options.Conventions.AuthorizePage("/VendorBill/Index", IndoPermissions.Finance.VendorBillTransaction);
                options.Conventions.AuthorizePage("/VendorDebitNote/Index", IndoPermissions.Finance.VendorDebitNoteTransaction);
                options.Conventions.AuthorizePage("/VendorPayment/Index", IndoPermissions.Finance.VendorPaymentTransaction);
                options.Conventions.AuthorizePage("/CustomerInvoice/Index", IndoPermissions.Finance.CustomerInvoiceTransaction);
                options.Conventions.AuthorizePage("/CustomerCreditNote/Index", IndoPermissions.Finance.CustomerCreditNoteTransaction);
                options.Conventions.AuthorizePage("/CustomerPayment/Index", IndoPermissions.Finance.CustomerPaymentTransaction);
                options.Conventions.AuthorizePage("/VendorBillReport/Index", IndoPermissions.Finance.VendorBillReport);
                options.Conventions.AuthorizePage("/VendorDebitNoteReport/Index", IndoPermissions.Finance.VendorDebitNoteReport);
                options.Conventions.AuthorizePage("/VendorPaymentReport/Index", IndoPermissions.Finance.VendorPaymentReport);
                options.Conventions.AuthorizePage("/CustomerInvoiceReport/Index", IndoPermissions.Finance.CustomerInvoiceReport);
                options.Conventions.AuthorizePage("/CustomerCreditNoteReport/Index", IndoPermissions.Finance.CustomerCreditNoteReport);
                options.Conventions.AuthorizePage("/CustomerPaymentReport/Index", IndoPermissions.Finance.CustomerPaymentReport);
                options.Conventions.AuthorizePage("/CashAndBankReport/Index", IndoPermissions.Finance.CashAndBankReport);
            });
            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/TransferOrder/Index", IndoPermissions.Transfer.InterWarehouseTransferTransaction);
                options.Conventions.AuthorizePage("/DeliveryOrder/Index", IndoPermissions.Transfer.DeliveryOrderTransaction);
                options.Conventions.AuthorizePage("/GoodsReceipt/Index", IndoPermissions.Transfer.GoodsReceiptTransaction);
                options.Conventions.AuthorizePage("/DeliveryOrderReport/Index", IndoPermissions.Transfer.DeliveryOrderReport);
                options.Conventions.AuthorizePage("/GoodsReceiptReport/Index", IndoPermissions.Transfer.GoodsReceiptReport);
            });
            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/Uom/Index", IndoPermissions.Inventory.UomMaster);
                options.Conventions.AuthorizePage("/Product/Index", IndoPermissions.Inventory.ProductMaster);
                options.Conventions.AuthorizePage("/Warehouse/Index", IndoPermissions.Inventory.WarehouseMaster);
                options.Conventions.AuthorizePage("/StockAdjustment/Index", IndoPermissions.Inventory.AdjustmentTransaction);
                options.Conventions.AuthorizePage("/Movement/Index", IndoPermissions.Inventory.MovementReport);
                options.Conventions.AuthorizePage("/Stock/Index", IndoPermissions.Inventory.StockReport);
                options.Conventions.AuthorizePage("/StockAdjustmentReport/Index", IndoPermissions.Inventory.AdjustmentReport);
            });
            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/Resource/Index", IndoPermissions.Utilities.ResourceMaster);
                options.Conventions.AuthorizePage("/Booking/Index", IndoPermissions.Utilities.BookingTransaction);
                options.Conventions.AuthorizePage("/Calendar/Index", IndoPermissions.Utilities.CalendarTransaction);
                options.Conventions.AuthorizePage("/Todo/Index", IndoPermissions.Utilities.TodoTransaction);
                options.Conventions.AuthorizePage("/ExpenseReport/Index", IndoPermissions.Utilities.ExpenseReport);
                options.Conventions.AuthorizePage("/BookingReport/Index", IndoPermissions.Utilities.BookingReport);
            });
            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/Company/Index", IndoPermissions.Settings.CompanyMaster);
                options.Conventions.AuthorizePage("/Currency/Index", IndoPermissions.Settings.CurrencyMaster);
                options.Conventions.AuthorizePage("/NumberSequence/Index", IndoPermissions.Settings.NumberSequenceMaster);
            });
           
        }

        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            });
        }

        private void ConfigureBundles()
        {
            Configure<AbpBundlingOptions>(options =>
            {
                options.StyleBundles.Configure(
                    GojazzThemeBundles.Styles.Global,
                    bundle =>
                    {
                        bundle.AddFiles("/global-styles.css");
                    }
                );
                options.ScriptBundles.Configure(
                    GojazzThemeBundles.Scripts.Global,
                    bundle =>
                    {
                        bundle.AddFiles("/global-scripts.js");
                    }
                );
            });
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                    options.Audience = "Indo";
                });
        }

        private void ConfigureAutoMapper()
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<IndoWebModule>();
            });
        }

        private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<IndoDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Indo.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<IndoDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Indo.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<IndoApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Indo.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<IndoApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Indo.Application"));
                    options.FileSets.ReplaceEmbeddedByPhysical<IndoWebModule>(hostingEnvironment.ContentRootPath);
                });
            }
        }

        private void ConfigureLocalizationServices()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("ar", "ar", "العربية"));
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português"));
                options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
                options.Languages.Add(new LanguageInfo("de-DE", "de-DE", "Deutsch", "de"));
                options.Languages.Add(new LanguageInfo("es", "es", "Español"));
                options.Languages.Add(new LanguageInfo("id", "id", "Bahasa"));
            });
        }

        private void ConfigureNavigationServices()
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new IndoMenuContributor());
            });
        }

        private void ConfigureAutoApiControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(IndoApplicationModule).Assembly);
            });
        }

        private void ConfigureSwaggerServices(IServiceCollection services)
        {
            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Indo API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                }
            );
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {

            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseAbpRequestLocalization();


            if (!env.IsDevelopment())
            {
                app.UseErrorPage();
            }

            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseJwtTokenMiddleware();

            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }


            app.UseUnitOfWork();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Indo API");
            });


            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();
        }
    }
}
