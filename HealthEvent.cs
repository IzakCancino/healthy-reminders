using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthy_reminders
{
    public class HealthEvent
    {
        public string Name;
        public string[] Alerts = [];
        public bool HasTimerEvent = true;

        public HealthEvent(string name) {
            Name = name;

            switch (Name) {
                case "Eye care":
                    Alerts = [
                        "Rest your eyes now.",
                        "Take a break for your eyes.",
                        "Protect your vision. Pause and reset.",
                        "Eyes need a quick rest.",
                        "Care for your eyes!"
                    ];
                    break;

                case "Posture health":
                    HasTimerEvent = false;
                    Alerts = [
                        "Check your posture.",
                        "Straighten up for a moment.",
                        "Posture matters—adjust now.",
                        "Sit tall and steady.",
                        "Don't forget your posture!"
                    ];
                    break;

                case "Physical activity":
                    Alerts = [
                        "Time to move!",
                        "Take a quick walk.",
                        "Stretch your legs now.",
                        "Get up and move around.",
                        "Stay active—stand up!"
                    ];
                    break;

                default:
                    Alerts = ["Default alert."];
                    break;
            }
        }
    }
}
