using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPet.Scheduler
{
    internal class ActionTaskScheduler
    {
        private List<ActionTask> tasks = new();
        Mutex mutex = new();

        public void AddTask(ActionTask task)
        {
            mutex.WaitOne();
            tasks.Add(task);
            mutex.ReleaseMutex();
        }
        public void Update(double delta)
        {
            mutex.WaitOne();
            foreach (var task in tasks)
            {
                task.AddDelta(delta);
            }
            tasks.RemoveAll(x => !x.CanExecute);
            mutex.ReleaseMutex();
        }
    }
}
