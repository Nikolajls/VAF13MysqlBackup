using Serilog;
using VAF13.Domain.Settings;
using VAF13.Features;
using VAF13.Service;

var builder = Host.CreateApplicationBuilder(args);

var services = builder.Services;
builder.Services.Configure<BackupSettings>(builder.Configuration.GetSection("BackupSettings"));
builder.Services.Configure<MysqlDumpSettings>(builder.Configuration.GetSection("MysqlDumpSettings"));

builder.Services.AddSerilog(lc => lc.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddWindowsService(options =>
{
    options.ServiceName = ".NET VAF13 BackupService";
});

builder.Services.AddHostedService<WorkerBackupService>();
services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<FeaturesAssemblyAnchor>());

var host = builder.Build();
host.Run();
