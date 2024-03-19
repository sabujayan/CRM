using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Domain.Services;
using Indo.LeadsAddress;
using Indo.Clientes;

namespace Indo.Leads
{
    public class LeadsManager : DomainService
    {
        private readonly ILeadsRepository _leadsRepository;

        public LeadsManager(ILeadsRepository leadsRepository)
        {
            _leadsRepository = leadsRepository;
        }

        public async Task<LeadsInfo> CreateAsync(string firstName, string lastName, string phone, string industry, string title, string email, string company, string website, string skypeId, string status, string comments)
        {
            Check.NotNullOrWhiteSpace(firstName, nameof(firstName));
            Check.NotNullOrWhiteSpace(lastName, nameof(lastName));

            var lead = new LeadsInfo(
                GuidGenerator.Create(),
                firstName,
                lastName
            )
            {
                Phone = phone,
                Industry = industry,
                Title = title,
                Email = email,
                Company = company,
                Website = website,
                SkypeId = skypeId,
                Status = status,
                Comments = comments
            };

            return await _leadsRepository.InsertAsync(lead);
        }


        public async Task UpdateAsync(Guid id, string firstName, string lastName, string phone, string industry, string title, string email, string company, string website, string skypeId, string status, string comments)
        {
            Check.NotNullOrWhiteSpace(firstName, nameof(firstName));
            Check.NotNullOrWhiteSpace(lastName, nameof(lastName));

            var lead = await _leadsRepository.FindAsync(id);
            if (lead == null)
            {
                throw new EntityNotFoundException(typeof(LeadsInfo), id);
            }

            var existing = await _leadsRepository.FindAsync(x => x.FirstName.Equals(firstName) && x.LastName.Equals(lastName) && x.Id != id);
            if (existing != null)
            {
                throw new LeadsAlreadyExistsException(firstName, lastName);
            }

            lead.FirstName = firstName;
            lead.LastName = lastName;
            lead.Phone = phone;
            lead.Industry = industry;
            lead.Title = title;
            lead.Email = email;
            lead.Company = company;
            lead.Website = website;
            lead.SkypeId = skypeId;
            lead.Status = status;
            lead.Comments = comments;

            await _leadsRepository.UpdateAsync(lead);
        }

        public async Task DeleteAsync(Guid id)
        {
            var leads = await _leadsRepository.FindAsync(id);
            if (leads == null)
            {
                throw new EntityNotFoundException(typeof(LeadsInfo), id);
            }

            await _leadsRepository.DeleteAsync(id);
        }
    }
}
