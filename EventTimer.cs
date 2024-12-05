using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace healthy_reminders
{
    public class EventTimer
    {
        public HealthEvent HealthEvent;
        public bool OnEventMode = false;
        public bool OnWaiting = true;

        // Window and elements variables
        private readonly MainWindow _mainWindow;
        public Dictionary<string, System.Windows.Controls.Label> ControlLabels;
        public Dictionary<string, System.Windows.Controls.Button> ControlButtons;

        // Timer and time variables
        public DispatcherTimer DispatcherTimer;
        public TimeSpan DelayTime;
        public TimeSpan EventTime;
        public TimeSpan CountdownTime;

        public EventTimer(
            MainWindow window, 
            TimeSpan delayTime, 
            TimeSpan eventTime, 
            HealthEvent healthEvent,
            Dictionary<string, System.Windows.Controls.Label> controlLabels,
            Dictionary<string, System.Windows.Controls.Button> controlButtons
        )
        {
            HealthEvent = healthEvent;

            _mainWindow = window;
            ControlLabels = controlLabels;
            ControlButtons = controlButtons;

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
            UpdateCountdown();

            // Once the countdown reaches 00:00
            if (CountdownTime.Ticks <= 0)
            {
                Stop();

                if (OnEventMode)
                {
                    OnEventMode = false;
                    UpdateControls();

                    _mainWindow.NotificationManager.ShowNotification(
                        HealthEvent.Name,
                        "Event ended!"
                    );

                    ControlLabels["Countdown"].FontWeight = FontWeights.Normal;
                    ControlLabels["Countdown"].Foreground = System.Windows.Media.Brushes.Black;
                }
                else
                {
                    OnEventMode = true;
                    UpdateControls();

                    Random rand = new Random();
                    _mainWindow.NotificationManager.ShowNotification(
                        HealthEvent.Name,
                        HealthEvent.Alerts[rand.Next(HealthEvent.Alerts.Length)]
                    );

                    ControlLabels["Countdown"].FontWeight = FontWeights.Bold;
                    ControlLabels["Countdown"].Foreground = System.Windows.Media.Brushes.DarkGreen;
                }

                Reset();
            }
        }

        // Reset timer
        public void Reset()
        {
            CountdownTime = OnEventMode ? EventTime : DelayTime;
            UpdateCountdown();
        }

        // Start timer
        public void Start()
        {
            OnWaiting = false;
            UpdateControls();

            Reset();
            DispatcherTimer.Start();

            ControlLabels["Countdown"].FontWeight = FontWeights.Normal;
            ControlLabels["Countdown"].Foreground = System.Windows.Media.Brushes.Gray;
        }

        // Update countdown timer
        public void UpdateCountdown()
        {
            ControlLabels["Countdown"].Content = CountdownTime.ToString(@"mm\:ss");
        }

        // Stop timer
        public void Stop()
        {
            OnWaiting = true;
            UpdateControls();

            Reset();
            DispatcherTimer.Stop();

            ControlLabels["Countdown"].FontWeight = FontWeights.Normal;
            ControlLabels["Countdown"].Foreground = System.Windows.Media.Brushes.Black;
        }

        // Skip timer of event itself
        public void Skip()
        {
            OnWaiting = true;
            OnEventMode = false;
            UpdateControls();

            Stop();
        }

        // Update control buttons based on countdown states
        public void UpdateControls()
        {
            ControlButtons["Play"].IsEnabled = (OnWaiting);
            ControlButtons["Stop"].IsEnabled = (!OnWaiting && !OnEventMode);
            ControlButtons["Skip"].IsEnabled = (OnEventMode);

            string message = "(";
            message += OnWaiting ? "Waiting to start " : "Running ";
            message += OnEventMode ? "event time" : "event delay";
            message += ")";
            ControlLabels["Status"].Content = message;
        }
    }
}
