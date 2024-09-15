namespace XboxDisplaySwitcher
{
    public class SettingsForm : Form
    {
        private ComboBox _txtMonitorOnConnect;
        private ComboBox _txtMonitorOnDisconnect;
        private Button _btnOk;
        private Button _btnCancel;
        private Button _btnReload;
        private readonly ControllerMonitorManager _controllerMonitorManager;

        public SettingsForm(ControllerMonitorManager controllerMonitorManager)
        {
            Icon = new Icon("xds-logo.ico");
            Size = new Size(235, 240);
            _controllerMonitorManager = controllerMonitorManager;
            InitializeComponents();
            LoadSettings();
        }

        private void InitializeComponents()
        {
            Label onConnectLabel = new Label() { Left = 10, Top = 10, Width = 200, Text = "Monitor on connect: " };
            _txtMonitorOnConnect = new ComboBox { Left = 10, Top = 40, Width = 200 };


            Label onDisconnectLabel = new Label() { Left = 10, Top = 70, Width = 200, Text = "Monitor on disconnect: " };
            _txtMonitorOnDisconnect = new ComboBox { Left = 10, Top = 100, Width = 200 };

            AddMonitorLists();

            _btnOk = new Button { Text = "OK", Left = 10, Top = 140, DialogResult = DialogResult.OK };
            _btnCancel = new Button { Text = "Cancel", Left = 120, Top = 140, DialogResult = DialogResult.Cancel };

            _btnOk.Click += (sender, e) => { SaveSettings(); Close(); };
            _btnCancel.Click += (sender, e) => { Close(); };

            _btnReload = new Button { Text = "Reload monitors", Left = 10, Top = 170, Width = 200, DialogResult = DialogResult.None };
            _btnReload.Click += (sender, e) => { _controllerMonitorManager.LoadMonitors(); AddMonitorLists(); };

            Controls.Add(onConnectLabel);
            Controls.Add(_txtMonitorOnConnect);
            Controls.Add(onDisconnectLabel);
            Controls.Add(_txtMonitorOnDisconnect);
            Controls.Add(_btnOk);
            Controls.Add(_btnCancel);
            Controls.Add(_btnReload);

            Text = "Settings";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            AcceptButton = _btnOk;
            CancelButton = _btnCancel;
        }

        private void AddMonitorLists()
        {
            _txtMonitorOnConnect.Items.Clear();
            _txtMonitorOnDisconnect.Items.Clear();
            _txtMonitorOnConnect.Items.AddRange(_controllerMonitorManager.Monitors.Items.Select(x => (object)x.ShortMonitorId).ToArray());
            _txtMonitorOnDisconnect.Items.AddRange(_controllerMonitorManager.Monitors.Items.Select(x => (object)x.ShortMonitorId).ToArray());
        }

        private void LoadSettings()
        {
            _txtMonitorOnConnect.Text = _controllerMonitorManager.MonitorOnConnect;
            _txtMonitorOnDisconnect.Text = _controllerMonitorManager.MonitorOnDisconnect;
        }

        private void InitializeComponent()
        {

        }

        private void SaveSettings()
        {
            _controllerMonitorManager.MonitorOnConnect = _txtMonitorOnConnect.Text;
            _controllerMonitorManager.MonitorOnDisconnect = _txtMonitorOnDisconnect.Text;
        }
    }
}
