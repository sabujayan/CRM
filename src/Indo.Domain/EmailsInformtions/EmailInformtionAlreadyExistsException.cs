using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Indo.EmailsInformtions
{
    public class EmailInformtionAlreadyExistsException : BusinessException
    {
        public EmailInformtionAlreadyExistsException(string name)
          : base("EmailInformtionAlreadyExists")
        {
            WithData("name", name);
        }
    }
}
