using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.DTOs.Request;
using AliHaydarBase.Api.DTOs.Response;

namespace AliHaydarBase.Api.Core.Repositories
{
    public class EmailServicesRepository : IEmailServicesRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostEnvironment;
        public EmailServicesRepository(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }
        public async Task<SystemResponseDto> ConfirmEmailTemp(EmailRequestDto request)
        {
            try
            {
                var message = _hostEnvironment.ContentRootPath + "\\Templates\\" + "EmailConfirmOtpTemplate.html";
                StreamReader streamReader = new StreamReader(message);
                var body = await streamReader.ReadToEndAsync();
                streamReader.Close();

                body = body.Replace("@ViewBag.UserName", request.MessageVariables[0]);
                body = body.Replace("@ViewBag.Company", _configuration.GetValue<string>("EmailService:From"));
                body = body.Replace("@ViewBag.Code", request.MessageVariables[1]);
                body = body.Replace("@ViewBag.Subject", request.MessageVariables[2]);


                request.Body = body;
                SystemResponseDto responseDto = await SendEmailAsync(request);
                return responseDto;
            }
            catch (System.Exception e)
            {
                var response = new SystemResponseDto();
                response.IsSuccessful = false;
                response.Errors.Append(e.Message);
                return response;
                throw;
            }
        }

        public async Task<SystemResponseDto> SendEmailAsync(EmailRequestDto request)
        {
            var response = new SystemResponseDto();
            try
            {
                var _email = _configuration.GetValue<string>("EmailService:Email");
                var _pass = _configuration.GetValue<string>("EmailService:ServiceKey");
                var _host = _configuration.GetValue<string>("EmailService:Host");
                var _port = _configuration.GetValue<int>("EmailService:Port");
                var _fromName = _configuration.GetValue<string>("EmailService:From");
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(_email!, _fromName);
                mailMessage.Subject = request.Subject;
                mailMessage.Body = request.Body;
                mailMessage.IsBodyHtml = true;
                if (request.Receptors.Count > 0)
                {
                    foreach (var receptor in request.Receptors)
                    {
                        mailMessage.To.Add(receptor);
                    }
                }
                else
                {
                    mailMessage.To.Add(_email!);
                }
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.EnableSsl = true;
                    smtp.Host = _host!;
                    smtp.Port = _port;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(_email, _pass);
                    smtp.SendCompleted += (s, e) => { smtp.Dispose(); };
                    await smtp.SendMailAsync(mailMessage);
                }
                response.IsSuccessful = true;

                return response;
            }
            catch (System.Exception e)
            {
                response.IsSuccessful = false;
                response.Errors.Append(e.Message);
                return response;
            }
        }
    }
}