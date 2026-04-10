using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Interfaces;

namespace AliHaydarBase.Api.Core.Services
{
    public class TokenCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TokenCleanupService> _logger;

        public TokenCleanupService(IServiceScopeFactory scopeFactory, ILogger<TokenCleanupService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var repo = scope.ServiceProvider.GetRequiredService<IRefreshTokenEntryRepository>();

                        var cutoff = DateTime.UtcNow.AddDays(-30);
                        var oldTokens = await repo.GetRevokedTokensOlderThanAsync(cutoff);

                        if (oldTokens.Any())
                        {
                            await repo.DeleteRangeAsync(oldTokens);
                            _logger.LogInformation($"🧹 Cleaned up {oldTokens.Count()} revoked refresh tokens older than {cutoff}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while cleaning up refresh tokens");
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}