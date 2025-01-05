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
        public NotifyIcon NotifyIcon;

        public NotificationManager(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;

            // Initialize NotifyIcon
            NotifyIcon = new NotifyIcon
            {
                Icon = new System.Drawing.Icon("Assets/icon.ico"),
                Visible = true,
                Text = "Healthy Reminders"
            };

            // Settings for system tray app
            NotifyIcon.DoubleClick += (s, e) =>
            {
                ShowApp();
            };
            NotifyIcon.BalloonTipClicked += (s, e) =>
            {
                ShowApp();
            };
            NotifyIcon.ContextMenuStrip = new ContextMenuStrip();
            NotifyIcon.ContextMenuStrip.Items.Add("Show", null, (s, e) => ShowApp());
            NotifyIcon.ContextMenuStrip.Items.Add("Exit", null, (s, e) => CloseApp());
        }

        // Print a system notification
        public void ShowNotification(string title, string message, int duration = 5000)
        {
            NotifyIcon.BalloonTipTitle = title;
            NotifyIcon.BalloonTipText = message;
            NotifyIcon.BalloonTipIcon = ToolTipIcon.None;
            NotifyIcon.ShowBalloonTip(duration);
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
            NotifyIcon.Dispose();
            System.Windows.Application.Current.Shutdown();
        }

        // Update the text showed with the icon in the apps tray
        public void UpdateText(string text)
        {
            NotifyIcon.Text = text;
        }        
    }
}
