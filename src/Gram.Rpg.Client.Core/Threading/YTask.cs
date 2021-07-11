using System;
using System.Threading.Tasks;

namespace Gram.Rpg.Client.Core.Threading
{
    public static class YTask
    {
        public static Task RunInSequence(params Func<Task>[] args)
        {
            async Task RunTasks()
            {
                foreach (var func in args) 
                    await func();
            }

            return RunTasks();
        }
    }
}