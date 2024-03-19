using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.CustomerPayments
{
    public interface ICustomerPaymentAppService : IApplicationService
    {
        Task<CustomerPaymentReadDto> GetAsync(Guid id);

        Task<List<CustomerPaymentReadDto>> GetListAsync();

        Task<List<CustomerPaymentReadDto>> GetListByCustomerAsync(Guid customerId);

        Task<List<CustomerPaymentReadDto>> GetListByInvoiceAsync(Guid customerInvoiceId);

        Task<List<CustomerPaymentReadDto>> GetListByCreditNoteAsync(Guid customerCreditNoteId);

        Task<CustomerPaymentReadDto> CreateAsync(CustomerPaymentCreateDto input);

        Task UpdateAsync(Guid id, CustomerPaymentUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<CashAndBankLookupDto>> GetCashAndBankLookupAsync();

        Task<ListResultDto<CustomerLookupDto>> GetCustomerLookupAsync();

        Task<CustomerPaymentReadDto> ConfirmAsync(Guid customerPaymentId);

        Task<CustomerPaymentReadDto> CancelAsync(Guid customerPaymentId);
    }
}
