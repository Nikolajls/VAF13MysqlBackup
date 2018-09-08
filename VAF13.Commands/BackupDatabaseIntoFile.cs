using System;
using System.IO;
using VAF13.LogSettings;
using VAF13.Queries;
using VAF13.Settings;

namespace VAF13.Commands
{
    public class BackupDatabaseIntoFileCommand : CommandBase
    {
        protected override void OnExecuting()
        {
            //Abort backup if only when skywin is running and not currently running
            if (BackupSettings.GetInstance().OnlyBackupWhenSkyWinIsRunning && !new IsSkywinRunningQuery().Query())
            {
                LogOptions.GetLogger().Info("Backup only when skywin is running - it's not right now - aborting");
                return;
            }

            var sqlData = new DumpMySqlDatabaseQuery().Query();
            if (string.IsNullOrWhiteSpace(sqlData))
            {
                LogOptions.GetLogger().Warn("Unable to dump sql database into string {output}", sqlData);
                return;
            }

            var dateTime = DateTime.Now;
            var subPath = Path.Combine(dateTime.Year.ToString(), dateTime.Month.ToString(), dateTime.Day.ToString(),
                DateTime.Now.ToString("s").Replace(":", "-") + ".sql");

            var initialFile = Path.Combine(BackupSettings.GetInstance().SaveBackupPath, subPath);
            LogOptions.GetLogger().Info("Initial backup will be saved into {filepath}", initialFile);

            try
            {
                var fi = new FileInfo(initialFile);
                if (!fi?.Directory?.Exists ?? true)
                {
                    LogOptions.GetLogger().Info("Directory path for file doesnt exists creating it");
                    fi?.Directory?.Create();
                }

                LogOptions.GetLogger().Info("Saving backup file");
                File.WriteAllText(initialFile, sqlData);
                LogOptions.GetLogger().Info("Saved backup file");

                if (BackupSettings.GetInstance().CopyBackupsTo?.Count > 0)
                    new CopyBackupFileIntoExtraLocationsCommand(initialFile, subPath).Execute();
            }
            catch (Exception e)
            {
                LogOptions.GetLogger().Fatal(e, "Exception in save initial file {message}", e.Message);
            }
        }
    }
}