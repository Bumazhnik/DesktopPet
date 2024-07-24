namespace DesktopPet
{
    internal static class Program
    {
        private const string DEFAULT_CONFIG_PATH = "characterConfig.json";
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new CharacterForm(args.Length > 0 ? args[0] : DEFAULT_CONFIG_PATH));
        }
    }
}