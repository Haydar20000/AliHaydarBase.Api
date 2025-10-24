using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models;
using AliHaydarBase.Api.Dependencies;

namespace AliHaydarBase.Api.Core.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AliHaydarDbContext _context;
        public IUsersRepository User { get; private set; }

        public IRefreshTokenEntryRepository RefreshTokens { get; private set; }



        public UnitOfWork(AliHaydarDbContext context)
        {
            _context = context;
            User = new UsersRepository(_context);
            RefreshTokens = new RefreshTokenEntryRepositories(_context);
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}