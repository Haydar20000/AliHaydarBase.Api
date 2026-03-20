using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Models;

namespace AliHaydarBase.Api.Core.Interfaces
{
    public interface IRefreshTokenEntryRepository : IRepository<RefreshTokenEntry>
    {
        Task<IEnumerable<RefreshTokenEntry>> GetRevokedTokensOlderThanAsync(DateTime cutoff);
        Task DeleteRangeAsync(IEnumerable<RefreshTokenEntry> tokens);
    }
}