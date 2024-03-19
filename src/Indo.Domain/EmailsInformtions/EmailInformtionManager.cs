using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Indo.EmailsInformtions
{
    public class EmailInformtionManager : DomainService
    {
        private readonly IEmailInformtionRepository _emailInformtionRepository;

        public EmailInformtionManager(IEmailInformtionRepository emailInformtionRepository)
        {
            _emailInformtionRepository = emailInformtionRepository;
        }
    }
}
