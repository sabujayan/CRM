using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Indo.CashAndBanks;
using Indo.CustomerCreditNoteDetails;
using Indo.CustomerCreditNotes;
using Indo.CustomerInvoiceDetails;
using Indo.CustomerPayments;
using Indo.NumberSequences;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.CustomerInvoices
{
    public class CustomerInvoiceManager : DomainService
    {
        private readonly ICustomerInvoiceRepository _customerInvoiceRepository;
        private readonly ICustomerInvoiceDetailRepository _customerInvoiceDetailRepository;
        private readonly NumberSequenceManager _numberSequenceManager;
        private readonly ICustomerCreditNoteRepository _customerCreditNoteRepository;
        private readonly CustomerCreditNoteManager _customerCreditNoteManager;
        private readonly ICustomerCreditNoteDetailRepository _customerCreditNoteDetailRepository;
        private readonly CustomerCreditNoteDetailManager _customerCreditNoteDetailManager;
        private readonly ICashAndBankRepository _cashAndBankRepository;
        private readonly ICustomerPaymentRepository _customerPaymentRepository;
        private readonly CustomerPaymentManager _customerPaymentManager;

        public CustomerInvoiceManager(
            ICustomerInvoiceRepository customerInvoiceRepository,
            ICustomerInvoiceDetailRepository customerInvoiceDetailRepository,
            NumberSequenceManager numberSequenceManager,
            ICustomerCreditNoteRepository customerCreditNoteRepository,
            CustomerCreditNoteManager customerCreditNoteManager,
            ICustomerCreditNoteDetailRepository customerCreditNoteDetailRepository,
            CustomerCreditNoteDetailManager customerCreditNoteDetailManager,
            ICashAndBankRepository cashAndBankRepository,
            ICustomerPaymentRepository customerPaymentRepository,
            CustomerPaymentManager customerPaymentManager
            )
        {
            _customerInvoiceRepository = customerInvoiceRepository;
            _customerInvoiceDetailRepository = customerInvoiceDetailRepository;
            _numberSequenceManager = numberSequenceManager;
            _customerCreditNoteRepository = customerCreditNoteRepository;
            _customerCreditNoteManager = customerCreditNoteManager;
            _customerCreditNoteDetailRepository = customerCreditNoteDetailRepository;
            _customerCreditNoteDetailManager = customerCreditNoteDetailManager;
            _cashAndBankRepository = cashAndBankRepository;
            _customerPaymentRepository = customerPaymentRepository;
            _customerPaymentManager = customerPaymentManager;
        }
        public async Task<CustomerInvoice> CreateAsync(
            [NotNull] string number,
            [NotNull] Guid customerId,
            [NotNull] DateTime invoiceDate,
            [NotNull] DateTime invoiceDueDate
            )
        {
            Check.NotNullOrWhiteSpace(number, nameof(number));
            Check.NotNull<Guid>(customerId, nameof(customerId));
            Check.NotNull<DateTime>(invoiceDate, nameof(invoiceDate));
            Check.NotNull<DateTime>(invoiceDueDate, nameof(invoiceDueDate));

            var existing = await _customerInvoiceRepository.FindAsync(x => x.Number.Equals(number));
            if (existing != null)
            {
                throw new CustomerInvoiceAlreadyExistsException(number);
            }

            return new CustomerInvoice(
                GuidGenerator.Create(),
                number,
                customerId,
                invoiceDate, 
                invoiceDueDate
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] CustomerInvoice customerInvoice,
           [NotNull] string newName)
        {
            Check.NotNull(customerInvoice, nameof(customerInvoice));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _customerInvoiceRepository.FindAsync(x => x.Number.Equals(newName));
            if (existing != null && existing.Id != customerInvoice.Id)
            {
                throw new CustomerInvoiceAlreadyExistsException(newName);
            }

            customerInvoice.ChangeName(newName);
        }

        public async Task<CustomerInvoice> ConfirmAsync(Guid customerInvoiceId)
        {
            var obj = await _customerInvoiceRepository.GetAsync(customerInvoiceId);
            if (obj.Status == CustomerInvoiceStatus.Draft)
            {
                obj.Status = CustomerInvoiceStatus.Confirm;
                return await _customerInvoiceRepository.UpdateAsync(obj, true);
            }
            else
            {
                throw new UserFriendlyException("Only Draft can be confirm!");
            }
        }

        public async Task<CustomerInvoice> CancelAsync(Guid customerInvoiceId)
        {
            var obj = await _customerInvoiceRepository.GetAsync(customerInvoiceId);
            if (obj.Status == CustomerInvoiceStatus.Confirm)
            {
                obj.Status = CustomerInvoiceStatus.Cancelled;
                return await _customerInvoiceRepository.UpdateAsync(obj, true);
            }
            else
            {
                throw new UserFriendlyException("Only Confirm can be cancelled!");
            }
        }

        public async Task<CustomerCreditNote> GenerateCreditNote(Guid customerInvoiceId)
        {
            try
            {
                var invoice = await _customerInvoiceRepository.GetAsync(customerInvoiceId);
                if (invoice.Status == CustomerInvoiceStatus.Confirm)
                {

                    var creditNote = await _customerCreditNoteManager.CreateAsync(
                            await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.CreditNote),
                            invoice.CustomerId,
                            DateTime.Now,
                            invoice.Id
                        );

                    var result = await _customerCreditNoteRepository.InsertAsync(creditNote);

                    var queryable = await _customerInvoiceDetailRepository.GetQueryableAsync();
                    var query = from customerInvoiceDetail in queryable
                                where customerInvoiceDetail.CustomerInvoiceId == invoice.Id
                                select new { customerInvoiceDetail };
                    var queryResult = await AsyncExecuter.ToListAsync(query);
                    var details = new List<CustomerCreditNoteDetail>();
                    foreach (var item in queryResult)
                    {
                        var detail = await _customerCreditNoteDetailManager.CreateAsync(
                                result.Id,
                                item.customerInvoiceDetail.ProductName,
                                item.customerInvoiceDetail.UomId,
                                item.customerInvoiceDetail.Price,
                                item.customerInvoiceDetail.TaxRate,
                                item.customerInvoiceDetail.Quantity,
                                item.customerInvoiceDetail.DiscAmt
                            );
                        details.Add(detail);

                    }
                    await _customerCreditNoteDetailRepository.InsertManyAsync(details);

                    return creditNote;
                }
                else
                {
                    throw new UserFriendlyException("Only Confirm can be processed!");
                }
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException("Error: " + ex.Message);
            }
        }

        public async Task<CustomerPayment> GeneratePayment(Guid customerInvoiceId)
        {
            try
            {
                var invoice = await _customerInvoiceRepository.GetAsync(customerInvoiceId);
                if (invoice.Status == CustomerInvoiceStatus.Confirm)
                {

                    var queryable = await _customerInvoiceDetailRepository.GetQueryableAsync();
                    var query = from customerInvoiceDetail in queryable
                                where customerInvoiceDetail.CustomerInvoiceId == invoice.Id
                                select new { customerInvoiceDetail };
                    var queryResult = await AsyncExecuter.ToListAsync(query);
                    var total = queryResult.Sum(x => x.customerInvoiceDetail.Total);

                    var bank = _cashAndBankRepository.FirstOrDefault();

                    var payment = await _customerPaymentManager.CreateAsync(
                            await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.InvoicePayment),
                            DateTime.Now,
                            bank.Id,
                            invoice.CustomerId,
                            total,
                            invoice.Number,
                            invoice.Id,
                            NumberSequenceModules.Invoice
                        );

                    var result = await _customerPaymentRepository.InsertAsync(payment, true);

                    return payment;
                }
                else
                {
                    throw new UserFriendlyException("Only Confirm can be processed!");
                }
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException("Error: " + ex.Message);
            }
        }


    }
}
