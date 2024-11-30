using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace healthy_reminders
{
    internal class NotificationManager
    {
        private readonly MainWindow _mainWindow;
        private readonly NotifyIcon _notifyIcon;

        public NotificationManager(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;

            // Initialize NotifyIcon
            _notifyIcon = new NotifyIcon
            {
                Icon = new System.Drawing.Icon("Assets/icon.ico"),
                Visible = true,
                Text = "Healthy Reminders"
            };

            // Settings for system tray app
            _notifyIcon.DoubleClick += (s, e) =>
            {
                ShowApp();
            };
            _notifyIcon.BalloonTipClicked += (s, e) =>
            {
                ShowApp();
            };
            _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("Show", null, (s, e) => ShowApp());
            _notifyIcon.ContextMenuStrip.Items.Add("Exit", null, (s, e) => CloseApp());
        }

        // Print a system notification
        public void ShowNotification(string title, string message, int duration = 5000)
        {
            _notifyIcon.BalloonTipTitle = title;
            _notifyIcon.BalloonTipText = message;
            _notifyIcon.BalloonTipIcon = ToolTipIcon.None;
            _notifyIcon.ShowBalloonTip(duration);
        }

        // Show app's main window and bring it to front
        public void ShowApp()
        {
            _mainWindow.Show();
            _mainWindow.Activate();
            _mainWindow.Focus();
            _mainWindow.Topmost = true;
            _mainWindow.Topmost = false;
            _mainWindow.WindowState = WindowState.Normal;
        }

        // Hide app's main window
        public void HideApp()
        {
            _mainWindow.Hide();
        }

        // Close completely the app
        public void CloseApp()
        {
            _notifyIcon.Dispose();
            System.Windows.Application.Current.Shutdown();
        }

        public void UpdateText(string text)
        {
            _notifyIcon.Text = text;
        }

        
    }
}
