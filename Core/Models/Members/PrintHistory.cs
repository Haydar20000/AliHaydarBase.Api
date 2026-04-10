using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.Core.Models.Members
{
    public class PrintHistory
    {

        public Guid Id { get; set; }

        public Guid MemberId { get; set; }
        public Member Member { get; set; }

        public Guid TemplateId { get; set; }
        public IdCardTemplate Template { get; set; }

        public DateTime PrintedAt { get; set; }

        // This is the UserId of the admin who printed the card
        public Guid PrintedBy { get; set; }
    }
}