using System;
using Volo.Abp.Application.Dtos;

namespace Indo.CashAndBanks
{
    public class CashAndBankReportDto 
    {
        public Guid Id { get; set; }
        public string Period { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentNumber { get; set; }
        public string SourceDocument { get; set; }
        public string SourceDocumentModule { get; set; }
        public string SourceDocumentPath { get; set; }
        public Guid SourceDocumentId { get; set; }
        public string ThirdParty { get; set; }
        public string Status { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Currency { get; set; }
        public float Amount { get; set; }
    }
}
