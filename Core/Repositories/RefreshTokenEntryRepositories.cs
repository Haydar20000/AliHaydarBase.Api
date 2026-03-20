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
        private readonly IUnitOfWork _unitOfWork;
        public RefreshTokenEntryRepositories(AliHaydarDbContext context, IUnitOfWork unitOfWork) : base(context)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task DeleteRangeAsync(IEnumerable<RefreshTokenEntry> tokens)
        {
            _ = _unitOfWork.RefreshTokens.DeleteRangeAsync(tokens);
            await _unitOfWork.Complete();
        }

        public async Task<IEnumerable<RefreshTokenEntry>> GetRevokedTokensOlderThanAsync(DateTime cutoff)
        {
            return await _unitOfWork.RefreshTokens.FindAsync(t => t.IsRevoked && t.RevokedAt < cutoff);
        }
    }
}