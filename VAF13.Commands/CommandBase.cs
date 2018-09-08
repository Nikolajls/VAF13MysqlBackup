namespace VAF13.Commands
{
    public abstract class AbstractCommand
    {
        protected void Run()
        {
            PerformCommand();
        }

        protected abstract void PerformCommand();
    }


    public abstract class CommandBase : AbstractCommand
    {
        protected abstract void OnExecuting();


        protected override void PerformCommand()
        {
            OnExecuting();
        }

        public void Execute()
        {
            Run();
        }
    }

    public abstract class CommandBase<T> : AbstractCommand
    {
        private T Result { get; set; }

        protected abstract T OnExecuting();

        protected override void PerformCommand()
        {
            Result = OnExecuting();
        }

        public T Execute()
        {
            Run();
            return Result;
        }
    }
}