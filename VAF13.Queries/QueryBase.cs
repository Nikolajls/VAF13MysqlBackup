using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VAF13.LogSettings;

namespace VAF13.Queries
{
    public abstract class QueryBase<T>
    {
        protected QueryBase()
        {
            LogOptions.GetLogger().Info("Running Query of {type}",this.GetType());
        }

        public abstract T Query();
    }
}
