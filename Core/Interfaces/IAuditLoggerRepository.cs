using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Models;

namespace AliHaydarBase.Api.Core.Interfaces
{
    public interface IAuditLoggerRepository : IRepository<AuditLog>
    {
        Task LogAsync(string userId, string action, string description, string ip, string deviceId, object? metadata = null);

    }
}