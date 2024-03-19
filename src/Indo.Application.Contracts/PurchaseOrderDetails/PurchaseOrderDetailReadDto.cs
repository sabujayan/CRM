using Indo.PurchaseOrders;
using System;
using Volo.Abp.Application.Dtos;

namespace Indo.PurchaseOrderDetails
{
    public class PurchaseOrderDetailReadDto : AuditedEntityDto<Guid>
    {
        public Guid PurchaseOrderId { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string VendorName { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string UomName { get; set; }
        public float Price { get; set; }
        public string PriceString { get; set; }
        public string CurrencyName { get; set; }
        public float Quantity { get; set; }
        public float SubTotal { get; set; }
        public string SubTotalString { get; set; }
        public float DiscAmt { get; set; }
        public string DiscAmtString { get; set; }
        public float BeforeTax { get; set; }
        public string BeforeTaxString { get; set; }
        public float TaxRate { get; set; }        
        public float TaxAmount { get; set; }
        public string TaxAmountString { get; set; }
        public float Total { get; set; }
        public string TotalString { get; set; }
        public PurchaseOrderStatus Status { get; set; }
        public string StatusString { get; set; }
    }
}
