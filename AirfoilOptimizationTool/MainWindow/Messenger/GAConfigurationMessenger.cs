using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool.MainWindow.Messenger {
    public class GAConfigurationMessenger {
        //
        // Event Trigger
        public event EventHandler GAConfigurationWindowShouldOpenEvent;
        //
        // Event Names
        public static string eventName => nameof(GAConfigurationWindowShouldOpenEvent);

        public static GAConfigurationMessenger instance { get; } = new GAConfigurationMessenger();

        // singleton Pattern
        private GAConfigurationMessenger() { }

        // Fire a trigger that the event in this class is read.
        public void showDialog() {
            instance.GAConfigurationWindowShouldOpenEvent?.Invoke(this, new EventArgs());
        }

    }
}
