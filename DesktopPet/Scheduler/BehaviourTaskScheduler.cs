using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPet.Scheduler
{
    internal class BehaviourTaskScheduler
    {
        private List<BehaviourTask> tasks = new();
        public void AddTask(BehaviourTask task)
        {
            tasks.Add(task);
        }
        public void Update(double delta)
        {
            if (tasks.Count <= 0) return;
            var task = tasks[0];
            task.Update(delta);
            if(task.Ended)
                tasks.RemoveAt(0);
        }
    }
}
