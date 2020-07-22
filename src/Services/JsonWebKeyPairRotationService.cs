using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspNetCore.JsonWebKeys.Services
{
    public class JsonWebKeyPairRotationService : BackgroundService
    {
        private readonly JsonWebKeyPairManagerService _keyPairManager;
        private readonly ILogger _logger;

        public JsonWebKeyPairRotationService(JsonWebKeyPairManagerService keyPairManager, IServiceProvider serviceProvider)
        {
            _keyPairManager = keyPairManager;
            _logger = serviceProvider.GetService<ILogger<JsonWebKeyPairRotationService>>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"JsonWebKeyPairRotationService is starting.");

            stoppingToken.Register(() =>
                _logger.LogDebug($"JWK rotation background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug($"Attempting JWK rotation.");

                _keyPairManager.RotateKeys();

                await Task.Delay(3600000, stoppingToken);
            }

            _logger.LogDebug($"JWK rotation background task is stopping.");
        }
    }
}
