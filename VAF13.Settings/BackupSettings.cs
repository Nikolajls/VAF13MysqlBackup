using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VAF13.LogSettings;

namespace VAF13.Settings
{
    public class BackupSettings
    {
        /// <summary>
        /// Host of the MySQL server
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Name of the database to backup
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Username of the user to use
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Password for the user
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The path that the initial backup will be written to
        /// </summary>
        public string SaveBackupPath { get; set; }

        /// <summary>
        /// List of other paths to copy the files to, can be a network share or other drives
        /// </summary>
        public List<string> CopyBackupsTo = new List<string>();

        /// <summary>
        /// Only perform the backup if SkyWin is running
        /// </summary>
        public bool OnlyBackupWhenSkyWinIsRunning { get; set; }

        /// <summary>
        /// path to mysqldump executable
        /// </summary>
        public string Executable { get; set; }

        // Singleton
        private static BackupSettings LocalInstance { get; set; }
        public static BackupSettings GetInstance()
        {
            LocalInstance = LocalInstance ?? new BackupSettings();
            return LocalInstance;
        }
        //Constructor
        public BackupSettings()
        {
            LogOptions.GetLogger().Info("Starting to read configuration");

            var temporayConfigRead = ConfigurationManager.AppSettings["Host"];
            if (temporayConfigRead != null) Host = temporayConfigRead;


            temporayConfigRead = ConfigurationManager.AppSettings["Database"];
            if (temporayConfigRead != null) Database = temporayConfigRead;

            temporayConfigRead = ConfigurationManager.AppSettings["User"];
            if (temporayConfigRead != null) User = temporayConfigRead;

            temporayConfigRead = ConfigurationManager.AppSettings["Password"];
            if (temporayConfigRead != null) Password = temporayConfigRead;

            temporayConfigRead = ConfigurationManager.AppSettings["SaveBackupPath"];
            if (temporayConfigRead != null) SaveBackupPath = temporayConfigRead;

            bool tmp;
            temporayConfigRead = ConfigurationManager.AppSettings["OnlyBackupWhenSkyWinIsRunning"];
            if (temporayConfigRead != null && bool.TryParse(temporayConfigRead, out tmp)) OnlyBackupWhenSkyWinIsRunning = tmp;

            temporayConfigRead = ConfigurationManager.AppSettings["CopyBackupsTo"];
            if (temporayConfigRead != null) CopyBackupsTo = temporayConfigRead.Split('|').ToList();

            temporayConfigRead = ConfigurationManager.AppSettings["Executable"];
            if (temporayConfigRead != null) Executable = temporayConfigRead;

            LogOptions.GetLogger().Info("Loaded settings are as follows");
            LogOptions.GetLogger().Info("User:{User}", User);
            LogOptions.GetLogger().Info("Password:{Password}", Password);
            LogOptions.GetLogger().Info("Database:{Database}", Database);
            LogOptions.GetLogger().Info("SaveBackupPath:{SaveBackupPath}", SaveBackupPath);
            LogOptions.GetLogger().Info("OnlyBackupWhenSkyWinIsRunning:{OnlyBackupWhenSkyWinIsRunning}", OnlyBackupWhenSkyWinIsRunning);
            LogOptions.GetLogger().Info("CopyBackupsTo:{CopyBackupsTo}", CopyBackupsTo);
            LogOptions.GetLogger().Info("Executable:{Executable}", Executable);
        }


    }
}
