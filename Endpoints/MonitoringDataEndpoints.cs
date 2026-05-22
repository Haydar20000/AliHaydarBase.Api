

using System.Security.Claims;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models.MonitoringData;
using AliHaydarBase.Api.DTOs.Request.MonitoringDataDtos;
using AliHaydarBase.Api.DTOs.Response;
using AutoMapper;

namespace AliHaydarBase.Api.Endpoints
{
    public static class MonitoringDataEndpoints
    {
        public static void MapMonitoringDataEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("api/monitoring");

            group.MapPost("/print-history", CreatePrintHistory).RequireAuthorization();
            group.MapGet("/print-history", GetPrintHistory).RequireAuthorization();
            group.MapGet("/print-history/{id:guid}", GetPrintHistoryById).RequireAuthorization();
            group.MapDelete("/print-history/{id:guid}", DeletePrintHistory).RequireAuthorization();
        }

        // ---------------------------------------------------------
        // POST /api/monitoring/print-history
        // ---------------------------------------------------------
        private static async Task<IResult> CreatePrintHistory(
            PrintHistoryCreateDto dto,
            IUnitOfWork unitOfWork,
            ClaimsPrincipal user)
        {
            // Extract userId from JWT
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
            {
                return Results.Json(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Unauthorized user"
                }, statusCode: StatusCodes.Status401Unauthorized);
            }

            var entity = new PrintHistory
            {
                Id = Guid.NewGuid(),
                MemberId = dto.MemberId,
                MemberName = dto.MemberName,
                TemplateId = dto.TemplateId,
                TemplateName = dto.TemplateName,
                PrintMode = dto.PrintMode,
                PrintedAtUtc = DateTime.UtcNow,
                FrontThumbnailBase64 = dto.FrontThumbnailBase64,
                BackThumbnailBase64 = dto.BackThumbnailBase64,
                UserId = userId
            };

            await unitOfWork.PrintHistory.AddAsync(entity);
            await unitOfWork.Complete();

            return Results.Ok(new ApiResponse<Guid>
            {
                Success = true,
                Message = "Print history saved successfully",
                Data = entity.Id
            });
        }

        // ---------------------------------------------------------
        // GET /api/monitoring/print-history
        // ---------------------------------------------------------
        private static async Task<IResult> GetPrintHistory(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            var items = await unitOfWork.PrintHistory.GetAllAsync();

            var dto = mapper.Map<List<PrintHistoryRowDto>>(items);

            return Results.Ok(new ApiResponse<List<PrintHistoryRowDto>>
            {
                Success = true,
                Message = "Print history retrieved successfully",
                Data = dto
            });
        }

        // ---------------------------------------------------------
        // GET /api/monitoring/print-history/{id}
        // ---------------------------------------------------------
        private static async Task<IResult> GetPrintHistoryById(
            Guid id,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            var item = await unitOfWork.PrintHistory.GetByIdAsync(id);

            if (item == null)
            {
                return Results.NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Print history record not found"
                });
            }

            var dto = mapper.Map<PrintHistoryDetailsDto>(item);

            return Results.Ok(new ApiResponse<PrintHistoryDetailsDto>
            {
                Success = true,
                Message = "Print history details retrieved successfully",
                Data = dto
            });
        }

        // ---------------------------------------------------------
        // DELETE /api/monitoring/print-history/{id}
        // ---------------------------------------------------------
        private static async Task<IResult> DeletePrintHistory(
            Guid id,
            IUnitOfWork unitOfWork)
        {
            var item = await unitOfWork.PrintHistory.GetByIdAsync(id);

            if (item == null)
            {
                return Results.NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Print history record not found"
                });
            }

            await unitOfWork.PrintHistory.DeleteAsync(id);
            await unitOfWork.Complete();

            return Results.Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Print history deleted successfully"
            });
        }
    }
}
