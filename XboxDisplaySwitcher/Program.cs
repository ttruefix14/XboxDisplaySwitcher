namespace XboxDisplaySwitcher
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var trayIcon = new TrayIcon();
            Application.Run();
        }
    }
}
