using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models;
using AliHaydarBase.Api.Dependencies;
using Microsoft.EntityFrameworkCore;

namespace AliHaydarBase.Api.Core.Repositories
{
    public class RefreshTokenEntryRepositories : Repository<RefreshTokenEntry>, IRefreshTokenEntryRepository
    {
        public RefreshTokenEntryRepositories(AliHaydarDbContext context)
       : base(context)
        {
        }

        public async Task<IEnumerable<RefreshTokenEntry>> GetRevokedTokensOlderThanAsync(DateTime cutoff)
        {
            return await FindAsync(t => t.IsRevoked && t.RevokedAt < cutoff);
        }

        public async Task DeleteRangeAsync(IEnumerable<RefreshTokenEntry> tokens)
        {
            foreach (var token in tokens)
                await DeleteAsync(token.Id);
        }
    }
}