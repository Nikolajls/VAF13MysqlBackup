using NLog;

namespace VAF13.LogSettings
{
    public static class LogOptions
    {
        private static Logger _logger;

        public static Logger GetLogger()
        {
            return _logger ?? (_logger = LogManager.GetCurrentClassLogger());
        }
    }
}