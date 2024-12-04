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
        public bool OnEventMode = false;

        // Window and elements variables
        private readonly MainWindow _mainWindow;
        public System.Windows.Controls.Label LabelCountdown;

        // Timer and time variables
        public DispatcherTimer DispatcherTimer;
        public TimeSpan DelayTime;
        public TimeSpan EventTime;
        public TimeSpan CountdownTime;

        public EventTimer(MainWindow window, TimeSpan delayTime, TimeSpan eventTime, HealthEvent healthEvent, System.Windows.Controls.Label labelCountdown)
        {
            HealthEvent = healthEvent;

            _mainWindow = window;
            LabelCountdown = labelCountdown;

            // Create and adjust the timer
            DispatcherTimer = new DispatcherTimer();
            DispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            DispatcherTimer.Interval = TimeSpan.FromSeconds(1);

            // Time variables
            DelayTime = delayTime;
            CountdownTime = delayTime;
            EventTime = eventTime;
        }

        // In every tick of the timer
        public void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            // Reduce one second in the timer and show it
            CountdownTime = CountdownTime.Subtract(TimeSpan.FromSeconds(1));
            Update();

            // Once the countdown reaches 00:00
            if (CountdownTime.Ticks <= 0)
            {
                Stop();
                
                if (OnEventMode) {
                    OnEventMode = false;

                    _mainWindow.NotificationManager.ShowNotification(
                        HealthEvent.Name,
                        "Event ended!"
                    );

                    Reset();
                } 
                else
                {
                    OnEventMode = true;

                    Random rand = new Random();
                    _mainWindow.NotificationManager.ShowNotification(
                        HealthEvent.Name,
                        HealthEvent.Alerts[rand.Next(HealthEvent.Alerts.Length)]
                    );

                    CountdownTime = EventTime;
                    Update();
                    DispatcherTimer.Start();
                }                
            }
        }

        // Reset timer
        public void Reset()
        {
            CountdownTime = DelayTime;
            Update();
        }

        // Start timer
        public void Start()
        {
            Reset();
            DispatcherTimer.Start();
        }

        // Update timer
        public void Update()
        {
            LabelCountdown.Content = CountdownTime.ToString(@"mm\:ss");
        }

        // Stop timer
        public void Stop()
        {
            Reset();
            DispatcherTimer.Stop();
        }
    }
}
