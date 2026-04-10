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

        public IClaimDefinitionsRepository ClaimDefinitions { get; private set; }

        public IAuditLoggerRepository AuditLogger { get; private set; }

        public IBlogsRepository Blogs { get; private set; }

        public ICategoriesRepository Categories { get; private set; }

        public IBlogImagesRepository BlogImages { get; private set; }

        public IMemberRepository Members { get; private set; }
        public IPrintHistoryRepository PrintHistory { get; private set; }
        public IIdCardTemplateRepository IdCardTemplates { get; private set; }

        public UnitOfWork(AliHaydarDbContext context)
        {
            _context = context;
            User = new UsersRepository(_context);
            RefreshTokens = new RefreshTokenEntryRepositories(_context, this);
            ClaimDefinitions = new ClaimDefinitionsRepository(_context);
            AuditLogger = new AuditLoggerRepository(_context, this);
            Blogs = new BlogsRepository(_context);
            Categories = new CategoriesRepository(_context);
            BlogImages = new BlogImagesRepository(_context);
            Members = new MemberRepository(_context);
            PrintHistory = new PrintHistoryRepository(_context);
            IdCardTemplates = new IdCardTemplateRepository(_context);

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