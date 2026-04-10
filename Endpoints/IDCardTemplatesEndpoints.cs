
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.DTOs.Response;
using AliHaydarBase.Api.DTOs.Response.Members;
using AutoMapper;

namespace AliHaydarBase.Api.Endpoints
{
    public static class IDCardTemplatesEndpoints
    {
        public static void MapIDCardTemplatesEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("api/idcard-templates");

            group.MapGet("/", GetAllTemplates);
            group.MapGet("/{id:guid}", GetTemplateById);
        }

        private static async Task<IResult> GetAllTemplates(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            var templates = await unitOfWork.IdCardTemplates.GetAllTemplatesAsync();

            var dtoList = mapper.Map<List<IdCardTemplateDto>>(templates);

            return Results.Ok(new ApiResponse<List<IdCardTemplateDto>>
            {
                Success = true,
                Message = "ID card templates retrieved successfully",
                Data = dtoList
            });
        }

        private static async Task<IResult> GetTemplateById(
            Guid id,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            var template = await unitOfWork.IdCardTemplates.GetTemplateAsync(id);

            if (template == null)
            {
                return Results.NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "ID card template not found"
                });
            }

            var dto = mapper.Map<IdCardTemplateDto>(template);

            return Results.Ok(new ApiResponse<IdCardTemplateDto>
            {
                Success = true,
                Message = "ID card template retrieved successfully",
                Data = dto
            });
        }
    }

}