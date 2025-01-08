using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using VAF13.Domain.Settings;

namespace VAF13.Features.Commands;

public class BackupDatabase
{
    public class Command : IRequest<Result>
    {
    }

    public class Result
    {
        public bool Success { get; set; }
        public string Subdirs { get; set; }

        public string Filename { get; set; }
        public string FullPath { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly IMediator _mediator;
        private readonly MysqlDumpSettings _settings;
        private readonly BackupSettings _backupSettings;
        private readonly ILogger<Handler> _logger;

        public Handler(ILogger<Handler> logger, IMediator mediator, IOptions<MysqlDumpSettings> settings, IOptions<BackupSettings> backupSettings)
        {
            _logger = logger;
            _mediator = mediator;
            _settings = settings.Value;
            _backupSettings = backupSettings.Value;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Command executed at: {time}", DateTimeOffset.Now);

            var dateTime = DateTime.Now;
            var fileName = $"{dateTime.ToString("s").Replace(":", "-")}.sql";
            var subDirectories = Path.Combine(dateTime.Year.ToString(), dateTime.Month.ToString(), dateTime.Day.ToString());
            var initialFile = Path.Combine(_backupSettings.SaveBackupPath, subDirectories, fileName);

            var result = new Result
            {
                Subdirs = subDirectories,
                Filename = fileName,
                FullPath = initialFile
            };

            _logger.LogInformation("Starting to run mysqldump of database");
            var executable = _settings.Executable;
            var arguments = $"""-h {_settings.Host} -P {_settings.Port} -u {_settings.User} -p{_settings.Password} --databases {_settings.Database} -r "{initialFile}" {_settings.ExtraArguments} """;
            _logger.LogInformation("Starting dump {executable} with arguments: {arguments}", executable, arguments);
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = executable,
                    Arguments = arguments,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                var p = Process.Start(processStartInfo);
                var standardOutput = p?.StandardOutput.ReadToEndAsync(cancellationToken);
                await p?.WaitForExitAsync(cancellationToken);
                result.Success = true;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Exception in dump mysql database {message}", e.Message);
            }

            return result;
        }
    }
}