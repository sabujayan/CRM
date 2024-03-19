using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.VendorDebitNotes;
using Indo.Vendors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.VendorDebitNote
{
    public class DetailModel : IndoPageModel
    {
        [BindProperty]
        public VendorDebitNoteViewModel VendorDebitNote { get; set; }
        public VendorViewModel Vendor { get; set; }

        private readonly IVendorAppService _vendorAppService;

        private readonly IVendorDebitNoteAppService _vendorDebitNoteAppService;
        public DetailModel(
            IVendorDebitNoteAppService vendorDebitNoteAppService,
            IVendorAppService vendorAppService
            )
        {
            _vendorDebitNoteAppService = vendorDebitNoteAppService;
            _vendorAppService = vendorAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            VendorDebitNote = ObjectMapper.Map<VendorDebitNoteReadDto, VendorDebitNoteViewModel>(await _vendorDebitNoteAppService.GetAsync(id));

            Vendor = ObjectMapper.Map<VendorReadDto, VendorViewModel>(await _vendorAppService.GetAsync(VendorDebitNote.VendorId));
        }

        public class VendorDebitNoteViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            public Guid VendorId { get; set; }
            public string VendorName { get; set; }
            public string Number { get; set; }
            public string Description { get; set; }
            public DateTime DebitNoteDate { get; set; }
            public string CurrencyName { get; set; }
            public VendorDebitNoteStatus Status { get; set; }
            public string VendorBillNumber { get; set; }
        }

        public class VendorViewModel
        {
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }
        }
    }
}
