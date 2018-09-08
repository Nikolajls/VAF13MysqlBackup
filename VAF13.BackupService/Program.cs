using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VAF13.Commands;
using VAF13.LogSettings;
using VAF13.Queries;

namespace VAF13.BackupService
{
    class Program
    {
        static void Main(string[] args)
        {
            LogOptions.GetLogger().Info("Starting windows program");

            new BackupDatabaseIntoFileCommand().Execute();;


            Console.WriteLine("IDLE");
            Console.ReadLine();
        }
    }
}
