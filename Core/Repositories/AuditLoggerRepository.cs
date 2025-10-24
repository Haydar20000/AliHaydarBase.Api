using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models;
using AliHaydarBase.Api.Dependencies;
using Microsoft.EntityFrameworkCore;

namespace AliHaydarBase.Api.Core.Repositories
{
    public class AuditLoggerRepository : Repository<AuditLog>, IAuditLoggerRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuditLoggerRepository(AliHaydarDbContext context, IUnitOfWork unitOfWork) : base(context)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task LogAsync(string userId, string action, string description, string ip, string deviceId, object? metadata = null)
        {
            var log = new AuditLog
            {
                UserId = userId,
                Action = action,
                Description = description,
                IpAddress = ip,
                DeviceId = deviceId,
                Metadata = metadata is null ? "" : JsonSerializer.Serialize(metadata)
            };

            await _unitOfWork.AuditLogger.AddAsync(log);
            await _unitOfWork.Complete();

        }
    }
}