using System;
using System.Threading.Tasks;
using Indo.NumberSequences;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.CustomerPayments
{
    public class CustomerPaymentManager : DomainService
    {
        private readonly ICustomerPaymentRepository _customerPaymentRepository;

        public CustomerPaymentManager(ICustomerPaymentRepository customerPaymentRepository)
        {
            _customerPaymentRepository = customerPaymentRepository;
        }
        public async Task<CustomerPayment> CreateAsync(
            [NotNull] string number,
            [NotNull] DateTime paymentDate,
            [NotNull] Guid cashAndBankId,
            [NotNull] Guid customerId,
            [NotNull] float amount,
            [NotNull] string sourceDocument,
            [NotNull] Guid sourceDocumentId,
            [NotNull] NumberSequenceModules sourceDocumentModule
            )
        {
            Check.NotNullOrWhiteSpace(number, nameof(number));
            Check.NotNull(paymentDate, nameof(paymentDate));
            Check.NotNull(cashAndBankId, nameof(cashAndBankId));
            Check.NotNull(customerId, nameof(customerId));
            Check.NotNull(amount, nameof(amount));
            Check.NotNullOrWhiteSpace(sourceDocument, nameof(sourceDocument));
            Check.NotNull(sourceDocumentId, nameof(sourceDocumentId));
            Check.NotNull(sourceDocumentModule, nameof(sourceDocumentModule));

            var existing = await _customerPaymentRepository.FindAsync(x => x.Number.Equals(number));
            if (existing != null)
            {
                throw new CustomerPaymentAlreadyExistsException(number);
            }

            return new CustomerPayment(
                GuidGenerator.Create(),
                number,
                paymentDate,
                cashAndBankId,
                customerId,
                amount,
                sourceDocument,
                sourceDocumentId,
                sourceDocumentModule
            );
        }
        public async Task ChangeNumberAsync(
           [NotNull] CustomerPayment customerPayment,
           [NotNull] string newNumber)
        {
            Check.NotNull(customerPayment, nameof(customerPayment));
            Check.NotNullOrWhiteSpace(newNumber, nameof(newNumber));

            var existing = await _customerPaymentRepository.FindAsync(x => x.Number.Equals(newNumber));
            if (existing != null && existing.Id != customerPayment.Id)
            {
                throw new CustomerPaymentAlreadyExistsException(newNumber);
            }

            customerPayment.ChangeNumber(newNumber);
        }

        public async Task<CustomerPayment> ConfirmAsync(Guid customerPaymentId)
        {
            var obj = await _customerPaymentRepository.GetAsync(customerPaymentId);
            if (obj.Status == CustomerPaymentStatus.Draft)
            {
                obj.Status = CustomerPaymentStatus.Confirm;
                return await _customerPaymentRepository.UpdateAsync(obj, true);
            }
            else
            {
                throw new UserFriendlyException("Only Draft can be confirm!");
            }
        }

        public async Task<CustomerPayment> CancelAsync(Guid customerPaymentId)
        {
            var obj = await _customerPaymentRepository.GetAsync(customerPaymentId);
            if (obj.Status == CustomerPaymentStatus.Confirm)
            {
                obj.Status = CustomerPaymentStatus.Cancelled;
                return await _customerPaymentRepository.UpdateAsync(obj, true);
            }
            else
            {
                throw new UserFriendlyException("Only Confirm can be cancelled!");
            }
        }
    }
}
