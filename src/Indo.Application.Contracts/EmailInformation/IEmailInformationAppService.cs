using Indo.Clientes;
using Indo.EmailsTemplates;
using Indo.Expenses;
using Indo.Projectes;
using Indo.Technologies;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Indo.EmailInformation
{
    public interface IEmailInformationAppService : IApplicationService
    {
        Task<ListResultDto<EmployeeLookupDto>> GetEmployeeLookupAsync();
        Task<ListResultDto<EmailsTemplatesLookUpDto>> GetEmailsTemplatesLookupAsync();
        void SendEmail(EmailDto request);
    }
}
