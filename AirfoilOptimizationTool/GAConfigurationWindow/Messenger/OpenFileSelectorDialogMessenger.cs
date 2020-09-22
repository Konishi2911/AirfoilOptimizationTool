using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool.GAConfigurationWindow.Messenger {
    class OpenFileSelectorDialogMessenger {
        //
        // Event Trigger
        public event EventHandler OpenFileSelectorDialogEvent;

        //
        // Event Names
        public static string eventName => nameof(OpenFileSelectorDialogEvent);

        public static OpenFileSelectorDialogMessenger instance { get; } = new OpenFileSelectorDialogMessenger();

        // singleton Pattern
        private OpenFileSelectorDialogMessenger() { }

        // Fire a trigger that the event in this class is read.
        public string showDialog() {
            OpenFileSelectorDialogEventArgs eventArgs = new OpenFileSelectorDialogEventArgs();

            instance.OpenFileSelectorDialogEvent?.Invoke(this, eventArgs);

            return eventArgs.selectedPath;
        }

        // Release all event handler
        public void disconnect() {
            OpenFileSelectorDialogEvent = null;
        }
    }
}
