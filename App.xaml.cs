using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUIEx;


namespace Quick_Sound_Output_Change_Windows_11
{
    public partial class App : Application
    {
        private System.Windows.Forms.NotifyIcon? _trayIcon;
        private WindowEx? _window;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);


        public App()
        {
            const string mutexName = "QuickSoundOutputChangeAppMutex";
            bool createdNew;
            _ = new System.Threading.Mutex(true, mutexName, out createdNew);

            if (!createdNew)
            {
                MessageBox(IntPtr.Zero, "The application is already running.", "Quick Sound Output Change", 0);
                Environment.Exit(0);
                return;
            }

            InitializeComponent();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            SetupTrayIcon();

        }

        private void SetupTrayIcon()
        {
            _trayIcon = new System.Windows.Forms.NotifyIcon();

            string iconPath = System.IO.Path.Combine(AppContext.BaseDirectory, "Assets", "AudioDeviceIcon.ico");
            _trayIcon.Icon = new System.Drawing.Icon(iconPath);
            _trayIcon.Visible = true;
            _trayIcon.Text = "Quick Sound Output Change";

            var contextMenu = new System.Windows.Forms.ContextMenuStrip();

            var startupItem = new System.Windows.Forms.ToolStripMenuItem("Enable Startup");
            startupItem.CheckOnClick = true;
            startupItem.Checked = IsStartupEnabled();
            startupItem.Click += (s, e) =>
            {
                SetStartup(startupItem.Checked);
            };
            contextMenu.Items.Add(startupItem);

            var exitItem = new System.Windows.Forms.ToolStripMenuItem("Exit");
            exitItem.Click += (s, e) =>
            {
                _trayIcon.Visible = false;
                _trayIcon.Dispose();
                Environment.Exit(0);
            };
            contextMenu.Items.Add(exitItem);

            _trayIcon.ContextMenuStrip = contextMenu;

            _trayIcon.MouseClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    if (_window == null)
                    {
                        _window = new MainWindow();
                        _window.Activate();
                    }
                    else if (_window.Visible)
                    {
                        _window.Hide();
                    }
                    else
                    {
                        _window.Show();
                        _window.Activate();
                        _window.SetForegroundWindow();
                    }
                }
            };
        }

        public static void SetStartup(bool enable)
        {
            string runKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
            string appName = "QuickSoundOutputChange";
            string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            using RegistryKey? key = Registry.CurrentUser.OpenSubKey(runKey, true);

            if (enable)
            {
                key?.SetValue(appName, $"\"{appPath}\"");
            }
            else
            {
                key?.DeleteValue(appName, false);
            }
        }

        private static bool IsStartupEnabled()
        {
            string runKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
            string appName = "QuickSoundOutputChange";

            using RegistryKey? key = Registry.CurrentUser.OpenSubKey(runKey, false);
            return key?.GetValue(appName) != null;
        }
    }
}
