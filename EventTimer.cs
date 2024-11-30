using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace healthy_reminders
{
    public class EventTimer
    {
        public HealthEvent HealthEvent;

        // Window and elements variables
        private readonly MainWindow _mainWindow;
        public System.Windows.Controls.Label LabelCountdown;

        // Timer and time variables
        public DispatcherTimer DispatcherTimer;
        public TimeSpan LapTime;
        public TimeSpan CountdownTime;

        public EventTimer(MainWindow window, TimeSpan lapTime, HealthEvent healthEvent, System.Windows.Controls.Label labelCountdown)
        {
            HealthEvent = healthEvent;

            _mainWindow = window;
            LabelCountdown = labelCountdown;

            // Create and adjust the timer
            DispatcherTimer = new DispatcherTimer();
            DispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            DispatcherTimer.Interval = TimeSpan.FromSeconds(1);

            // Time variables
            LapTime = lapTime;
            CountdownTime = lapTime;
        }

        // In every tick of the timer
        public void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            // Reduce one second in the timer and show it
            CountdownTime = CountdownTime.Subtract(TimeSpan.FromSeconds(1));
            LabelCountdown.Content = CountdownTime.ToString(@"mm\:ss");

            // Once the countdown reaches 00:00
            if (CountdownTime.Ticks <= 0)
            {
                Stop();
                Random rand = new Random();
                _mainWindow.NotificationManager.ShowNotification(
                    HealthEvent.Name, 
                    HealthEvent.Alerts[rand.Next(HealthEvent.Alerts.Length)]
                );
            }
        }

        // Reset timer
        public void Reset()
        {
            CountdownTime = LapTime;
            LabelCountdown.Content = CountdownTime.ToString(@"mm\:ss");
        }

        // Start timer
        public void Start()
        {
            Reset();
            DispatcherTimer.Start();
        }

        // Stop timer
        public void Stop()
        {
            Reset();
            DispatcherTimer.Stop();
        }
    }
}
