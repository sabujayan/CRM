using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.CustomerPayments;
using Indo.VendorPayments;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Indo.CashAndBanks
{
    public class CashAndBankAppService : IndoAppService, ICashAndBankAppService
    {
        private readonly ICashAndBankRepository _cashAndBankRepository;
        private readonly CashAndBankManager _cashAndBankManager;
        private readonly ICustomerPaymentRepository _customerPaymentRepository;
        private readonly CustomerPaymentAppService _customerPaymentAppService;
        private readonly VendorPaymentAppService _vendorPaymentAppService;
        public CashAndBankAppService(
            ICashAndBankRepository cashAndBankRepository,
            CashAndBankManager cashAndBankManager,
            ICustomerPaymentRepository customerPaymentRepository,
            CustomerPaymentAppService customerPaymentAppService,
            VendorPaymentAppService vendorPaymentAppService
            )
        {
            _cashAndBankRepository = cashAndBankRepository;
            _cashAndBankManager = cashAndBankManager;
            _customerPaymentRepository = customerPaymentRepository;
            _customerPaymentAppService = customerPaymentAppService;
            _vendorPaymentAppService = vendorPaymentAppService;
        }
        public async Task<CashAndBankReadDto> GetAsync(Guid id)
        {
            var obj = await _cashAndBankRepository.GetAsync(id);
            return ObjectMapper.Map<CashAndBank, CashAndBankReadDto>(obj);
        }
        public async Task<List<CashAndBankReadDto>> GetListAsync()
        {
            var queryable = await _cashAndBankRepository.GetQueryableAsync();
            var query = from cashAndBank in queryable
                        select new { cashAndBank };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<CashAndBank, CashAndBankReadDto>(x.cashAndBank);
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<List<CashAndBankReportDto>> GetReportAsync()
        {
            var customers = await _customerPaymentAppService.GetListAsync();
            var customerReports = customers.Select(x => new CashAndBankReportDto { 
                Id = x.Id,
                Period = x.Period,
                PaymentMethod = x.CashAndBankName,
                PaymentNumber = x.Number,
                SourceDocument = x.SourceDocument,
                SourceDocumentModule = x.SourceDocumentModuleString,
                SourceDocumentPath = x.SourceDocumentPath,
                SourceDocumentId = x.SourceDocumentId,
                ThirdParty = x.CustomerName,
                Status = x.StatusString,
                PaymentDate = x.PaymentDate,
                Currency = x.CurrencyName,
                Amount = x.Balance
            });

            var vendors = await _vendorPaymentAppService.GetListAsync();
            var vendorReports = vendors.Select(x => new CashAndBankReportDto
            {
                Id = x.Id,
                Period = x.Period,
                PaymentMethod = x.CashAndBankName,
                PaymentNumber = x.Number,
                SourceDocument = x.SourceDocument,
                SourceDocumentModule = x.SourceDocumentModuleString,
                SourceDocumentPath = x.SourceDocumentPath,
                SourceDocumentId = x.SourceDocumentId,
                ThirdParty = x.VendorName,
                Status = x.StatusString,
                PaymentDate = x.PaymentDate,
                Currency = x.CurrencyName,
                Amount = x.Balance
            });

            var combine = customerReports.Concat(vendorReports);

            return combine.ToList();
        }
        public async Task<CashAndBankReadDto> CreateAsync(CashAndBankCreateDto input)
        {
            var obj = await _cashAndBankManager.CreateAsync(
                input.Name
            );

            obj.Description = input.Description;

            await _cashAndBankRepository.InsertAsync(obj);

            return ObjectMapper.Map<CashAndBank, CashAndBankReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, CashAndBankUpdateDto input)
        {
            var obj = await _cashAndBankRepository.GetAsync(id);

            if (obj.Name != input.Name)
            {
                await _cashAndBankManager.ChangeNameAsync(obj, input.Name);
            }

            obj.Description = input.Description;

            await _cashAndBankRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            if (_customerPaymentRepository.Where(x => x.CashAndBankId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            await _cashAndBankRepository.DeleteAsync(id);
        }
    }
}
