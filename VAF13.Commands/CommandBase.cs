﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        protected CommandBase()
        {

        }

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
        protected CommandBase()
        {

        }

        protected abstract T OnExecuting();

        private T Result { get; set; }

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
