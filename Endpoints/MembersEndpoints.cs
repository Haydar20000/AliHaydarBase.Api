using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models.Common;
using AliHaydarBase.Api.DTOs.Response;
using AliHaydarBase.Api.DTOs.Response.Members;
using AutoMapper;

namespace AliHaydarBase.Api.Endpoints
{
    public static class MembersEndpoints
    {
        public static void MapMembersEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("api/members");
            group.MapGet("/", GetMembers);
            group.MapGet("/{id:guid}", GetMemberDetails);
        }

        // ---------------------------------------------------------
        // GET /api/members?page=1&pageSize=20&search=ali
        // ---------------------------------------------------------
        private static async Task<IResult> GetMembers(
            int page,
            int pageSize,
            string? search,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return Results.BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Page and pageSize must be greater than zero"
                });
            }

            var paged = await unitOfWork.Members.GetPagedMembersAsync(page, pageSize, search ?? "");

            var dtoItems = mapper.Map<List<MemberRowDto>>(paged.Items);

            var dto = new PagedResult<MemberRowDto>(
                dtoItems,
                paged.TotalCount,
                paged.Page,
                paged.PageSize
            );

            return Results.Ok(new ApiResponse<PagedResult<MemberRowDto>>
            {
                Success = true,
                Message = "Members retrieved successfully",
                Data = dto
            });
        }

        // ---------------------------------------------------------
        // GET /api/members/{id}
        // ---------------------------------------------------------
        private static async Task<IResult> GetMemberDetails(
            Guid id,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            var member = await unitOfWork.Members.GetByIdAsync(id);

            if (member == null)
            {
                return Results.NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Member not found"
                });
            }

            var dto = mapper.Map<MemberDetailsDto>(member);

            return Results.Ok(new ApiResponse<MemberDetailsDto>
            {
                Success = true,
                Message = "Member details retrieved successfully",
                Data = dto
            });
        }
    }

}