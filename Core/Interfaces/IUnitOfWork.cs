using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Models;

namespace AliHaydarBase.Api.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUsersRepository User { get; }
        IRefreshTokenEntryRepository RefreshTokens { get; }
        IClaimDefinitionsRepository ClaimDefinitions { get; }
        IAuditLoggerRepository AuditLogger { get; } // IAuditLogger
        Task<int> Complete();

    }
}