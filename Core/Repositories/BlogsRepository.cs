using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models.Blogs;
using AliHaydarBase.Api.Dependencies;
using AliHaydarBase.Api.DTOs.Response.Blogs;
using Microsoft.EntityFrameworkCore;

namespace AliHaydarBase.Api.Core.Repositories
{
    public class BlogsRepository : Repository<Blogs>, IBlogsRepository
    {
        public BlogsRepository(AliHaydarDbContext context) : base(context)
        {

        }

        public async Task<Blogs?> GetBlogWithDetailsAsync(Guid id)
        {
            return await Query(b => b.Id == id)
                .Include(b => b.Category)
                .Include(b => b.Images)
                .FirstOrDefaultAsync();
        }

        public async Task<PagedResult<Blogs>> GetPagedBlogsAsync(int page, int pageSize)
        {
            var query = Query()
                .Include(b => b.Category)
                .Include(b => b.Images)
                .OrderByDescending(b => b.CreatedAt);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Blogs>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public async Task<List<Blogs>> GetBlogsByCategoryAsync(Guid categoryId)
        {
            return await Query(b => b.CategoryId == categoryId)
                .Include(b => b.Images)
                .Include(b => b.Category)
                .ToListAsync();
        }

        public async Task<List<Blogs>> SearchBlogsAsync(string keyword)
        {
            keyword = keyword.ToLower();

            return await Query(b =>
                    b.Title.ToLower().Contains(keyword) ||
                    b.Content.ToLower().Contains(keyword))
                .Include(b => b.Images)
                .Include(b => b.Category)
                .ToListAsync();
        }
    }
}