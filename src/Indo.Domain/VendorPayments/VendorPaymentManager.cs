using System;
using System.Threading.Tasks;
using Indo.NumberSequences;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.VendorPayments
{
    public class VendorPaymentManager : DomainService
    {
        private readonly IVendorPaymentRepository _vendorPaymentRepository;

        public VendorPaymentManager(IVendorPaymentRepository vendorPaymentRepository)
        {
            _vendorPaymentRepository = vendorPaymentRepository;
        }
        public async Task<VendorPayment> CreateAsync(
            [NotNull] string number,
            [NotNull] DateTime paymentDate,
            [NotNull] Guid cashAndBankId,
            [NotNull] Guid vendorId,
            [NotNull] float amount,
            [NotNull] string sourceDocument,
            [NotNull] Guid sourceDocumentId,
            [NotNull] NumberSequenceModules sourceDocumentModule
            )
        {
            Check.NotNullOrWhiteSpace(number, nameof(number));
            Check.NotNull(paymentDate, nameof(paymentDate));
            Check.NotNull(cashAndBankId, nameof(cashAndBankId));
            Check.NotNull(vendorId, nameof(vendorId));
            Check.NotNull(amount, nameof(amount));
            Check.NotNullOrWhiteSpace(sourceDocument, nameof(sourceDocument));
            Check.NotNull(sourceDocumentId, nameof(sourceDocumentId));
            Check.NotNull(sourceDocumentModule, nameof(sourceDocumentModule));

            var existing = await _vendorPaymentRepository.FindAsync(x => x.Number.Equals(number));
            if (existing != null)
            {
                throw new VendorPaymentAlreadyExistsException(number);
            }

            return new VendorPayment(
                GuidGenerator.Create(),
                number,
                paymentDate,
                cashAndBankId,
                vendorId,
                amount,
                sourceDocument,
                sourceDocumentId,
                sourceDocumentModule
            );
        }
        public async Task ChangeNumberAsync(
           [NotNull] VendorPayment vendorPayment,
           [NotNull] string newNumber)
        {
            Check.NotNull(vendorPayment, nameof(vendorPayment));
            Check.NotNullOrWhiteSpace(newNumber, nameof(newNumber));

            var existing = await _vendorPaymentRepository.FindAsync(x => x.Number.Equals(newNumber));
            if (existing != null && existing.Id != vendorPayment.Id)
            {
                throw new VendorPaymentAlreadyExistsException(newNumber);
            }

            vendorPayment.ChangeNumber(newNumber);
        }

        public async Task<VendorPayment> ConfirmAsync(Guid vendorPaymentId)
        {
            var obj = await _vendorPaymentRepository.GetAsync(vendorPaymentId);
            if (obj.Status == VendorPaymentStatus.Draft)
            {
                obj.Status = VendorPaymentStatus.Confirm;
                return await _vendorPaymentRepository.UpdateAsync(obj, true);
            }
            else
            {
                throw new UserFriendlyException("Only Draft can be confirm!");
            }
        }

        public async Task<VendorPayment> CancelAsync(Guid vendorPaymentId)
        {
            var obj = await _vendorPaymentRepository.GetAsync(vendorPaymentId);
            if (obj.Status == VendorPaymentStatus.Confirm)
            {
                obj.Status = VendorPaymentStatus.Cancelled;
                return await _vendorPaymentRepository.UpdateAsync(obj, true);
            }
            else
            {
                throw new UserFriendlyException("Only Confirm can be cancelled!");
            }
        }
    }
}
