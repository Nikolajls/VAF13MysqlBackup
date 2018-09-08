using VAF13.Commands;
using VAF13.LogSettings;

namespace VAF13.BackupService
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            LogOptions.GetLogger().Info("Starting windows program");
            new BackupDatabaseIntoFileCommand().Execute();
            LogOptions.GetLogger().Info("Finished executing");
        }
    }
}