using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.DTOs.Request;
using AliHaydarBase.Api.DTOs.Response;

namespace AliHaydarBase.Api.Core.Interfaces
{
    public interface IEmailServicesRepository
    {
        Task<SystemResponseDto> SendEmailAsync(EmailRequestDto request);

        Task<SystemResponseDto> ConfirmEmailTemp(EmailRequestDto request);
    }
}