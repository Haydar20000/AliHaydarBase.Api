using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models.Common;
using AliHaydarBase.Api.DTOs.Response;
using AliHaydarBase.Api.DTOs.Response.Members;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AliHaydarBase.Api.Endpoints
{
    public static class MembersEndpoints
    {
        public static void MapMembersEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("api/members");

            // Existing endpoints
            group.MapGet("/", GetMembers);
            group.MapGet("/{id:guid}", GetMemberDetails);

            // NEW ADMIN ENDPOINTS
            group.MapPost("/approve-reprint/{id:guid}", ApproveReprint)
                 .RequireAuthorization(); // Admin only

            group.MapPost("/block/{id:guid}", BlockMember)
                 .RequireAuthorization(); // Admin only

            group.MapPost("/unblock/{id:guid}", UnblockMember)
                 .RequireAuthorization(); // Admin only
                                          // Fetch multiple members by IDs (for Print Selected)
            group.MapPost("/ids", GetMembersBatch);
            group.MapPost("/print", GetMembersForPrint);
            // GET /api/members/all
            group.MapGet("/all", GetAllMembers);

        }
        private static async Task<IResult> GetAllMembers(IUnitOfWork repo)
        {
            try
            {
                var members = await repo.Members.GetAllMembersAsync();

                var xx = Results.Ok(new ApiResponse<List<MemberRowDto>>
                {
                    Success = true,
                    Message = "Members loaded successfully.",
                    Data = members,
                    Errors = null
                });
                return xx;
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new ApiResponse<List<MemberRowDto>>
                {
                    Success = false,
                    Message = "Failed to load members.",
                    Data = null,
                    Errors = new List<string> { ex.Message }
                });
            }
        }


        private static async Task<IResult> GetMembersForPrint(IUnitOfWork repo, List<Guid> memberIds)
        {
            var members = await repo.Members.GetMembersForPrintAsync(memberIds);
            return Results.Ok(members);
        }


        // ---------------------------------------------------------
        // GET /api/members?page=1&pageSize=20&search=ali
        // ---------------------------------------------------------
        private static async Task<IResult> GetMembers(
            int page,
            int pageSize,
            string? search,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ClaimsPrincipal user)
        {
            Console.WriteLine("BACKEND REQUEST RECEIVED: " + DateTime.Now.ToString("HH:mm:ss.fff"));

            if (page <= 0 || pageSize <= 0)
            {
                return Results.BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Page and pageSize must be greater than zero"
                });
            }

            // 1. Extract user city
            var userCity = user.FindFirst("city")?.Value;

            // 2. Check if user is admin
            var isAdmin = user.IsInRole("Admin");

            // 3. Fetch paged members
            var paged = await unitOfWork.Members.GetPagedMembersAsync(page, pageSize, search ?? "");

            // 4. Filter by city if NOT admin
            if (!isAdmin && !string.IsNullOrEmpty(userCity))
            {
                paged.Items = paged.Items.Where(m => m.City == userCity).ToList();
                paged.TotalCount = paged.Items.Count;
            }

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

        // ---------------------------------------------------------
        // POST /api/members/approve-reprint/{id}
        // ---------------------------------------------------------
        private static async Task<IResult> ApproveReprint(
            Guid id,
            IUnitOfWork unitOfWork)
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

            // Admin approves reprint
            member.IsBlockedByAdmin = false;
            member.IsIdPrinted = false;

            await unitOfWork.Members.UpdateAsync(member);
            await unitOfWork.Complete();

            return Results.Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Reprint approved. Member can print again."
            });
        }

        // ---------------------------------------------------------
        // POST /api/members/block/{id}
        // ---------------------------------------------------------
        private static async Task<IResult> BlockMember(
            Guid id,
            IUnitOfWork unitOfWork)
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

            member.IsBlockedByAdmin = true;

            await unitOfWork.Members.UpdateAsync(member);
            await unitOfWork.Complete();

            return Results.Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Member blocked successfully"
            });
        }
        // ---------------------------------------------------------
        // POST /api/members/batch
        // ---------------------------------------------------------
        private static async Task<IResult> GetMembersBatch(IUnitOfWork repo, List<Guid> memberIds)
        {
            var members = await repo.Members.GetMembersByIdsAsync(memberIds);
            return Results.Ok(members);
        }
        // ---------------------------------------------------------
        // POST /api/members/unblock/{id}
        // ---------------------------------------------------------
        private static async Task<IResult> UnblockMember(
            Guid id,
            IUnitOfWork unitOfWork)
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

            member.IsBlockedByAdmin = false;

            await unitOfWork.Members.UpdateAsync(member);
            await unitOfWork.Complete();

            return Results.Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Member unblocked successfully"
            });
        }

    }

}