using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace AliHaydarBase.Api.Core.Repositories
{
    public class RefreshTokenEntryRepositories : Repository<RefreshTokenEntry>, IRefreshTokenEntryRepository
    {
        public RefreshTokenEntryRepositories(DbContext context) : base(context)
        {

        }
    }
}