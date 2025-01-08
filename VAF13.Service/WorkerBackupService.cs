using MediatR;
using Microsoft.Extensions.Options;
using VAF13.Domain.Settings;
using VAF13.Features.Commands;

namespace VAF13.Service
{
    public class WorkerBackupService : BackgroundService
    {
        private readonly IMediator _mediator;
        private readonly BackupSettings _settings;
        private readonly ILogger<WorkerBackupService> _logger;

        public WorkerBackupService(ILogger<WorkerBackupService> logger, IMediator mediator,
            IOptions<BackupSettings> settings)
        {
            _logger = logger;
            _mediator = mediator;
            _settings = settings.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("WorkerBackupService running at: {time}", DateTimeOffset.Now);
                    try
                    {
                        await _mediator.Send(new RunBackup.Command(), stoppingToken);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Error running backup from the service");
                    }

                    var seconds = _settings.MinuteInterval * 60 * 1000;
                    _logger.LogInformation("Waiting {seconds} seconds or as minutes {minutes}", seconds,
                        _settings.MinuteInterval);
                    await Task.Delay(seconds, stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                // When the stopping token is canceled, for example, a call made from services.msc,
                // we shouldn't exit with a non-zero exit code. In other words, this is expected...
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                // Terminates this process and returns an exit code to the operating system.
                // This is required to avoid the 'BackgroundServiceExceptionBehavior', which
                // performs one of two scenarios:
                // 1. When set to "Ignore": will do nothing at all, errors cause zombie services.
                // 2. When set to "StopHost": will cleanly stop the host, and log errors.
                //
                // In order for the Windows Service Management system to leverage configured
                // recovery options, we need to terminate the process with a non-zero exit code.
                Environment.Exit(1);
            }
        }
    }
}
