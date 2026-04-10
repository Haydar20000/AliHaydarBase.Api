using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models.Blogs;
using AliHaydarBase.Api.DTOs.Request.Blogs;
using AliHaydarBase.Api.DTOs.Response;
using AliHaydarBase.Api.DTOs.Response.Blogs;

namespace AliHaydarBase.Api.Endpoints
{
    public static class BlogImagesEndpoints
    {
        public static void MapBlogImagesEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("api/blogs");
            group.MapPost("/{blogId}/upload", UploadBlogImage).RequireAuthorization(); // 🔥 Require auth for uploading images
            group.MapGet("/{blogId}", GetBlogImages);
            group.MapDelete("/{imageId}", DeleteBlogImage);
            group.MapPut("/{imageId}/set-gallery", SetImageAsGallery);
        }
        // Example: UploadBlogImage method with file handling, validation, and response mapping
        private static async Task<IResult> UploadBlogImage(
    Guid blogId,
    UploadBlogImageRequestDto dto,
    IUnitOfWork unitOfWork,
    IWebHostEnvironment env,
    ClaimsPrincipal user)
        {
            // Extract userId from JWT
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

            // Check if blog exists
            var blog = await unitOfWork.Blogs.SingleOrDefault(b => b.Id == blogId);
            if (blog == null)
            {
                return Results.NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Blog not found"
                });
            }

            // Validate file
            if (dto.File == null || dto.File.Length == 0)
            {
                return Results.BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Invalid file"
                });
            }

            // Save file locally
            var uploadsFolder = Path.Combine(env.WebRootPath, "uploads", "blogs");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.File.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            var imageUrl = $"/uploads/blogs/{fileName}";

            // Create BlogImages record
            var image = new BlogImages
            {
                Id = Guid.NewGuid(),
                BlogId = blogId,
                Blog = blog,
                UserId = userId,
                ImageUrl = imageUrl,
                Caption = null,
                IsGallery = dto.IsGallery,
                CreatedAt = DateTime.UtcNow
            };

            await unitOfWork.BlogImages.AddAsync(image);
            await unitOfWork.Complete();

            return Results.Ok(new ApiResponse<BlogImageResponseDto>
            {
                Success = true,
                Message = "Image uploaded successfully",
                Data = new BlogImageResponseDto
                {
                    Id = image.Id,
                    ImageUrl = image.ImageUrl,
                    Caption = image.Caption,
                    IsGallery = image.IsGallery,
                    CreatedAt = image.CreatedAt,
                    UpdatedAt = image.UpdatedAt
                }
            });
        }

        // Example: GetBlogImages method to retrieve all images for a blog and return as DTOs
        private static async Task<IResult> GetBlogImages(Guid blogId, IUnitOfWork unitOfWork)
        {
            var images = await unitOfWork.BlogImages.GetImagesByBlogIdAsync(blogId);

            var response = images.Select(i => new BlogImageResponseDto
            {
                Id = i.Id,
                ImageUrl = i.ImageUrl,
                Caption = i.Caption,
                IsGallery = i.IsGallery,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt
            }).ToList();

            return Results.Ok(new ApiResponse<List<BlogImageResponseDto>>
            {
                Success = true,
                Message = "Images retrieved successfully",
                Data = response
            });
        }

        // Example: DeleteBlogImage method to delete an image by ID, including file deletion and DB record removal
        private static async Task<IResult> DeleteBlogImage(Guid imageId, IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            var image = await unitOfWork.BlogImages.SingleOrDefault(i => i.Id == imageId);

            if (image == null)
            {
                return Results.NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Image not found"
                });
            }

            // Delete local file
            var filePath = Path.Combine(env.WebRootPath, image.ImageUrl.TrimStart('/'));
            if (File.Exists(filePath))
                File.Delete(filePath);

            // Delete DB record
            await unitOfWork.BlogImages.DeleteAsync(imageId);
            await unitOfWork.Complete();

            return Results.Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Image deleted successfully"
            });
        }

        // Example: SetImageAsGallery method to mark an image as part of the gallery and update the record
        private static async Task<IResult> SetImageAsGallery(Guid imageId, IUnitOfWork unitOfWork)
        {
            var image = await unitOfWork.BlogImages.SingleOrDefault(i => i.Id == imageId);

            if (image == null)
            {
                return Results.NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Image not found"
                });
            }

            image.IsGallery = true;
            image.UpdatedAt = DateTime.UtcNow;

            await unitOfWork.BlogImages.UpdateAsync(image);
            await unitOfWork.Complete();

            return Results.Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Image marked as gallery"
            });
        }

    }
}