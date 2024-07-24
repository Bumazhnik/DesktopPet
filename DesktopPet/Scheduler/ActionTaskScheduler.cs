namespace DesktopPet.Scheduler
{
    internal class ActionTaskScheduler
    {
        private List<ActionTask> tasks = new();
        Mutex mutex = new();

        public async Task AddTask(ActionTask task)
        {
            await Task.Run(() =>
            {
                mutex.WaitOne();
                tasks.Add(task);
                mutex.ReleaseMutex();
            });

        }
        public async Task ClearTasks()
        {
            await Task.Run(() =>
            {
                mutex.WaitOne();
                tasks.Clear();
                mutex.ReleaseMutex();
            });
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
        public int Count => tasks.Count;
    }
}
