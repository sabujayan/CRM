using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Product
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateProductViewModel Product { get; set; }
        public List<SelectListItem> Uoms { get; set; }

        private readonly IProductAppService _productAppService;
        public CreateModel(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }
        public async Task OnGetAsync()
        {
            Product = new CreateProductViewModel();

            var uomLookup = await _productAppService.GetUomLookupAsync();
            Uoms = uomLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _productAppService.CreateAsync(
                    ObjectMapper.Map<CreateProductViewModel, ProductCreateDto>(Product)
                    );
                return NoContent();

            }
            catch (ProductAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateProductViewModel
        {
            [SelectItems(nameof(Uoms))]
            [DisplayName("UoM")]
            public Guid UomId { get; set; }

            [Required]
            [StringLength(ProductConsts.MaxNameLength)]
            public string Name { get; set; }

            [Required]
            public float Price { get; set; }

            [Required]
            [DisplayName("Tax Rate")]
            public float TaxRate { get; set; }

            [Required]
            [DisplayName("Retail Price")]
            public float RetailPrice { get; set; }
        }
    }
}
