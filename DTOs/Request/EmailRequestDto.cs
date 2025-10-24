using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Request
{
    public class EmailRequestDto
    {
        public required List<MailAddress> Receptors { get; set; }
        public required string Subject { get; set; }
        public string? Body { get; set; }
        public required List<string> MessageVariables { get; set; }
    }
}