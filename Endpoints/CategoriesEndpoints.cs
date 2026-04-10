using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models.Blogs;
using AliHaydarBase.Api.DTOs.Request.Blogs;
using AliHaydarBase.Api.DTOs.Response;
using AliHaydarBase.Api.DTOs.Response.Blogs;

namespace AliHaydarBase.Api.Endpoints
{
    public static class CategoriesEndpoints
    {
        public static void MapCategoriesEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("api/categories");

            group.MapPost("/", CreateCategory);
            group.MapGet("/", GetAllCategories);
            group.MapGet("/{id}", GetCategoryById);
            group.MapPut("/{id}", UpdateCategory);
            group.MapDelete("/{id}", DeleteCategory);
        }

        private static async Task<IResult> CreateCategory(CreateCategoryRequestDto dto, IUnitOfWork unitOfWork)
        {
            // 1. Validate DTO using DataAnnotations
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

            // 2. Check for duplicate category name
            var existing = await unitOfWork.Categories
                .SingleOrDefault(c => c.Name.ToLower() == dto.Name.ToLower());

            if (existing != null)
            {
                return Results.BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Category name already exists"
                });
            }

            // 3. Map DTO → Entity
            var category = new Categories
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                IconName = dto.IconName,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow
            };

            // 4. Save to database
            await unitOfWork.Categories.AddAsync(category);
            await unitOfWork.Complete();

            // 5. Map Entity → Response DTO
            var responseDto = new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                IconName = category.IconName,
                Description = category.Description
            };

            // 6. Return success response
            return Results.Ok(new ApiResponse<CategoryResponseDto>
            {
                Success = true,
                Message = "Category created successfully",
                Data = responseDto
            });
        }


        private static async Task<IResult> GetAllCategories(IUnitOfWork unitOfWork)
        {
            var categories = await unitOfWork.Categories.GetAllAsync();

            var response = categories.Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                IconName = c.IconName,
                Description = c.Description
            }).ToList();

            return Results.Ok(new ApiResponse<List<CategoryResponseDto>>
            {
                Success = true,
                Message = "Categories retrieved successfully",
                Data = response
            });
        }

        private static async Task<IResult> GetCategoryById(Guid id, IUnitOfWork unitOfWork)
        {
            // 1. Fetch category
            var category = await unitOfWork.Categories.GetByIdAsync(id);

            // 2. Handle not found
            if (category == null)
            {
                return Results.NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Category not found"
                });
            }

            // 3. Map entity → DTO
            var responseDto = new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                IconName = category.IconName,
                Description = category.Description
            };

            // 4. Return success
            return Results.Ok(new ApiResponse<CategoryResponseDto>
            {
                Success = true,
                Message = "Category retrieved successfully",
                Data = responseDto
            });
        }


        private static async Task<IResult> UpdateCategory(Guid id, UpdateCategoryRequestDto dto, IUnitOfWork unitOfWork)
        {
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
            var category = await unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
            {
                return Results.NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Category not found"
                });
            }

            // 3. Check for duplicate name (exclude current category)
            var duplicate = await unitOfWork.Categories
                .SingleOrDefault(c =>
                    c.Name.ToLower() == dto.Name.ToLower() &&
                    c.Id != id);

            if (duplicate != null)
            {
                return Results.BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Another category with this name already exists"
                });
            }

            // 4. Update fields
            category.Name = dto.Name;
            category.IconName = dto.IconName;
            category.Description = dto.Description;
            category.UpdatedAt = DateTime.UtcNow;

            // 5. Save changes
            await unitOfWork.Categories.UpdateAsync(category);
            await unitOfWork.Complete();

            // 6. Map to response DTO
            var responseDto = new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                IconName = category.IconName,
                Description = category.Description
            };

            // 7. Return success
            return Results.Ok(new ApiResponse<CategoryResponseDto>
            {
                Success = true,
                Message = "Category updated successfully",
                Data = responseDto
            });
        }


        private static async Task<IResult> DeleteCategory(Guid id, IUnitOfWork unitOfWork)
        {
            // 1. Check if category exists
            var category = await unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
            {
                return Results.NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Category not found"
                });
            }

            // 2. Delete category
            await unitOfWork.Categories.DeleteAsync(id);
            await unitOfWork.Complete();

            // 3. Return success
            return Results.Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Category deleted successfully"
            });
        }

    }

}