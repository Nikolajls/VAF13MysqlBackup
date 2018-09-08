using System;
using System.Diagnostics;
using VAF13.LogSettings;
using VAF13.Settings;

namespace VAF13.Queries
{
    public class DumpMySqlDatabaseQuery : QueryBase<string>
    {
        public override string Query()
        {
            LogOptions.GetLogger().Info("Starting to run mysqldump of database");
            var settings = BackupSettings.GetInstance();
            var executable = settings.Executable;
            var arguments = $"-u {settings.User} -p{settings.Password} {settings.Database}";
            LogOptions.GetLogger().Info("Starting dump {executable} {arguments}", executable, arguments);
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
                var standardOutput = p?.StandardOutput.ReadToEnd();
                p?.WaitForExit();
                return standardOutput;
            }
            catch (Exception e)
            {
                LogOptions.GetLogger().Fatal(e, "Exception in dump mysql database {message}", e.Message);
                return string.Empty;
            }
        }
    }
}