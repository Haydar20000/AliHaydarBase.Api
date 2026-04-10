using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Models.Common;
using AliHaydarBase.Api.Core.Models.Members;

namespace AliHaydarBase.Api.Core.Interfaces
{
    public interface IMemberRepository : IRepository<Member>
    {
        Task<PagedResult<Member>> GetPagedMembersAsync(int page, int pageSize, string search);
    }
}