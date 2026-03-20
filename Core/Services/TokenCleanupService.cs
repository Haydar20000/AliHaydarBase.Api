using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Interfaces;

namespace AliHaydarBase.Api.Core.Services
{
    public class TokenCleanupService : BackgroundService
    {
        private readonly IRefreshTokenEntryRepository _refreshTokenRepo;
        private readonly ILogger<TokenCleanupService> _logger;

        public TokenCleanupService(IRefreshTokenEntryRepository refreshTokenRepo, ILogger<TokenCleanupService> logger)
        {
            _refreshTokenRepo = refreshTokenRepo;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var cutoff = DateTime.UtcNow.AddDays(-30); // keep 30 days history
                var oldTokens = await _refreshTokenRepo.GetRevokedTokensOlderThanAsync(cutoff);

                if (oldTokens.Any())
                {
                    await _refreshTokenRepo.DeleteRangeAsync(oldTokens);
                    _logger.LogInformation($"🧹 Cleaned up {oldTokens.Count()} revoked refresh tokens older than {cutoff}");
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // run daily
            }
        }
    }
}