using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models.Common;
using AliHaydarBase.Api.Core.Models.Members;
using AliHaydarBase.Api.Dependencies;
using AliHaydarBase.Api.DTOs.Response.Members;
using Microsoft.EntityFrameworkCore;

namespace AliHaydarBase.Api.Core.Repositories
{
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        public MemberRepository(AliHaydarDbContext context) : base(context) { }

        public async Task<List<MemberRowDto>> GetAllMembersAsync()
        {
            var items = await Query().ToListAsync();

            return items.Select(m => new MemberRowDto
            {
                Id = m.Id,
                FullNameArabic = m.FullNameArabic,
                FullNameEnglish = m.FullNameEnglish,

                Stage = m.Stage,
                RegisterNumber = m.RegisterNumber,

                City = m.City,
                Phone = m.Phone,

                DateOfBirth = m.DateOfBirth,
                RegisterDate = m.RegisterDate,
                LastYearIdentityRenewal = m.LastYearIdentityRenewal,

                Status = m.IsBlockedByAdmin ? "Blocked" : "Active",
                ImageBase64 = m.ImageUrl,

                IsIdPrinted = m.IsIdPrinted,
                IsBlockedByAdmin = m.IsBlockedByAdmin
            }).ToList();
        }
        public async Task<List<Member>> GetMembersByIdsAsync(List<Guid> memberIds)
        {
            return await Query()
                .Where(m => memberIds.Contains(m.Id))
                .ToListAsync();
        }
        public async Task<List<MemberRowDto>> GetMembersForPrintAsync(List<Guid> memberIds)
        {
            var items = await Query()
                .Where(m => memberIds.Contains(m.Id))
                .ToListAsync();

            return items.Select(m => new MemberRowDto
            {
                Id = m.Id,
                FullNameArabic = m.FullNameArabic,
                FullNameEnglish = m.FullNameEnglish,

                Stage = m.Stage,
                RegisterNumber = m.RegisterNumber,

                City = m.City,
                Phone = m.Phone,

                DateOfBirth = m.DateOfBirth,
                RegisterDate = m.RegisterDate,
                LastYearIdentityRenewal = m.LastYearIdentityRenewal,

                Status = m.IsBlockedByAdmin ? "Blocked" : "Active",
                ImageBase64 = m.ImageUrl != null ? Convert.ToBase64String(System.IO.File.ReadAllBytes(m.ImageUrl)) : null,

                IsIdPrinted = m.IsIdPrinted,
                IsBlockedByAdmin = m.IsBlockedByAdmin
            }).ToList();
        }
        public async Task<PagedResult<Member>> GetPagedMembersAsync(int page, int pageSize, string search)
        {
            var query = Query();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(m =>
                    m.FullNameArabic.Contains(search) ||
                    m.RegisterNumber.Contains(search));
            }

            // Default ordering (required for stable pagination)
            query = query.OrderBy(m => m.FullNameArabic);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Member>(items, totalCount, page, pageSize);
        }

    }
}