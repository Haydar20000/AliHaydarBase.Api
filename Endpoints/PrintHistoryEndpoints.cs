using System.Security.Claims;
using System.Text;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models.Common;
using AliHaydarBase.Api.Core.Models.MonitoringData;
using AliHaydarBase.Api.DTOs.Request.MonitoringDataDtos;
using AliHaydarBase.Api.DTOs.Response;
using AliHaydarBase.Api.DTOs.Response.Members;
using AliHaydarBase.Api.HelperFunctions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AliHaydarBase.Api.Endpoints
{
    public static class PrintHistoryEndpoints
    {
        public static void MapPrintHistoryEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("api/print-history");

            group.MapGet("/member/{memberId:guid}", GetByMember);
            group.MapGet("/template/{templateId:guid}", GetByTemplate);

            // Unified paged endpoint (filters + search + sorting + admin city restriction)
            group.MapGet("/paged", GetPagedHistory)
                 .RequireAuthorization();

            group.MapPost("/", AddPrintHistory)
                 .RequireAuthorization();

            group.MapGet("/export", ExportHistory)
                 .RequireAuthorization();

            group.MapGet("/analytics", GetAnalytics)
                 .RequireAuthorization();

            group.MapGet("/logs", GetActivityLogs)
                .RequireAuthorization();

            group.MapGet("/admins", GetAdminUsers)
                .RequireAuthorization();

        }

        /// <summary>
        /// GET /api/print-history/admins
        /// Returns a list of all admin users (Id + FullName)
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
        private static async Task<IResult> GetAdminUsers(IUnitOfWork unitOfWork)
        {
            // Return ALL users (because DB does not store roles)
            var admins = await unitOfWork.User
                .Query()
                .Select(u => new
                {
                    Id = u.Id,
                    Name = u.FullName,
                })
                .ToListAsync();

            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Admin users retrieved successfully",
                Data = admins
            });
        }


        // ---------------------------------------------------------
        // GET /api/print-history/paged
        // Unified endpoint (filters + search + sorting + pagination)
        // ---------------------------------------------------------
        private static async Task<IResult> GetPagedHistory(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ClaimsPrincipal user,
    int page = 1,
    int pageSize = 20,
    string? city = null,
    string? search = null,
    string? template = null,
    string? mode = null,
    DateTime? from = null,
    DateTime? to = null,
    string sortBy = "PrintedAt",
    string sortDir = "desc",
    string? actionType = null,
    string? adminUser = null,
    string? memberId = null)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return Results.BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Page and pageSize must be greater than zero"
                });
            }

            // Extract user info
            var userCity = user.FindFirst("city")?.Value;
            var isAdmin = user.IsInRole("Admin");

            // Non-admins can only see their own city
            if (!isAdmin)
                city = userCity;

            // Base query
            var query = unitOfWork.PrintHistory.Query();

            // -----------------------------
            // ⭐ Apply Filters (Exact Match)
            // -----------------------------

            if (!string.IsNullOrWhiteSpace(city))
                query = query.Where(h => h.Member.City == city);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(h =>
                    h.Member.FullNameArabic.Contains(search) ||
                    h.Member.RegisterNumber.Contains(search));

            if (!string.IsNullOrWhiteSpace(template))
                query = query.Where(h => h.TemplateName == template);

            if (!string.IsNullOrWhiteSpace(mode))
                query = query.Where(h => h.PrintMode == mode);

            if (from.HasValue)
                query = query.Where(h => h.PrintedAtUtc >= from.Value);

            if (to.HasValue)
                query = query.Where(h => h.PrintedAtUtc <= to.Value);

            // ⭐ NEW FILTERS (Exact Match)
            if (!string.IsNullOrWhiteSpace(actionType))
                query = query.Where(h => h.ActionType == actionType);

            if (!string.IsNullOrWhiteSpace(adminUser))
                query = query.Where(h => h.UserId.ToString() == adminUser);

            if (!string.IsNullOrWhiteSpace(memberId))
                query = query.Where(h => h.Member.RegisterNumber == memberId);

            // -----------------------------
            // ⭐ Sorting
            // -----------------------------
            query = sortBy switch
            {
                "MemberName" => (sortDir == "asc")
                    ? query.OrderBy(h => h.Member.FullNameArabic)
                    : query.OrderByDescending(h => h.Member.FullNameArabic),

                "TemplateName" => (sortDir == "asc")
                    ? query.OrderBy(h => h.TemplateName)
                    : query.OrderByDescending(h => h.TemplateName),

                _ => (sortDir == "asc")
                    ? query.OrderBy(h => h.PrintedAtUtc)
                    : query.OrderByDescending(h => h.PrintedAtUtc)
            };

            // -----------------------------
            // ⭐ Pagination
            // -----------------------------
            var total = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dto = mapper.Map<List<PrintHistoryRowDto>>(items);

            var paged = new PagedResult<PrintHistoryRowDto>(dto, total, page, pageSize);

            return Results.Ok(new ApiResponse<PagedResult<PrintHistoryRowDto>>
            {
                Success = true,
                Message = "Paged print history retrieved successfully",
                Data = paged
            });
        }

        // ---------------------------------------------------------
        // GET /api/print-history/member/{memberId}
        // ---------------------------------------------------------
        private static async Task<IResult> GetByMember(
            Guid memberId,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            var history = await unitOfWork.PrintHistory.GetByMemberAsync(memberId);

            var dto = mapper.Map<List<PrintHistoryRecord>>(history);

            return Results.Ok(new ApiResponse<List<PrintHistoryRecord>>
            {
                Success = true,
                Message = "Print history retrieved successfully",
                Data = dto
            });
        }

        // ---------------------------------------------------------
        // GET /api/print-history/template/{templateId}
        // ---------------------------------------------------------
        private static async Task<IResult> GetByTemplate(
            Guid templateId,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            var history = await unitOfWork.PrintHistory.GetByTemplateAsync(templateId);

            var dto = mapper.Map<List<PrintHistoryRecord>>(history);

            return Results.Ok(new ApiResponse<List<PrintHistoryRecord>>
            {
                Success = true,
                Message = "Template print history retrieved successfully",
                Data = dto
            });
        }

        // ---------------------------------------------------------
        // POST /api/print-history
        // ---------------------------------------------------------
        private static async Task<IResult> AddPrintHistory(
            PrintHistoryRecord record,
            IUnitOfWork unitOfWork,
            ClaimsPrincipal user)
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdString, out Guid userId))
                return Results.Unauthorized();

            if (record.MemberId == Guid.Empty || record.TemplateId == Guid.Empty)
            {
                return Results.BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "MemberId and TemplateId are required"
                });
            }

            var entity = new PrintHistory
            {
                Id = Guid.NewGuid(),
                MemberId = record.MemberId,
                TemplateId = record.TemplateId,
                PrintedAtUtc = DateTime.UtcNow,
                UserId = userId
            };

            await unitOfWork.PrintHistory.AddAsync(entity);
            await unitOfWork.Complete();

            return Results.Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Print history added successfully"
            });
        }

        private static async Task<IResult> ExportHistory(
            string format,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ClaimsPrincipal user)
        {
            var isAdmin = user.IsInRole("Admin");
            var userCity = user.FindFirst("city")?.Value;

            // Admins get all history, users get only their city
            var history = await unitOfWork.PrintHistory.GetAllHistoryAsync();

            if (!isAdmin && !string.IsNullOrWhiteSpace(userCity))
                history = history.Where(h => h.Member.City == userCity).ToList();

            var dto = mapper.Map<List<PrintHistoryRowDto>>(history);

            if (format?.ToLower() == "csv")
            {
                var csv = ExportHelper.ToCsv(dto);
                return Results.File(Encoding.UTF8.GetBytes(csv), "text/csv", "print-history.csv");
            }

            if (format?.ToLower() == "pdf")
            {
                var pdf = ExportHelper.ToPdf(dto);
                return Results.File(pdf, "application/pdf", "print-history.pdf");
            }

            return Results.BadRequest("Invalid format. Use ?format=csv or ?format=pdf");
        }

        private static async Task<IResult> GetAnalytics(
            IUnitOfWork unitOfWork,
            ClaimsPrincipal user)
        {
            var isAdmin = user.IsInRole("Admin");
            var userCity = user.FindFirst("city")?.Value;

            var query = unitOfWork.PrintHistory.Query();

            // Non-admins only see their city
            if (!isAdmin && !string.IsNullOrWhiteSpace(userCity))
                query = query.Where(h => h.Member.City == userCity);

            var today = DateTime.UtcNow.Date;
            var week = today.AddDays(-7);
            var month = today.AddMonths(-1);

            var result = new
            {
                Today = await query.CountAsync(h => h.PrintedAtUtc >= today),
                Week = await query.CountAsync(h => h.PrintedAtUtc >= week),
                Month = await query.CountAsync(h => h.PrintedAtUtc >= month),

                TopUser = await query
                    .GroupBy(h => h.UserId)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .FirstOrDefaultAsync(),

                TopCity = await query
                    .GroupBy(h => h.Member.City)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .FirstOrDefaultAsync(),

                TopTemplate = await query
                    .GroupBy(h => h.TemplateName)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .FirstOrDefaultAsync()
            };

            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Analytics retrieved successfully",
                Data = result
            });
        }

        private static async Task<IResult> GetActivityLogs(
            int page,
            int pageSize,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ClaimsPrincipal user)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return Results.BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Page and pageSize must be greater than zero"
                });
            }

            var isAdmin = user.IsInRole("Admin");
            var userCity = user.FindFirst("city")?.Value;

            var query = unitOfWork.PrintHistory.Query();

            if (!isAdmin && !string.IsNullOrWhiteSpace(userCity))
                query = query.Where(h => h.Member.City == userCity);

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(h => h.PrintedAtUtc)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dto = mapper.Map<List<PrintHistoryRowDto>>(items);

            var paged = new PagedResult<PrintHistoryRowDto>(dto, total, page, pageSize);

            return Results.Ok(new ApiResponse<PagedResult<PrintHistoryRowDto>>
            {
                Success = true,
                Message = "Activity logs retrieved successfully",
                Data = paged
            });
        }



    }

}