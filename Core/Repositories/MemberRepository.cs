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

        public async Task<PagedResult<Member>> GetPagedMembersAsync(int page, int pageSize, string search, string? sortBy, string? sortDir)
        {
            var query = Query();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(m =>
                    m.FullNameArabic.Contains(search) ||
                    m.RegisterNumber.Contains(search));
            }
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                query = sortBy.ToLower() switch
                {
                    "fullname" => sortDir == "desc"
                        ? query.OrderByDescending(m => m.FullNameArabic)
                        : query.OrderBy(m => m.FullNameArabic),

                    "stage" => sortDir == "desc"
                        ? query.OrderByDescending(m => m.Stage)
                        : query.OrderBy(m => m.Stage),

                    "registernumber" => sortDir == "desc"
                        ? query.OrderByDescending(m => m.RegisterNumber)
                        : query.OrderBy(m => m.RegisterNumber),

                    "lastyearidentityrenewal" => sortDir == "desc"
                        ? query.OrderByDescending(m => m.LastYearIdentityRenewal)
                        : query.OrderBy(m => m.LastYearIdentityRenewal),

                    "status" => sortDir == "desc"
                        ? query.OrderByDescending(m => m.IsBlockedByAdmin)
                        : query.OrderBy(m => m.IsBlockedByAdmin),

                    _ => query.OrderBy(m => m.FullNameArabic)
                };
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(m => m.FullNameArabic)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Member>(items, totalCount, page, pageSize);
        }
    }
}