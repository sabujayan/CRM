using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.EmailsTemplates
{
    public class EmailTemplatesManager : DomainService
    {
        private readonly IEmailTemplatesRepository _emailtemplateRepository;

        public EmailTemplatesManager(IEmailTemplatesRepository emailtemplateRepository)
        {
            _emailtemplateRepository = emailtemplateRepository;
        }

        public async Task<EmailTemplates> CreateAsync(
       [NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            var existing = await _emailtemplateRepository.FindAsync(x => x.TemplateName.Equals(name));
            return new EmailTemplates(
                GuidGenerator.Create(),
                name
            );
        }
    }
}
