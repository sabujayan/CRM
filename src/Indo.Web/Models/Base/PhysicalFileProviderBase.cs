using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Indo.Web.Models.Base;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;


namespace Indo.Web.Models.Base
{
    public  interface PhysicalFileProviderBase : FileProviderBase
    {        
            void RootFolder(string folderName);
        }
    
}
