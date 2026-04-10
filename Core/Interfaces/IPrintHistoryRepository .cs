using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Models.Members;

namespace AliHaydarBase.Api.Core.Interfaces
{
    public interface IPrintHistoryRepository : IRepository<PrintHistory>
    {
        Task<List<PrintHistory>> GetByMemberAsync(Guid memberId);
        Task<List<PrintHistory>> GetByTemplateAsync(Guid templateId);
        Task<List<PrintHistory>> GetAllHistoryAsync();
    }
}