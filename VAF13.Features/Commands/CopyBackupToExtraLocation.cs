using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VAF13.Domain.Settings;

namespace VAF13.Features.Commands;

public class CopyBackupToExtraLocation
{
    public class Command : IRequest
    {
        public string SourceFilePath { get; set; }
        public string SourceFilename { get; set; }
        public string SubDirectories { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly BackupSettings _settings;
        private readonly ILogger<Handler> _logger;

        public Handler(ILogger<Handler> logger, IOptions<BackupSettings> settings)
        {
            _logger = logger;
            _settings = settings.Value;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Command executed at: {time}", DateTimeOffset.Now);

            foreach (var extraPath in _settings.CopyBackupsTo)
            {
                var destinationPath = Path.Combine(extraPath, request.SubDirectories, request.SourceFilename);
                CopyFile(request.SourceFilePath, destinationPath);
            }
        }

        private void CopyFile(string sourcePath, string destinationPath)
        {
            try
            {
                var fi = new FileInfo(destinationPath);
                var dirExists = fi?.Directory?.Exists ?? false;
                if (!dirExists)
                {
                    _logger.LogInformation("Directory {path} for copying destination not existing",
                        destinationPath);
                    fi?.Directory?.Create();
                }

                _logger.LogInformation("Copying {SourcePath} to {DestinationPath}", sourcePath, destinationPath);
                File.Copy(sourcePath, destinationPath);
                _logger.LogInformation("Copied successful");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception in copy file {SourcePath} to {DestinationPath} - Message:{Message}", sourcePath, destinationPath, e.Message);
            }
        }
    }
}