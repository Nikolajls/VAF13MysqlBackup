namespace VAF13.Domain.Settings;

public class MysqlDumpSettings
{
    /// <summary>
    ///     Host of the MySQL server
    /// </summary>
    public string Host { get; set; } = "localhost";

    /// <summary>
    ///     Host of the MySQL server
    /// </summary>
    public string Port { get; set; } = "3306";

    /// <summary>
    ///     Username of the user to use
    /// </summary>
    public string User { get; set; } = string.Empty;

    /// <summary>
    ///     Password for the user
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    ///     Name of the database to backup
    /// </summary>
    public string Database { get; set; } = string.Empty;

    /// <summary>
    ///     ExtraArguments for mysqldump
    /// </summary>
    public string ExtraArguments { get; set; } = string.Empty;

    /// <summary>
    ///     path to mysqldump executable
    /// </summary>
    public string Executable { get; set; } = string.Empty;
}
