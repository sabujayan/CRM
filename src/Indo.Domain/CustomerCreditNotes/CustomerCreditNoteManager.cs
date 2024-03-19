using System;
using System.Linq;
using System.Threading.Tasks;
using Indo.CashAndBanks;
using Indo.CustomerCreditNoteDetails;
using Indo.CustomerPayments;
using Indo.NumberSequences;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.CustomerCreditNotes
{
    public class CustomerCreditNoteManager : DomainService
    {
        private readonly ICustomerCreditNoteRepository _customerCreditNoteRepository;
        private readonly ICustomerCreditNoteDetailRepository _customerCreditNoteDetailRepository;
        private readonly NumberSequenceManager _numberSequenceManager;
        private readonly ICashAndBankRepository _cashAndBankRepository;
        private readonly ICustomerPaymentRepository _customerPaymentRepository;
        private readonly CustomerPaymentManager _customerPaymentManager;

        public CustomerCreditNoteManager(
            ICustomerCreditNoteRepository customerCreditNoteRepository,
            ICustomerCreditNoteDetailRepository customerCreditNoteDetailRepository,
            NumberSequenceManager numberSequenceManager,
            ICashAndBankRepository cashAndBankRepository,
            ICustomerPaymentRepository customerPaymentRepository,
            CustomerPaymentManager customerPaymentManager
            )
        {
            _customerCreditNoteRepository = customerCreditNoteRepository;
            _customerCreditNoteDetailRepository = customerCreditNoteDetailRepository;
            _numberSequenceManager = numberSequenceManager;
            _cashAndBankRepository = cashAndBankRepository;
            _customerPaymentRepository = customerPaymentRepository;
            _customerPaymentManager = customerPaymentManager;
        }
        public async Task<CustomerCreditNote> CreateAsync(
            [NotNull] string number,
            [NotNull] Guid customerId,
            [NotNull] DateTime creditNoteDate,
            [NotNull] Guid customerCreditNoteId
            )
        {
            Check.NotNullOrWhiteSpace(number, nameof(number));
            Check.NotNull<Guid>(customerId, nameof(customerId));
            Check.NotNull<DateTime>(creditNoteDate, nameof(creditNoteDate));
            Check.NotNull<Guid>(customerCreditNoteId, nameof(customerCreditNoteId));

            var existing = await _customerCreditNoteRepository.FindAsync(x => x.Number.Equals(number));
            if (existing != null)
            {
                throw new CustomerCreditNoteAlreadyExistsException(number);
            }

            return new CustomerCreditNote(
                GuidGenerator.Create(),
                number,
                customerId,
                creditNoteDate,
                customerCreditNoteId
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] CustomerCreditNote customerCreditNote,
           [NotNull] string newName)
        {
            Check.NotNull(customerCreditNote, nameof(customerCreditNote));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _customerCreditNoteRepository.FindAsync(x => x.Number.Equals(newName));
            if (existing != null && existing.Id != customerCreditNote.Id)
            {
                throw new CustomerCreditNoteAlreadyExistsException(newName);
            }

            customerCreditNote.ChangeName(newName);
        }

        public async Task<CustomerCreditNote> ConfirmAsync(Guid customerCreditNoteId)
        {
            var obj = await _customerCreditNoteRepository.GetAsync(customerCreditNoteId);
            if (obj.Status == CustomerCreditNoteStatus.Draft)
            {
                obj.Status = CustomerCreditNoteStatus.Confirm;
                return await _customerCreditNoteRepository.UpdateAsync(obj, true);
            }
            else
            {
                throw new UserFriendlyException("Only Draft can be confirm!");
            }
        }

        public async Task<CustomerCreditNote> CancelAsync(Guid customerCreditNoteId)
        {
            var obj = await _customerCreditNoteRepository.GetAsync(customerCreditNoteId);
            if (obj.Status == CustomerCreditNoteStatus.Confirm)
            {
                obj.Status = CustomerCreditNoteStatus.Cancelled;
                return await _customerCreditNoteRepository.UpdateAsync(obj, true);
            }
            else
            {
                throw new UserFriendlyException("Only Confirm can be cancelled!");
            }
        }

        public async Task<CustomerPayment> GeneratePayment(Guid customerCreditNoteId)
        {
            try
            {
                var cn = await _customerCreditNoteRepository.GetAsync(customerCreditNoteId);
                if (cn.Status == CustomerCreditNoteStatus.Confirm)
                {

                    var queryable = await _customerCreditNoteDetailRepository.GetQueryableAsync();
                    var query = from customerCreditNoteDetail in queryable
                                where customerCreditNoteDetail.CustomerCreditNoteId == cn.Id
                                select new { customerCreditNoteDetail };
                    var queryResult = await AsyncExecuter.ToListAsync(query);
                    var total = queryResult.Sum(x => x.customerCreditNoteDetail.Total);

                    var bank = _cashAndBankRepository.FirstOrDefault();

                    var payment = await _customerPaymentManager.CreateAsync(
                            await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.InvoicePayment),
                            DateTime.Now,
                            bank.Id,
                            cn.CustomerId,
                            total,
                            cn.Number,
                            cn.Id,
                            NumberSequenceModules.CreditNote
                        );

                    var result = await _customerPaymentRepository.InsertAsync(payment);

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
