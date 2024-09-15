using System.Xml.Serialization;

namespace XboxDisplaySwitcher
{
    public class TrayIcon
    {
        private const string CONFIG_FILE = "appConfig.xml";

        private NotifyIcon _trayIcon;
        private SettingsForm _settingsForm;
        private readonly ControllerMonitorManager _controllerMonitorManager;

        public TrayIcon()
        {
            _controllerMonitorManager = new ControllerMonitorManager();
            _trayIcon = new NotifyIcon
            {
                Icon = new Icon("xds-logo.ico"),
                Visible = true,
                ContextMenuStrip = CreateContextMenu(),
            };
            _trayIcon.MouseDoubleClick += new MouseEventHandler(OpenSettings);

            LoadConfig();
            _controllerMonitorManager.StartMonitoring();
        }

        private ContextMenuStrip CreateContextMenu()
        {
            var contextMenu = new ContextMenuStrip();

            var settingsMenuItem = new ToolStripMenuItem("Settings", null, OpenSettings);
            var exitMenuItem = new ToolStripMenuItem("Exit", null, Exit);

            contextMenu.Items.Add(settingsMenuItem);
            contextMenu.Items.Add(exitMenuItem);

            return contextMenu;
        }

        private void OpenSettings(object sender, EventArgs e)
        {
            OpenSettings();
        }

        private void OpenSettings()
        {
            if (_settingsForm == null || _settingsForm.IsDisposed)
            {
                using (_settingsForm = new SettingsForm(_controllerMonitorManager))
                {
                    _settingsForm.ShowDialog();
                }
            }
            else
            {
                _settingsForm.Activate();  // Активируем окно, если оно уже открыто
            }

        }

        private void Exit(object sender, EventArgs e)
        {
            _trayIcon.Visible = false;
            SaveConfig();

            Application.Exit();
        }

        private void SaveConfig()
        {
            AppConfig appConfig = new AppConfig()
            {
                MonitorOnConnect = _controllerMonitorManager.MonitorOnConnect,
                MonitorOnDisconnect = _controllerMonitorManager.MonitorOnDisconnect,
            };
            using (var stream = new StreamWriter(CONFIG_FILE))
            {
                var serializer = new XmlSerializer(typeof(AppConfig));
                serializer.Serialize(stream, appConfig);
            }
        }

        private void LoadConfig()
        {
            _controllerMonitorManager.LoadMonitors();

            if (!File.Exists(CONFIG_FILE))
            {
                OpenSettings();
                return;
            }

            AppConfig appConfig;
            using (var stream = new StreamReader(CONFIG_FILE))
            {
                var serializer = new XmlSerializer(typeof(AppConfig));

                appConfig = (AppConfig)serializer.Deserialize(stream);
            }

            if (_controllerMonitorManager.MonitorExists(appConfig.MonitorOnConnect))
                _controllerMonitorManager.MonitorOnConnect = appConfig.MonitorOnConnect;

            if (_controllerMonitorManager.MonitorExists(appConfig.MonitorOnDisconnect))
                _controllerMonitorManager.MonitorOnDisconnect = appConfig.MonitorOnDisconnect;

            if (_controllerMonitorManager.MonitorOnConnect == string.Empty || _controllerMonitorManager.MonitorOnDisconnect == string.Empty)
                OpenSettings();
        }


    }
}
