using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VAF13.Domain.Settings;
using VAF13.Features.Queries;

namespace VAF13.Features.Commands;

public class RunBackup
{
    public class Command : IRequest
    { }

    public class Handler : IRequestHandler<Command>
    {
        private readonly IMediator _mediator;
        private readonly BackupSettings _settings;
        private readonly ILogger<Handler> _logger;

        public Handler(ILogger<Handler> logger, IMediator mediator, IOptions<BackupSettings> settings)
        {
            _logger = logger;
            _mediator = mediator;
            _settings = settings.Value;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Backup command executed at: {time}", DateTimeOffset.Now);
            if (_settings.OnlyBackupWhenSkyWinIsRunning)
            {
                var isSkywinRunning = await _mediator.Send(new IsSkywinRunning.Query(), cancellationToken);
                _logger.LogInformation("SkyWin is running: {IsSkywinRunning} and settings says only backup when running", isSkywinRunning);
                if (!isSkywinRunning)
                    return;
            }

            var mysqldumpResult = await _mediator.Send(new BackupDatabase.Command(), cancellationToken);
            _logger.LogInformation("BackupDatabase result: {DumpFilename} {DumpFilePath}", mysqldumpResult.Filename, mysqldumpResult.FullPath);

            if (!mysqldumpResult.Success)
            {
                _logger.LogError("BackupDatabase mysqldump failed");
            }
            else if (_settings.CopyBackupsTo.Any())
            {
                _logger.LogInformation("Starting to copy to extra locations");
                await _mediator.Send(new CopyBackupToExtraLocation.Command
                {
                    SourceFilePath = mysqldumpResult.FullPath,
                    SourceFilename = mysqldumpResult.Filename,
                    SubDirectories = mysqldumpResult.Subdirs
                }, cancellationToken);
            }
            else
            {
                _logger.LogInformation("No extra locations to copy to");
            }
        }
    }
}