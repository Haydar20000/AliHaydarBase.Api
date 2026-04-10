using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models.Members;
using AliHaydarBase.Api.Dependencies;
using Microsoft.EntityFrameworkCore;

namespace AliHaydarBase.Api.Core.Repositories
{
    public class IdCardTemplateRepository : Repository<IdCardTemplate>, IIdCardTemplateRepository
    {
        public IdCardTemplateRepository(AliHaydarDbContext context) : base(context) { }

        public async Task<IdCardTemplate?> GetTemplateAsync(Guid id)
        {
            return await Query(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<IdCardTemplate>> GetAllTemplatesAsync()
        {
            return await Query().OrderBy(t => t.Year).ToListAsync();
        }
    }


}