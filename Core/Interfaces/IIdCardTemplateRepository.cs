using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Models.Members;

namespace AliHaydarBase.Api.Core.Interfaces
{
    public interface IIdCardTemplateRepository : IRepository<IdCardTemplate>
    {
        Task<IdCardTemplate?> GetTemplateAsync(Guid id);
        Task<List<IdCardTemplate>> GetAllTemplatesAsync();
    }
}