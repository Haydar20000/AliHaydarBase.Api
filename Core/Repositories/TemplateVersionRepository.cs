using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models.Members;
using AliHaydarBase.Api.Dependencies;

namespace AliHaydarBase.Api.Core.Repositories
{
    public class TemplateVersionRepository : Repository<TemplateVersion>, ITemplateVersionRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public TemplateVersionRepository(AliHaydarDbContext context) : base(context)
        {
        }

        // ---------------------------------------------------------
        // Get all versions for a template (newest → oldest)
        // ---------------------------------------------------------
        public async Task<List<TemplateVersion>> GetVersionsAsync(Guid templateId)
        {
            var list = await FindAsync(v => v.TemplateId == templateId);
            return list
                .OrderByDescending(v => v.VersionNumber)
                .ToList();
        }

        // ---------------------------------------------------------
        // Get a specific version
        // ---------------------------------------------------------
        public async Task<TemplateVersion?> GetVersionAsync(Guid templateId, int versionNumber)
        {
            var list = await FindAsync(v =>
                v.TemplateId == templateId &&
                v.VersionNumber == versionNumber
            );

            return list.FirstOrDefault();
        }

        // ---------------------------------------------------------
        // Add a new version
        // (base.AddAsync already saves changes)
        // ---------------------------------------------------------
        public async Task AddVersionAsync(TemplateVersion version)
        {
            await AddAsync(version); // inherited from Repository<TEntity>
        }
    }
}



