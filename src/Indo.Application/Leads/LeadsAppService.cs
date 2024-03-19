using Indo.Address;
using Indo.Addresss;
using Indo.LeadsAddress;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace Indo.Leads
{
    public class LeadsAppService : ApplicationService, ILeadsAppService
    {
        private readonly ILeadsRepository _leadsRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly ILeadAddressMatrixRepository _leadsAddressMatrixRepository;
        private readonly IObjectMapper _objectMapper;

        public LeadsAppService(
            ILeadsRepository leadsRepository,
            IAddressRepository addressRepository,
            ILeadAddressMatrixRepository leadsAddressMatrixRepository,
            IObjectMapper objectMapper)
        {
            _leadsRepository = leadsRepository;
            _addressRepository = addressRepository;
            _leadsAddressMatrixRepository = leadsAddressMatrixRepository;
            _objectMapper = objectMapper;
        }

        public async Task<LeadWithAddressesDto> GetLeadWithAddressesAsync(Guid id)
        {
            var lead = await _leadsRepository.GetAsync(id);
            if (lead == null)
            {
                throw new EntityNotFoundException(typeof(LeadsInfo), id);
            }

            var leadAddresses = await _leadsAddressMatrixRepository
                .Where(lam => lam.LeadsId == id)
                .ToListAsync();

            var addressIds = leadAddresses.Select(lam => lam.AddressId).ToList();

            var addresses = await _addressRepository
                .Where(address => addressIds.Contains(address.Id))
                .ToListAsync();

            var addressDtos = addresses.Select(address => new AddressDto
            {
                Street = address.Street,
                State = address.State,
                Country = address.Country,
                City = address.City,
                ZipCode = address.ZipCode
            }).ToList();

            return new LeadWithAddressesDto
            {
                FirstName = lead.FirstName,
                LastName = lead.LastName,
                Title = lead.Title,
                Phone = lead.Phone,
                Industry = lead.Industry,
                Email = lead.Email,
                Company = lead.Company,
                Website = lead.Website,
                SkypeId = lead.SkypeId,
                Addresses = addressDtos
            };
        }



        public async Task<Guid> CreateLeadWithAddressesAsync(CreateLeadDto input)
        {
            var lead = new LeadsInfo
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                Title = input.Title,
                Phone = input.Phone,
                Industry = input.Industry,
                Email = input.Email,
                Company = input.Company,
                Website = input.Website,
                SkypeId = input.SkypeId,
                Status = input.Status,
                Comments = input.Comments
            };

            lead = await _leadsRepository.InsertAsync(lead);

            if (input.Addresses != null && input.Addresses.Any())
            {
                foreach (var addressInput in input.Addresses)
                {
                    var address = new AddressInfo
                    {
                        Street = addressInput.Street,
                        State = addressInput.State,
                        Country = addressInput.Country,
                        City = addressInput.City,
                        ZipCode = addressInput.ZipCode
                    };

                    address = await _addressRepository.InsertAsync(address);

                    await _leadsAddressMatrixRepository.InsertAsync(new LeadsAddressMatrix
                    {
                        LeadsId = lead.Id,
                        AddressId = address.Id
                    });
                }
            }

            return lead.Id;
        }

        public async Task UpdateLeadAsync(Guid id, UpdateLeadDto input)
        {
            var lead = await _leadsRepository.GetAsync(id);
            if (lead == null)
            {
                throw new EntityNotFoundException(typeof(LeadsInfo), id);
            }

            lead.FirstName = input.FirstName;
            lead.LastName = input.LastName;
            lead.Title = input.Title;
            lead.Phone = input.Phone;
            lead.Industry = input.Industry;
            lead.Email = input.Email;
            lead.Company = input.Company;
            lead.Website = input.Website;
            lead.SkypeId = input.SkypeId;
            lead.Status = input.Status;
            lead.Comments = input.Comments;

            await _leadsRepository.UpdateAsync(lead);
        }

        public async Task DeleteLeadAsync(Guid id)
        {
            var lead = await _leadsRepository.GetAsync(id);
            if (lead == null)
            {
                throw new EntityNotFoundException(typeof(LeadsInfo), id);
            }

            await _leadsRepository.DeleteAsync(lead);
        }
    }

}
