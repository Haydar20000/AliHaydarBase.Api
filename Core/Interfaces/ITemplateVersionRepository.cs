using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Models.Members;
using AliHaydarBase.Api.DTOs.Request.Template;

namespace AliHaydarBase.Api.Core.Interfaces
{
    public interface ITemplateVersionRepository : IRepository<TemplateVersion>
    {
        Task<List<TemplateVersion>> GetVersionsAsync(Guid templateId);
        Task<TemplateVersion?> GetVersionAsync(Guid templateId, int versionNumber);
        Task AddVersionAsync(TemplateVersion version);
    }
}