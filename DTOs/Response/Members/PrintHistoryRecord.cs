using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Response.Members
{
    public class PrintHistoryRecord
    {
        public Guid Id { get; set; }
        public Guid MemberId { get; set; }
        public Guid TemplateId { get; set; }
        public DateTime PrintedAt { get; set; }
        public Guid PrintedBy { get; set; }
    }
}