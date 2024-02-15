namespace OsuThumbMaker
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Validate if there's any previous settings defined in config.json
            // if not create a default one
            ConfigManager.Config.LoadConfig();
        }
    }
}