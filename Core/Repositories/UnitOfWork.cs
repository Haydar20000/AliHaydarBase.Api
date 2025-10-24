using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Dependencies;

namespace AliHaydarBase.Api.Core.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AliHaydarDbContext _context;
        public IUsersRepository User { get; private set; }

        public UnitOfWork(AliHaydarDbContext context)
        {
            _context = context;
            User = new UsersRepository(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}