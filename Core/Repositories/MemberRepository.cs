using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models.Common;
using AliHaydarBase.Api.Core.Models.Members;
using AliHaydarBase.Api.Dependencies;
using Microsoft.EntityFrameworkCore;

namespace AliHaydarBase.Api.Core.Repositories
{
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        public MemberRepository(AliHaydarDbContext context) : base(context) { }

        public async Task<PagedResult<Member>> GetPagedMembersAsync(int page, int pageSize, string search)
        {
            var query = Query();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(m =>
                    m.FullName.Contains(search) ||
                    m.RegisterNumber.Contains(search));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(m => m.FullName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Member>(items, totalCount, page, pageSize);
        }
    }
}