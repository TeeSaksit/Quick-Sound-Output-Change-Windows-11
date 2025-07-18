using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Quick_Sound_Output_Change_Windows_11.Helpers;
using Quick_Sound_Output_Change_Windows_11.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;
using Windows.UI.Notifications;
using WinUIEx;
using static Quick_Sound_Output_Change_Windows_11.Services.AudioManager;


namespace Quick_Sound_Output_Change_Windows_11
{
    public sealed partial class MainWindow : WinUIEx.WindowEx
    {
        private SolidColorBrush _hoverBrush = ColorConverter.SolidColorBrushFromHex("#505050");
        private SolidColorBrush _normalBrush = new SolidColorBrush(Colors.Transparent);

        public ObservableCollection<AudioDeviceInfo> Devices { get; } = new();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;

        public MainWindow()
        {
            InitializeComponent();

            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            int exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
            exStyle |= WS_EX_TOOLWINDOW;
            SetWindowLong(hWnd, GWL_EXSTYLE, exStyle);

            IsTitleBarVisible = false;
            IsResizable = false;
            IsAlwaysOnTop = true;
            Activated += MainWindow_Activated;

            MyGrid.Resources["ActiveColorConverter"] = new ActiveColorConverter();

            MyItemsControl.ItemsSource = Devices;

            LoadDevices();
        }

        private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            if (args.WindowActivationState == WindowActivationState.Deactivated)
            {
                this.Hide();
                return;
            }

            LoadDevices();

            DisplayArea displayArea = DisplayArea.GetFromWindowId(AppWindow.Id, DisplayAreaFallback.Primary);

            if (displayArea != null)
            {
                int workAreaWidth = displayArea.WorkArea.Width;
                int workAreaHeight = displayArea.WorkArea.Height;

                int workAreaX = displayArea.WorkArea.X;
                int workAreaY = displayArea.WorkArea.Y;

                double percentageWidth = 0.196;
                double percentageHeight = 0.333;

                int newWindowWidth = (int)Math.Round(workAreaWidth * percentageWidth);
                int newWindowHeight = (int)Math.Round(workAreaHeight * percentageHeight);

                int marginX = 4;
                int marginY = 4;
                int targetX = workAreaX + workAreaWidth - newWindowWidth - marginX;
                int targetY = workAreaY + workAreaHeight - newWindowHeight - marginY;

                if (targetX < workAreaX) targetX = workAreaX;
                if (targetY < workAreaY) targetY = workAreaY;

                AppWindow.MoveAndResize(new RectInt32(targetX, targetY, newWindowWidth, newWindowHeight));
            }
        }

        private void Border_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (sender is Border border)
            {
                
                    border.Background = _hoverBrush;
            }
        }

        private void Border_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (sender is Border border && border.DataContext is AudioManager.AudioDeviceInfo device)
            {
                if (device.IsDefault)
                    border.Background = ActiveColorConverter.ActiveBrush;
                else
                    border.Background = _normalBrush;
            }
        }

        private void Border_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (sender is Border border && border.DataContext is AudioDeviceInfo device)
            {
                bool result = AudioManager.SetDefaultAudioDevice(device.Id);
                Debug.WriteLine(result ? $"Set default to: {device.Name}" : "Failed to set default device");

                foreach (var d in Devices) d.IsDefault = d.Id == device.Id;
                LoadDevices();
            }
        }

        private void LoadDevices()
        {
            var devices = AudioManager.GetOutputAudioDevices();

            Devices.Clear();
            foreach (var d in devices) Devices.Add(d);
        }
    }
}
