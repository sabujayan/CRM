using Indo.Clientes;
using Indo.ClientesAddress;
using Indo.ClientesContact;
using Indo.ClientsProjects;
using Indo.EmailsInformtions;
using Indo.Projectes;
using NETCore.MailKit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Account;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.Clients;
using MailKit.Net.Smtp;
using MimeKit;
using AutoMapper;
using Volo.Abp.Application.Dtos;
using Indo.Expenses;
using Volo.Abp.ObjectMapping;
using Indo.EmailsTemplates;
using Indo.Employees;
using Indo.Technologies;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Emailing;
using Volo.Abp;
using static IdentityServer4.Models.IdentityResources;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace Indo.EmailInformation
{
    public class EmailInformationAppService : IndoAppService, IEmailInformationAppService
    {
        private readonly IEmailInformtionRepository _emailInformtionRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmailTemplatesRepository _emailTemplatesRepository;
        private readonly IConfiguration _configuration;

        public EmailInformationAppService(
            IEmailInformtionRepository emailInformtionRepository,
            IEmployeeRepository employeeRepository,
            IEmailTemplatesRepository emailTemplatesRepository,
            IConfiguration configuration
        )
        {
            _emailInformtionRepository = emailInformtionRepository;
            _employeeRepository = employeeRepository;
            _emailTemplatesRepository = emailTemplatesRepository;
            _configuration = configuration;
        }

        public async Task<ListResultDto<EmployeeLookupDto>> GetEmployeeLookupAsync()
        {
            var list = await _employeeRepository.GetListAsync();
            return new ListResultDto<EmployeeLookupDto>(
                ObjectMapper.Map<List<Employee>, List<EmployeeLookupDto>>(list)
            );
        }

        public async Task<ListResultDto<EmailsTemplatesLookUpDto>> GetEmailsTemplatesLookupAsync()
        {
            var list = await _emailTemplatesRepository.GetListAsync();
            return new ListResultDto<EmailsTemplatesLookUpDto>(
                ObjectMapper.Map<List<EmailTemplates>, List<EmailsTemplatesLookUpDto>>(list)
            );
        }

        public void SendEmail(EmailDto request)
        {
            var template = _emailTemplatesRepository.FirstOrDefault(t => t.Id == request.TemplateId);
            if (template == null)
            {
                throw new Exception("Email template not found.");
            }

            var emailHost = _configuration["EmailHost"];
            var emailUserName = _configuration["EmailUserName"];
            var emailPassword = _configuration["EmailPassword"];

            var mailMessage = new MailMessage
            {
                From = new MailAddress(emailUserName),
                Subject = template.Subject,
                Body = template.Body,
                IsBodyHtml = true
            };

            foreach (var toEmail in request.To)
            {
                var employee = _employeeRepository.FirstOrDefault(e => e.Id.ToString() == toEmail);
                if (employee == null)
                {
                    throw new Exception("Employee not found.");
                }

                mailMessage.To.Add(employee.Email);
            }

            foreach (var ccEmail in request.Cc)
            {
                var employee = _employeeRepository.FirstOrDefault(e => e.Id.ToString() == ccEmail);
                if (employee == null)
                {
                    throw new Exception("Employee not found.");
                }

                mailMessage.CC.Add(employee.Email);
            }

            foreach (var bccEmail in request.Bcc)
            {
                var employee = _employeeRepository.FirstOrDefault(e => e.Id.ToString() == bccEmail);
                if (employee == null)
                {
                    throw new Exception("Employee not found.");
                }

                mailMessage.Bcc.Add(employee.Email);
            }

            using (var smtpClient = new System.Net.Mail.SmtpClient(emailHost, 587))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(emailUserName, emailPassword);

                try
                {
                    smtpClient.Send(mailMessage);

                    foreach (var toEmail in request.To)
                    {
                        var employee = _employeeRepository.FirstOrDefault(e => e.Id.ToString() == toEmail);
                        if (employee != null)
                        {
                            var emailInfo = new EmailInformtion(
                                Guid.NewGuid(),
                                request.TemplateId,
                                employee.Id
                            )
                            {
                                Status = true,
                                IsSent = true
                            };

                            _emailInformtionRepository.InsertAsync(emailInfo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to send email.", ex);
                }
            }
        }
    }

}
