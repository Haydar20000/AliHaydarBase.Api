using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models.Members;
using AliHaydarBase.Api.DTOs.Request.Template;
using AliHaydarBase.Api.DTOs.Response;
using AutoMapper;

namespace AliHaydarBase.Api.Endpoints
{
    public static class TemplateVersionsEndpoints
    {
        public static void MapTemplateVersionsEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("api/idcard-templates/{templateId:guid}/versions");

            group.MapGet("/", GetAllVersions);
            group.MapGet("/{versionNumber:int}", GetVersionByNumber);
            group.MapPost("/", CreateVersion);
            group.MapPost("/{versionNumber:int}/restore", RestoreVersion);
        }

        // ---------------------------------------------------------
        // GET /api/idcard-templates/{templateId}/versions
        // ---------------------------------------------------------
        private static async Task<IResult> GetAllVersions(
            Guid templateId,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            var versions = await unitOfWork.TemplateVersions.GetVersionsAsync(templateId);

            var dtoList = mapper.Map<List<TemplateVersionDto>>(versions);

            return Results.Ok(new ApiResponse<List<TemplateVersionDto>>
            {
                Success = true,
                Message = "Template versions retrieved successfully",
                Data = dtoList
            });
        }

        // ---------------------------------------------------------
        // GET /api/idcard-templates/{templateId}/versions/{versionNumber}
        // ---------------------------------------------------------
        private static async Task<IResult> GetVersionByNumber(
            Guid templateId,
            int versionNumber,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            var version = await unitOfWork.TemplateVersions.GetVersionAsync(templateId, versionNumber);

            if (version == null)
            {
                return Results.NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Version not found"
                });
            }

            var dto = mapper.Map<TemplateVersionDto>(version);

            return Results.Ok(new ApiResponse<TemplateVersionDto>
            {
                Success = true,
                Message = "Version retrieved successfully",
                Data = dto
            });
        }

        // ---------------------------------------------------------
        // POST /api/idcard-templates/{templateId}/versions
        // ---------------------------------------------------------
        private static async Task<IResult> CreateVersion(
            Guid templateId,
            TemplateVersionDto dto,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            var version = mapper.Map<TemplateVersion>(dto);
            version.TemplateId = templateId;
            version.CreatedAt = DateTime.UtcNow;

            await unitOfWork.TemplateVersions.AddVersionAsync(version);
            await unitOfWork.Complete();

            return Results.Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Version created successfully"
            });
        }

        // ---------------------------------------------------------
        // POST /api/idcard-templates/{templateId}/versions/{versionNumber}/restore
        // ---------------------------------------------------------
        private static async Task<IResult> RestoreVersion(
            Guid templateId,
            int versionNumber,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            var version = await unitOfWork.TemplateVersions.GetVersionAsync(templateId, versionNumber);

            if (version == null)
            {
                return Results.NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Version not found"
                });
            }

            var template = await unitOfWork.IdCardTemplates.GetTemplateAsync(templateId);

            if (template == null)
            {
                return Results.NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Template not found"
                });
            }

            // Restore JSON
            template.FrontLayoutJson = version.FrontLayoutJson;
            template.BackLayoutJson = version.BackLayoutJson;

            await unitOfWork.Complete();

            return Results.Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Template restored successfully"
            });
        }
    }
}
