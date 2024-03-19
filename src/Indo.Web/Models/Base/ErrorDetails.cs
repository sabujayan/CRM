using System;
using System.Collections.Generic;
using System.Linq;
#if EJ2_DNX
using System.Web;
#endif

namespace Indo.Web.Models.Base
{
    public class ErrorDetails
    {

        public string Code { get; set; }

        public string Message { get; set; }

        public IEnumerable<string> FileExists { get; set; }
    }
}