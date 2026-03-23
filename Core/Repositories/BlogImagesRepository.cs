using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models.Blogs;
using AliHaydarBase.Api.Dependencies;
using Microsoft.EntityFrameworkCore;

namespace AliHaydarBase.Api.Core.Repositories
{
    public class BlogImagesRepository : Repository<BlogImages>, IBlogImagesRepository
    {
        private readonly AliHaydarDbContext _context;

        public BlogImagesRepository(AliHaydarDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<BlogImages>> GetImagesByBlogIdAsync(Guid blogId)
        {
            return await _context.BlogImages
                .Where(i => i.BlogId == blogId)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();
        }
    }
}