namespace VAF13.Domain.Settings;

public class BackupSettings
{
    /// <summary>
    ///     List of other paths to copy the files to, can be a network share or other drives
    /// </summary>
    public List<string> CopyBackupsTo { get; set; } = new List<string>();

    public int MinuteInterval { get; set; } = 5;

    /// <summary>
    ///     The path that the initial backup will be written to
    /// </summary>
    public string SaveBackupPath { get; set; } = string.Empty;

    /// <summary>
    ///     Only perform the backup if SkyWin is running
    /// </summary>
    public bool OnlyBackupWhenSkyWinIsRunning { get; set; }
}
