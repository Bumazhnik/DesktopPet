using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPet.Scheduler
{
    internal class ActionTask
    {
        public Action action;
        public double delay;
        public int count;
        public int timesExecuted;
        public double time;

        public ActionTask(Action action, double delay, int count = -1, bool executeImmediately = false)
        {
            this.action = action;
            this.delay = delay;
            this.count = count;
            if (executeImmediately)
                Execute();
        }

        public void AddDelta(double delta)
        {
            time += delta;
            while(time > delay && CanExecute)
            {
                time -= delay;
                Execute();
            }
        }
        private void Execute()
        {
            timesExecuted++;
            action();
        }
        public bool CanExecute => count == -1 || timesExecuted < count;
    }
}
