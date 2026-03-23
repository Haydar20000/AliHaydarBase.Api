using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Models.Blogs;

namespace AliHaydarBase.Api.Core.Interfaces
{
    public interface IBlogImagesRepository : IRepository<BlogImages>
    {
        Task<List<BlogImages>> GetImagesByBlogIdAsync(Guid blogId);
    }
}