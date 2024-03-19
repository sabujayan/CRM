using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Indo.Pages
{
    public class Index_Tests : IndoWebTestBase
    {
        [Fact]
        public async Task Welcome_Page()
        {
            var response = await GetResponseAsStringAsync("/");
            response.ShouldNotBeNull();
        }
    }
}
