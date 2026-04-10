using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models.Members;
using AliHaydarBase.Api.Dependencies;
using Microsoft.EntityFrameworkCore;

namespace AliHaydarBase.Api.Core.Repositories
{
    public class PrintHistoryRepository : Repository<PrintHistory>, IPrintHistoryRepository
    {
        public PrintHistoryRepository(AliHaydarDbContext context) : base(context) { }

        public async Task<List<PrintHistory>> GetByMemberAsync(Guid memberId)
        {
            return await Query(p => p.MemberId == memberId)
                .OrderByDescending(p => p.PrintedAt)
                .ToListAsync();
        }

        public async Task<List<PrintHistory>> GetByTemplateAsync(Guid templateId)
        {
            return await Query(p => p.TemplateId == templateId)
                .OrderByDescending(p => p.PrintedAt)
                .ToListAsync();
        }

        public async Task<List<PrintHistory>> GetAllHistoryAsync()
        {
            return await Query()
                .OrderByDescending(p => p.PrintedAt)
                .ToListAsync();
        }
    }


}