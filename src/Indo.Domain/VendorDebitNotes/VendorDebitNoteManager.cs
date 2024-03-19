using System;
using System.Linq;
using System.Threading.Tasks;
using Indo.CashAndBanks;
using Indo.NumberSequences;
using Indo.VendorBills;
using Indo.VendorDebitNoteDetails;
using Indo.VendorPayments;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.VendorDebitNotes
{
    public class VendorDebitNoteManager : DomainService
    {
        private readonly IVendorDebitNoteRepository _vendorDebitNoteRepository;
        private readonly IVendorDebitNoteDetailRepository _vendorDebitNoteDetailRepository;
        private readonly ICashAndBankRepository _cashAndBankRepository;
        private readonly NumberSequenceManager _numberSequenceManager;
        private readonly IVendorPaymentRepository _vendorPaymentRepository;
        private readonly VendorPaymentManager _vendorPaymentManager;

        public VendorDebitNoteManager(
            IVendorDebitNoteRepository vendorDebitNoteRepository,
            IVendorDebitNoteDetailRepository vendorDebitNoteDetailRepository,
            ICashAndBankRepository cashAndBankRepository,
            NumberSequenceManager numberSequenceManager,
            IVendorPaymentRepository vendorPaymentRepository,
            VendorPaymentManager vendorPaymentManager
            )
        {
            _vendorDebitNoteRepository = vendorDebitNoteRepository;
            _vendorDebitNoteDetailRepository = vendorDebitNoteDetailRepository;
            _cashAndBankRepository = cashAndBankRepository;
            _numberSequenceManager = numberSequenceManager;
            _vendorPaymentRepository = vendorPaymentRepository;
            _vendorPaymentManager = vendorPaymentManager;
        }
        public async Task<VendorDebitNote> CreateAsync(
            [NotNull] string number,
            [NotNull] Guid vendorId,
            [NotNull] DateTime debitNoteDate,
            [NotNull] Guid vendorBillId
            )
        {
            Check.NotNullOrWhiteSpace(number, nameof(number));
            Check.NotNull<Guid>(vendorId, nameof(vendorId));
            Check.NotNull<DateTime>(debitNoteDate, nameof(debitNoteDate));
            Check.NotNull<Guid>(vendorBillId, nameof(vendorBillId));

            var existing = await _vendorDebitNoteRepository.FindAsync(x => x.Number.Equals(number));
            if (existing != null)
            {
                throw new VendorDebitNoteAlreadyExistsException(number);
            }

            return new VendorDebitNote(
                GuidGenerator.Create(),
                number,
                vendorId,
                debitNoteDate,
                vendorBillId
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] VendorDebitNote vendorDebitNote,
           [NotNull] string newName)
        {
            Check.NotNull(vendorDebitNote, nameof(vendorDebitNote));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _vendorDebitNoteRepository.FindAsync(x => x.Number.Equals(newName));
            if (existing != null && existing.Id != vendorDebitNote.Id)
            {
                throw new VendorDebitNoteAlreadyExistsException(newName);
            }

            vendorDebitNote.ChangeName(newName);
        }

        public async Task<VendorDebitNote> ConfirmAsync(Guid vendorDebitNoteId)
        {
            var obj = await _vendorDebitNoteRepository.GetAsync(vendorDebitNoteId);
            if (obj.Status == VendorDebitNoteStatus.Draft)
            {
                obj.Status = VendorDebitNoteStatus.Confirm;
                return await _vendorDebitNoteRepository.UpdateAsync(obj, true);
            }
            else
            {
                throw new UserFriendlyException("Only Draft can be confirm!");
            }
        }

        public async Task<VendorDebitNote> CancelAsync(Guid vendorDebitNoteId)
        {
            var obj = await _vendorDebitNoteRepository.GetAsync(vendorDebitNoteId);
            if (obj.Status == VendorDebitNoteStatus.Confirm)
            {
                obj.Status = VendorDebitNoteStatus.Cancelled;
                return await _vendorDebitNoteRepository.UpdateAsync(obj, true);
            }
            else
            {
                throw new UserFriendlyException("Only Confirm can be cancelled!");
            }
        }

        public async Task<VendorPayment> GeneratePayment(Guid vendorDebitNoteId)
        {
            try
            {
                var dn = await _vendorDebitNoteRepository.GetAsync(vendorDebitNoteId);
                if (dn.Status == VendorDebitNoteStatus.Confirm)
                {

                    var queryable = await _vendorDebitNoteDetailRepository.GetQueryableAsync();
                    var query = from vendorBillDetail in queryable
                                where vendorBillDetail.VendorDebitNoteId == dn.Id
                                select new { vendorBillDetail };
                    var queryResult = await AsyncExecuter.ToListAsync(query);
                    var total = queryResult.Sum(x => x.vendorBillDetail.Total);

                    var bank = _cashAndBankRepository.FirstOrDefault();

                    var payment = await _vendorPaymentManager.CreateAsync(
                            await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.BillPayment),
                            DateTime.Now,
                            bank.Id,
                            dn.VendorId,
                            total,
                            dn.Number,
                            dn.Id,
                            NumberSequenceModules.DebitNote
                        );

                    var result = await _vendorPaymentRepository.InsertAsync(payment);

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
