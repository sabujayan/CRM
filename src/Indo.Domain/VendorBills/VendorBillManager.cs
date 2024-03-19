using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Indo.CashAndBanks;
using Indo.NumberSequences;
using Indo.VendorBillDetails;
using Indo.VendorDebitNoteDetails;
using Indo.VendorDebitNotes;
using Indo.VendorPayments;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.VendorBills
{
    public class VendorBillManager : DomainService
    {
        private readonly IVendorBillRepository _vendorBillRepository;
        private readonly IVendorBillDetailRepository _vendorBillDetailRepository;
        private readonly NumberSequenceManager _numberSequenceManager;
        private readonly ICashAndBankRepository _cashAndBankRepository;
        private readonly IVendorPaymentRepository _vendorPaymentRepository;
        private readonly VendorPaymentManager _vendorPaymentManager;
        private readonly IVendorDebitNoteRepository _vendorDebitNoteRepository;
        private readonly VendorDebitNoteManager _vendorDebitNoteManager;
        private readonly IVendorDebitNoteDetailRepository _vendorDebitNoteDetailRepository;
        private readonly VendorDebitNoteDetailManager _vendorDebitNoteDetailManager;

        public VendorBillManager(
            IVendorBillRepository vendorBillRepository,
            IVendorBillDetailRepository vendorBillDetailRepository,
            NumberSequenceManager numberSequenceManager,
            ICashAndBankRepository cashAndBankRepository,
            IVendorPaymentRepository vendorPaymentRepository,
            VendorPaymentManager vendorPaymentManager,
            IVendorDebitNoteRepository vendorDebitNoteRepository,
            VendorDebitNoteManager vendorDebitNoteManager,
            IVendorDebitNoteDetailRepository vendorDebitNoteDetailRepository,
            VendorDebitNoteDetailManager vendorDebitNoteDetailManager

            )
        {
            _vendorBillRepository = vendorBillRepository;
            _vendorBillDetailRepository = vendorBillDetailRepository;
            _numberSequenceManager = numberSequenceManager;
            _cashAndBankRepository = cashAndBankRepository;
            _vendorPaymentRepository = vendorPaymentRepository;
            _vendorPaymentManager = vendorPaymentManager;
            _vendorDebitNoteRepository = vendorDebitNoteRepository;
            _vendorDebitNoteManager = vendorDebitNoteManager;
            _vendorDebitNoteDetailRepository = vendorDebitNoteDetailRepository;
            _vendorDebitNoteDetailManager = vendorDebitNoteDetailManager;
        }
        public async Task<VendorBill> CreateAsync(
            [NotNull] string number,
            [NotNull] Guid vendorId,
            [NotNull] DateTime billDate,
            [NotNull] DateTime billDueDate
            )
        {
            Check.NotNullOrWhiteSpace(number, nameof(number));
            Check.NotNull<Guid>(vendorId, nameof(vendorId));
            Check.NotNull<DateTime>(billDate, nameof(billDate));
            Check.NotNull<DateTime>(billDueDate, nameof(billDueDate));

            var existing = await _vendorBillRepository.FindAsync(x => x.Number.Equals(number));
            if (existing != null)
            {
                throw new VendorBillAlreadyExistsException(number);
            }

            return new VendorBill(
                GuidGenerator.Create(),
                number,
                vendorId,
                billDate, 
                billDueDate
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] VendorBill vendorBill,
           [NotNull] string newName)
        {
            Check.NotNull(vendorBill, nameof(vendorBill));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _vendorBillRepository.FindAsync(x => x.Number.Equals(newName));
            if (existing != null && existing.Id != vendorBill.Id)
            {
                throw new VendorBillAlreadyExistsException(newName);
            }

            vendorBill.ChangeName(newName);
        }

        public async Task<VendorBill> ConfirmAsync(Guid vendorBillId)
        {
            var obj = await _vendorBillRepository.GetAsync(vendorBillId);
            if (obj.Status == VendorBillStatus.Draft)
            {
                obj.Status = VendorBillStatus.Confirm;
                return await _vendorBillRepository.UpdateAsync(obj, true);
            }
            else
            {
                throw new UserFriendlyException("Only Draft can be confirm!");
            }
        }

        public async Task<VendorBill> CancelAsync(Guid vendorBillId)
        {
            var obj = await _vendorBillRepository.GetAsync(vendorBillId);
            if (obj.Status == VendorBillStatus.Confirm)
            {
                obj.Status = VendorBillStatus.Cancelled;
                return await _vendorBillRepository.UpdateAsync(obj, true);
            }
            else
            {
                throw new UserFriendlyException("Only Confirm can be cancelled!");
            }
        }


        public async Task<VendorDebitNote> GenerateDebitNote(Guid vendorBillId)
        {
            try
            {
                var bill = await _vendorBillRepository.GetAsync(vendorBillId);
                if (bill.Status == VendorBillStatus.Confirm)
                {

                    var creditNote = await _vendorDebitNoteManager.CreateAsync(
                            await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.DebitNote),
                            bill.VendorId,
                            DateTime.Now,
                            bill.Id
                        );

                    var result = await _vendorDebitNoteRepository.InsertAsync(creditNote);

                    var queryable = await _vendorBillDetailRepository.GetQueryableAsync();
                    var query = from customerInvoiceDetail in queryable
                                where customerInvoiceDetail.VendorBillId == bill.Id
                                select new { customerInvoiceDetail };
                    var queryResult = await AsyncExecuter.ToListAsync(query);
                    var details = new List<VendorDebitNoteDetail>();
                    foreach (var item in queryResult)
                    {
                        var detail = await _vendorDebitNoteDetailManager.CreateAsync(
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
                    await _vendorDebitNoteDetailRepository.InsertManyAsync(details);

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

        public async Task<VendorPayment> GeneratePayment(Guid vendorBillId)
        {
            try
            {
                var bill = await _vendorBillRepository.GetAsync(vendorBillId);
                if (bill.Status == VendorBillStatus.Confirm)
                {

                    var queryable = await _vendorBillDetailRepository.GetQueryableAsync();
                    var query = from vendorBillDetail in queryable
                                where vendorBillDetail.VendorBillId == bill.Id
                                select new { vendorBillDetail };
                    var queryResult = await AsyncExecuter.ToListAsync(query);
                    var total = queryResult.Sum(x => x.vendorBillDetail.Total);

                    var bank = _cashAndBankRepository.FirstOrDefault();

                    var payment = await _vendorPaymentManager.CreateAsync(
                            await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.BillPayment),
                            DateTime.Now,
                            bank.Id,
                            bill.VendorId,
                            total,
                            bill.Number,
                            bill.Id,
                            NumberSequenceModules.Bill
                        );

                    var result = await _vendorPaymentRepository.InsertAsync(payment, true);

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
