namespace DesktopPet.Scheduler
{
    internal class BehaviourTaskScheduler
    {
        private List<BehaviourTask> tasks = new();
        private Mutex mutex = new Mutex();
        public void AddTask(BehaviourTask task)
        {
            mutex.WaitOne();
            tasks.Add(task);
            mutex.ReleaseMutex();
        }
        public void ClearTasks()
        {
            mutex.WaitOne();
            tasks.Clear();
            mutex.ReleaseMutex();
        }
        public void Update(double delta)
        {
            mutex.WaitOne();
            if (tasks.Count <= 0) return;
            var task = tasks[0];
            task.Update(delta);
            if (task.Ended)
                tasks.RemoveAt(0);
            mutex.ReleaseMutex();
        }
        public int Count => tasks.Count;
    }
}
