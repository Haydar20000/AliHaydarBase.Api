using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models.Members;
using AliHaydarBase.Api.DTOs.Response;
using AliHaydarBase.Api.DTOs.Response.Members;
using AutoMapper;

namespace AliHaydarBase.Api.Endpoints
{
    public static class PrintHistoryEndpoints
    {
        public static void MapPrintHistoryEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("api/print-history");

            group.MapGet("/member/{memberId:guid}", GetByMember);
            group.MapGet("/template/{templateId:guid}", GetByTemplate);
            group.MapGet("/", GetAllHistory);

            group.MapPost("/", AddPrintHistory).RequireAuthorization();
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
        // GET /api/print-history
        // ---------------------------------------------------------
        private static async Task<IResult> GetAllHistory(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            var history = await unitOfWork.PrintHistory.GetAllHistoryAsync();

            var dto = mapper.Map<List<PrintHistoryRecord>>(history);

            return Results.Ok(new ApiResponse<List<PrintHistoryRecord>>
            {
                Success = true,
                Message = "All print history retrieved successfully",
                Data = dto
            });
        }

        // ---------------------------------------------------------
        // POST /api/print-history
        // Body: { memberId, templateId }
        // ---------------------------------------------------------
        private static async Task<IResult> AddPrintHistory(
            PrintHistoryRecord record,
            IUnitOfWork unitOfWork,
            ClaimsPrincipal user)
        {
            // Extract the authenticated user's ID (Guid)
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdString, out Guid userId))
                return Results.Unauthorized();

            // Validate input
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
                PrintedAt = DateTime.UtcNow,
                PrintedBy = userId
            };

            await unitOfWork.PrintHistory.AddAsync(entity);
            await unitOfWork.Complete();

            return Results.Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Print history added successfully"
            });
        }
    }

}