using SharpDX.XInput;
using System.Diagnostics;
using System.Xml.Serialization;

namespace XboxDisplaySwitcher
{
    public class ControllerMonitorManager
    {
        private const string MONITORS_LIST_FILE = "monitorsList.xml";

        private readonly Controller _controller;
        private bool _wasConnected;
        private string _monitorOnConnect;
        private string _monitorOnDisconnect;
        private MonitorsList _monitors;

        public ControllerMonitorManager()
        {
            _controller = new Controller(UserIndex.One);
        }

        public string MonitorOnConnect
        {
            get => _monitorOnConnect;
            set => _monitorOnConnect = value;
        }

        public string MonitorOnDisconnect
        {
            get => _monitorOnDisconnect;
            set => _monitorOnDisconnect = value;
        }
        public MonitorsList Monitors { get => _monitors; set => _monitors = value; }

        public void StartMonitoring()
        {
            new Thread(CheckControllerState) { IsBackground = true }.Start();
        }

        private void CheckControllerState()
        {
            while (true)
            {

                if (_controller.IsConnected && !_wasConnected)
                {
                    SwitchMonitor(_monitorOnConnect);
                    _wasConnected = true;
                }
                else if (!_controller.IsConnected && _wasConnected)
                {
                    SwitchMonitor(_monitorOnDisconnect);
                    _wasConnected = false;
                }

                Thread.Sleep(1000); // Check every second
            }
        }

        private void SwitchMonitor(string monitor)
        {
            Process.Start(@"MultiMonitorTool.exe", $"/SetPrimary {monitor.ToLower()}").WaitForExit();
        }

        public void LoadMonitors()
        {
            Process.Start("MultiMonitorTool", $"/sxml {MONITORS_LIST_FILE}").WaitForExit();
            using (var stream = new StreamReader(MONITORS_LIST_FILE))
            {
                var serializer = new XmlSerializer(typeof(MonitorsList));

                Monitors = (MonitorsList)serializer.Deserialize(stream);
                MonitorOnDisconnect = Monitors.Items.FirstOrDefault(x => x.IsPrimary).ShortMonitorId;
            }
        }

        public bool MonitorExists(string monitor)
        {
            return Monitors.Items.Select(x => x.ShortMonitorId).Contains(monitor);
        }
    }
}
