using VAF13.LogSettings;

namespace VAF13.Queries
{
    public abstract class QueryBase<T>
    {
        protected QueryBase()
        {
            LogOptions.GetLogger().Info("Running Query of {type}", GetType());
        }

        public abstract T Query();
    }
}