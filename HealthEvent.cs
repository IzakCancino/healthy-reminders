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
            }
        }
    }
}
