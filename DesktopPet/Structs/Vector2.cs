namespace DesktopPet.Structs
{
    internal struct Vector2
    {
        public double x;
        public double y;

        public Vector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public static Vector2 Lerp(Vector2 a, Vector2 b, double t)
        {
            t = Math.Max(Math.Min(t, 1), 0);
            return new Vector2(
                a.x + (b.x - a.x) * t,
                a.y + (b.y - a.y) * t
            );
        }

        public override string? ToString()
        {
            return $"X:{x},Y:{y}";
        }
    }
}
