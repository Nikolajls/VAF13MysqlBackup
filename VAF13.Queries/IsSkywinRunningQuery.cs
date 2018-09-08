using System.Diagnostics;

namespace VAF13.Queries
{
    public class IsSkywinRunningQuery : QueryBase<bool>
    {
        public override bool Query()
        {
            return Process.GetProcessesByName("SkyWin").Length > 0;
        }
    }
}