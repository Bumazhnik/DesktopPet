using System.Diagnostics;

namespace DesktopPet;
internal class GameTime
{
    private double time;
    private Stopwatch stopwatch = new Stopwatch();
    public GameTime()
    {
        stopwatch.Start();
    }
    public void Tick()
    {
        time = stopwatch.Elapsed.TotalSeconds;
    }
    public double Time
    {
        get => time;
    }
    public double Delta
    {
        get => stopwatch.Elapsed.TotalSeconds - time;
    }
}

