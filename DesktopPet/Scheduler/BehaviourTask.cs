namespace DesktopPet.Scheduler
{
    internal class BehaviourTask
    {
        public Action action;
        public double maxTime;
        public double currentTime;

        public BehaviourTask(Action action, double maxTime)
        {
            this.action = action;
            this.maxTime = maxTime;
        }
        public void Update(double delta)
        {
            if (!Ended)
                action();
            currentTime += delta;
        }
        public bool Ended => currentTime >= maxTime;
    }
}
