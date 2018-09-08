using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VAF13.LogSettings;
using VAF13.Settings;

namespace VAF13.Commands
{
    public class CopyBackupFileIntoExtraLocationsCommand : CommandBase
    {
        private readonly string _fileLocation;
        private readonly string _subPath;
        public CopyBackupFileIntoExtraLocationsCommand(string fileLocation, string subPath)
        {
            _fileLocation = fileLocation;
            _subPath = subPath;
        }

        protected override void OnExecuting()
        {
            foreach (var extraPath in BackupSettings.GetInstance().CopyBackupsTo)
            {
                var destinationPath = Path.Combine(extraPath, _subPath);
                var fi = new FileInfo(destinationPath);
                if (!fi?.Directory?.Exists ?? true)
                {
                    LogOptions.GetLogger().Info("Directory {path} for copying destination not existing", destinationPath);
                    fi?.Directory?.Create();
                }
                LogOptions.GetLogger().Info("Copying {src} to {dst}",_fileLocation,destinationPath);
                File.Copy(_fileLocation,destinationPath);
                LogOptions.GetLogger().Info("Copied successful");

            }
        }
    }
}
