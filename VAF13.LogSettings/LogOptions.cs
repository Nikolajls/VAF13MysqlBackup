using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace VAF13.LogSettings
{
    public static class LogOptions
    {
        private static Logger _logger;
        public static Logger GetLogger()
        {
            return _logger ?? (_logger = NLog.LogManager.GetCurrentClassLogger());
        }
    }
}
