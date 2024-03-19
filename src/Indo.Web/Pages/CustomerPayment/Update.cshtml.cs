using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.CustomerPayments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.CustomerPayment
{
    public class UpdateModel : IndoPageModel
    {        

        [BindProperty]
        public CustomerPaymentUpdateViewModel CustomerPayment { get; set; }

        public List<SelectListItem> Customers { get; set; }

        public List<SelectListItem> PaymentMethods { get; set; }

        private readonly ICustomerPaymentAppService _customerPaymentAppService;
        public UpdateModel(ICustomerPaymentAppService customerPaymentAppService)
        {
            _customerPaymentAppService = customerPaymentAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _customerPaymentAppService.GetAsync(id);
            CustomerPayment = ObjectMapper.Map<CustomerPaymentReadDto, CustomerPaymentUpdateViewModel>(dto);

            var customerLookup = await _customerPaymentAppService.GetCustomerLookupAsync();
            Customers = customerLookup.Items
                .Where(x => x.Id.Equals(dto.CustomerId))
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var cashBankLookup = await _customerPaymentAppService.GetCashAndBankLookupAsync();
            PaymentMethods = cashBankLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _customerPaymentAppService.UpdateAsync(
                    CustomerPayment.Id,
                    ObjectMapper.Map<CustomerPaymentUpdateViewModel, CustomerPaymentUpdateDto>(CustomerPayment)
                    );
                return NoContent();
            }
            catch (CustomerPaymentAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CustomerPaymentUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [StringLength(CustomerPaymentConsts.MaxNumberLength)]
            public string Number { get; set; }

            [TextArea]
            public string Description { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [DisplayName("Payment Date")]
            public DateTime PaymentDate { get; set; }

            [Required]
            [SelectItems(nameof(Customers))]
            [DisplayName("Customer")]
            public Guid CustomerId { get; set; }

            [Required]
            [SelectItems(nameof(PaymentMethods))]
            [DisplayName("Payment Methods")]
            public Guid CashAndBankId { get; set; }

            [Required]
            public float Amount { get; set; }
        }
    }
}
