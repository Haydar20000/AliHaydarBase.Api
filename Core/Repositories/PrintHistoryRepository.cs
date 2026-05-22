using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models.Common;
using AliHaydarBase.Api.Core.Models.Members;
using AliHaydarBase.Api.Core.Models.MonitoringData;
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
                .OrderByDescending(p => p.PrintedAtUtc)
                .ToListAsync();
        }

        public async Task<List<PrintHistory>> GetByTemplateAsync(Guid templateId)
        {
            return await Query(p => p.TemplateId == templateId)
                .OrderByDescending(p => p.PrintedAtUtc)
                .ToListAsync();
        }

        public async Task<List<PrintHistory>> GetAllHistoryAsync()
        {
            return await Query()
                .OrderByDescending(p => p.PrintedAtUtc)
                .ToListAsync();
        }

        public async Task<PagedResult<PrintHistory>> GetPagedHistoryAsync(
            int page,
            int pageSize,
            string? city,
            string? search,
            string? template,
            string? mode,
            DateTime? from,
            DateTime? to,
            string sortBy,
            string sortDir)
        {
            // base.Query() doesn’t include navigation properties, so we use _context directly
            var query = _context.Set<PrintHistory>()
                .Include(p => p.Member)
                .Include(p => p.TemplateName)
                .AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(city))
                query = query.Where(p => p.Member.City == city);

            if (!string.IsNullOrEmpty(template))
                query = query.Where(p => p.TemplateName.Contains(template));

            if (!string.IsNullOrEmpty(mode))
                query = query.Where(p => p.PrintMode == mode);

            if (from.HasValue)
                query = query.Where(p => p.PrintedAtUtc >= from.Value);

            if (to.HasValue)
                query = query.Where(p => p.PrintedAtUtc <= to.Value);

            // Search (MemberName, RegisterNumber, TemplateName)
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p =>
                    p.Member.FullNameArabic.Contains(search) ||
                    p.Member.RegisterNumber.Contains(search) ||
                    p.TemplateName.Contains(search));
            }

            // Sorting
            query = (sortBy, sortDir.ToLower()) switch
            {
                ("MemberName", "asc") => query.OrderBy(p => p.Member.FullNameArabic),
                ("MemberName", "desc") => query.OrderByDescending(p => p.Member.FullNameArabic),

                ("RegisterNumber", "asc") => query.OrderBy(p => p.Member.RegisterNumber),
                ("RegisterNumber", "desc") => query.OrderByDescending(p => p.Member.RegisterNumber),

                ("TemplateName", "asc") => query.OrderBy(p => p.TemplateName),
                ("TemplateName", "desc") => query.OrderByDescending(p => p.TemplateName),

                ("PrintedAt", "asc") => query.OrderBy(p => p.PrintedAtUtc),
                _ => query.OrderByDescending(p => p.PrintedAtUtc)
            };

            // Pagination
            var total = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<PrintHistory>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }
    }

}