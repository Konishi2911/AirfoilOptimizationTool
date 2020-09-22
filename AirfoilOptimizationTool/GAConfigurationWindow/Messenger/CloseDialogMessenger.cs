using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool.GAConfigurationWindow.Messenger {
    class CloseDialogMessenger {
        public event EventHandler CloseDialogEvent;
        public static string eventName => nameof(CloseDialogEvent);

        private CloseDialogMessenger() { }

        public static CloseDialogMessenger instance { get; } = new CloseDialogMessenger();

        public void requestClosingDialog() {
            CloseDialogEvent?.Invoke(this, EventArgs.Empty);
        }

        // Release all event handler
        public void disconnect() {
            CloseDialogEvent = null;
        }
    }
}
