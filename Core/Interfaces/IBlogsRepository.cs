using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Models.Blogs;
using AliHaydarBase.Api.DTOs.Response.Blogs;

namespace AliHaydarBase.Api.Core.Interfaces
{
    public interface IBlogsRepository : IRepository<Blogs>
    {
        // Get blog with category + images
        Task<Blogs?> GetBlogWithDetailsAsync(Guid id);

        // Get paginated blogs
        Task<PagedResult<Blogs>> GetPagedBlogsAsync(int page, int pageSize);

        // Get blogs by category
        Task<List<Blogs>> GetBlogsByCategoryAsync(Guid categoryId);

        // Search blogs
        Task<List<Blogs>> SearchBlogsAsync(string keyword);
    }
}