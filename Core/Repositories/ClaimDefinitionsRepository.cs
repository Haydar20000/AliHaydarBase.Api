using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace AliHaydarBase.Api.Core.Repositories
{
    public class ClaimDefinitionsRepository : Repository<ClaimDefinition>, IClaimDefinitionsRepository
    {
        public ClaimDefinitionsRepository(DbContext context) : base(context)
        {

        }
    }
}