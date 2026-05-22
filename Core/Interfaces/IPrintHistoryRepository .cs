using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Models.Common;
using AliHaydarBase.Api.Core.Models.Members;
using AliHaydarBase.Api.Core.Models.MonitoringData;
using AliHaydarBase.Api.DTOs.Response.Members;

namespace AliHaydarBase.Api.Core.Interfaces
{
    public interface IPrintHistoryRepository : IRepository<PrintHistory>
    {
        Task<List<PrintHistory>> GetByMemberAsync(Guid memberId);
        Task<List<PrintHistory>> GetByTemplateAsync(Guid templateId);
        Task<List<PrintHistory>> GetAllHistoryAsync();

        Task<PagedResult<PrintHistory>> GetPagedHistoryAsync(
        int page,
        int pageSize,
        string? city,
        string? search,
        string? template,
        string? mode,
        DateTime? from,
        DateTime? to,
        string sortBy,
        string sortDir);

    }
}