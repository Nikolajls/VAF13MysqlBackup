using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace VAF13.Features.Queries;

public class IsSkywinRunning
{
    public class Query : IRequest<bool>
    { }

    public class Handler : IRequestHandler<Query, bool>
    {
        private readonly ILogger<Handler> _logger;

        public Handler(ILogger<Handler> logger)
        {
            _logger = logger;
        }

        public Task<bool> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = Process.GetProcessesByName("SkyWin").Length > 0;
            _logger.LogInformation("Skywin running process:{IsSkywinRunning}", result);
            return Task.FromResult(result);
        }
    }
}