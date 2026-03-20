using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models.Blogs;
using AliHaydarBase.Api.Dependencies;

namespace AliHaydarBase.Api.Core.Repositories
{
    public class BlogsRepository : Repository<Blogs>, IBlogsRepository
    {
        public BlogsRepository(AliHaydarDbContext context) : base(context)
        {

        }
    }
}