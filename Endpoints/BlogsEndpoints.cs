using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models.Blogs;
using AliHaydarBase.Api.DTOs.Request.Blogs;
using AliHaydarBase.Api.DTOs.Response.Blogs;

namespace AliHaydarBase.Api.Endpoints
{
    public static class BlogsEndpoints
    {
        public static void MapBlogsEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("api/blogs");

            group.MapPost("/", CreateBlog).RequireAuthorization(); // 🔥 Require auth for creating blogs
            group.MapGet("/", GetBlogs);
            group.MapGet("/{id}", GetBlogById);
            group.MapPut("/{id}", UpdateBlog);
            group.MapDelete("/{id}", DeleteBlog).RequireAuthorization(); // 🔥 Require auth for deleting blogs
        }

        // Methods will be added here...

        // Example: CreateBlog method with validation and user extraction from JWT
        private static async Task<IResult> CreateBlog(CreateBlogRequestDto dto, IUnitOfWork unitOfWork, ClaimsPrincipal user)
        {
            // 0. Extract UserId from JWT
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
            {
                return Results.Json(
                    new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Unauthorized user"
                    },
                    statusCode: StatusCodes.Status401Unauthorized
                );
            }

            // 1. Validate DTO
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(dto, context, results, true))
            {
                var errors = results.Select(r => r.ErrorMessage ?? "Invalid input").ToList();

                return Results.BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = errors
                });
            }

            // 2. Check if category exists
            var category = await unitOfWork.Categories.SingleOrDefault(c => c.Id == dto.CategoryId);
            if (category == null)
            {
                return Results.BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Category does not exist"
                });
            }

            // 3. Map DTO → Entity
            var blog = new Blogs
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Content = dto.Content,
                CategoryId = dto.CategoryId,
                UserId = userId,
                PublishedBy = user.Identity?.Name, // optional but useful
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsVisible = true,
                AuditLevel1Approved = true,
                AuditLevel2Approved = true,
                AuditLevel3Approved = true
            };

            // 4. Save blog
            await unitOfWork.Blogs.AddAsync(blog);
            await unitOfWork.Complete();

            // 5. Load blog with details (Category + Images)
            var blogWithDetails = await unitOfWork.Blogs.GetBlogWithDetailsAsync(blog.Id);

            // 6. Map Entity → Response DTO
            var responseDto = new BlogResponseDto
            {
                Id = blogWithDetails!.Id,
                Title = blogWithDetails.Title,
                Content = blogWithDetails.Content,

                CategoryId = blogWithDetails.CategoryId,
                CategoryName = blogWithDetails.Category?.Name ?? "",
                CategoryIcon = blogWithDetails.Category?.IconName ?? "",

                UserId = blogWithDetails.UserId,
                PublishedBy = blogWithDetails.PublishedBy,

                IsVisible = blogWithDetails.IsVisible,
                TargetRole = blogWithDetails.TargetRole,
                CreatedAt = blogWithDetails.CreatedAt,
                ExpireAt = blogWithDetails.ExpireAt,

                AuditLevel1Approved = blogWithDetails.AuditLevel1Approved,
                AuditLevel2Approved = blogWithDetails.AuditLevel2Approved,
                AuditLevel3Approved = blogWithDetails.AuditLevel3Approved,

                Images = blogWithDetails.Images.Select(i => new BlogImageResponseDto
                {
                    Id = i.Id,
                    ImageUrl = i.ImageUrl,
                    IsGallery = i.IsGallery
                }).ToList()
            };

            // 7. Return success
            return Results.Ok(new ApiResponse<BlogResponseDto>
            {
                Success = true,
                Message = "Blog created successfully",
                Data = responseDto
            });
        }

        // Example: GetBlogs method with pagination
        private static async Task<IResult> GetBlogs(int page, int pageSize, IUnitOfWork unitOfWork)
        {
            // 1. Validate paging parameters
            if (page <= 0 || pageSize <= 0)
            {
                return Results.BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Page and pageSize must be greater than zero"
                });
            }

            // 2. Fetch paginated blogs
            var pagedResult = await unitOfWork.Blogs.GetPagedBlogsAsync(page, pageSize);

            // 3. Map to DTOs
            var responseList = pagedResult.Items.Select(blog => new BlogResponseDto
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,

                CategoryId = blog.CategoryId,
                CategoryName = blog.Category?.Name ?? "",
                CategoryIcon = blog.Category?.IconName ?? "",

                UserId = blog.UserId,
                PublishedBy = blog.PublishedBy,

                IsVisible = blog.IsVisible,
                TargetRole = blog.TargetRole,
                CreatedAt = blog.CreatedAt,
                ExpireAt = blog.ExpireAt,

                AuditLevel1Approved = blog.AuditLevel1Approved,
                AuditLevel2Approved = blog.AuditLevel2Approved,
                AuditLevel3Approved = blog.AuditLevel3Approved,

                Images = blog.Images.Select(i => new BlogImageResponseDto
                {
                    Id = i.Id,
                    ImageUrl = i.ImageUrl,
                    IsGallery = i.IsGallery
                }).ToList()

            }).ToList();

            // 4. Build paginated response
            var response = new ApiResponse<object>
            {
                Success = true,
                Message = "Blogs retrieved successfully",
                Data = new
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = pagedResult.TotalCount,
                    TotalPages = (int)Math.Ceiling((double)pagedResult.TotalCount / pageSize),
                    Items = responseList
                }
            };

            // 5. Return success
            return Results.Ok(response);
        }

        // Example: GetBlogById method with details (Category + Images)
        private static async Task<IResult> GetBlogById(Guid id, IUnitOfWork unitOfWork)
        {
            // 1. Fetch blog with details
            var blog = await unitOfWork.Blogs.GetBlogWithDetailsAsync(id);

            // 2. Handle not found
            if (blog == null)
            {
                return Results.NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Blog not found"
                });
            }

            // 3. Map entity → DTO
            var responseDto = new BlogResponseDto
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,

                CategoryId = blog.CategoryId,
                CategoryName = blog.Category?.Name ?? "",
                CategoryIcon = blog.Category?.IconName ?? "",

                UserId = blog.UserId,
                PublishedBy = blog.PublishedBy,

                IsVisible = blog.IsVisible,
                TargetRole = blog.TargetRole,
                CreatedAt = blog.CreatedAt,
                ExpireAt = blog.ExpireAt,

                AuditLevel1Approved = blog.AuditLevel1Approved,
                AuditLevel2Approved = blog.AuditLevel2Approved,
                AuditLevel3Approved = blog.AuditLevel3Approved,

                Images = blog.Images.Select(i => new BlogImageResponseDto
                {
                    Id = i.Id,
                    ImageUrl = i.ImageUrl,
                    IsGallery = i.IsGallery
                }).ToList()
            };

            // 4. Return success
            return Results.Ok(new ApiResponse<BlogResponseDto>
            {
                Success = true,
                Message = "Blog retrieved successfully",
                Data = responseDto
            });
        }

        // Example: UpdateBlog method with validation, user extraction, and category existence check
        private static async Task<IResult> UpdateBlog(Guid id, UpdateBlogRequestDto dto, IUnitOfWork unitOfWork, ClaimsPrincipal user)
        {
            // 0. Extract UserId from JWT
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
            {
                return Results.Unauthorized();
            }

            // 1. Validate DTO
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(dto, context, results, true))
            {
                var errors = results.Select(r => r.ErrorMessage ?? "Invalid input").ToList();

                return Results.BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = errors
                });
            }

            // 2. Check if blog exists
            var blog = await unitOfWork.Blogs.GetBlogWithDetailsAsync(id);

            if (blog == null)
            {
                return Results.NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Blog not found"
                });
            }

            // 3. Check if category exists
            var category = await unitOfWork.Categories.SingleOrDefault(c => c.Id == dto.CategoryId);
            if (category == null)
            {
                return Results.BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Category does not exist"
                });
            }

            // 4. Update fields
            blog.Title = dto.Title;
            blog.Content = dto.Content;
            blog.CategoryId = dto.CategoryId;
            blog.IsVisible = dto.IsVisible;
            blog.TargetRole = dto.TargetRole;
            blog.ExpireAt = dto.ExpireAt;
            blog.UpdatedAt = DateTime.UtcNow;

            // Optional: update PublishedBy if admin edits
            blog.PublishedBy = user.Identity?.Name;

            // 5. Save changes
            await unitOfWork.Blogs.UpdateAsync(blog);
            await unitOfWork.Complete();

            // 6. Map to response DTO
            var responseDto = new BlogResponseDto
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,

                CategoryId = blog.CategoryId,
                CategoryName = blog.Category?.Name ?? "",
                CategoryIcon = blog.Category?.IconName ?? "",

                UserId = blog.UserId,
                PublishedBy = blog.PublishedBy,

                IsVisible = blog.IsVisible,
                TargetRole = blog.TargetRole,
                CreatedAt = blog.CreatedAt,
                ExpireAt = blog.ExpireAt,

                AuditLevel1Approved = blog.AuditLevel1Approved,
                AuditLevel2Approved = blog.AuditLevel2Approved,
                AuditLevel3Approved = blog.AuditLevel3Approved,

                Images = blog.Images.Select(i => new BlogImageResponseDto
                {
                    Id = i.Id,
                    ImageUrl = i.ImageUrl,
                    IsGallery = i.IsGallery
                }).ToList()
            };

            // 7. Return success
            return Results.Ok(new ApiResponse<BlogResponseDto>
            {
                Success = true,
                Message = "Blog updated successfully",
                Data = responseDto
            });
        }

        // Example: DeleteBlog method with existence check and cascade delete handling
        private static async Task<IResult> DeleteBlog(Guid id, IUnitOfWork unitOfWork)
        {
            // 1. Check if blog exists (with images if needed)
            var blog = await unitOfWork.Blogs.GetBlogWithDetailsAsync(id);

            if (blog == null)
            {
                return Results.NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Blog not found"
                });
            }

            // 🔹 NOTE:
            // For now we do a normal delete.
            // Later, when ImagesGallery + Azure Storage are implemented,
            // we will:
            // - Keep images where IsGallery == true (move them to ImagesGallery)
            // - Delete only non-gallery images from storage.

            // 2. Delete blog (and related images via cascade or repo logic)
            await unitOfWork.Blogs.DeleteAsync(id);
            await unitOfWork.Complete();

            // 3. Return success
            return Results.Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Blog deleted successfully"
            });
        }
    }
}