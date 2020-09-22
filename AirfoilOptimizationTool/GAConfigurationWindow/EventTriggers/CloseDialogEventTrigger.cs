using AirfoilOptimizationTool.GAConfigurationWindow.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool.GAConfigurationWindow.EventTriggers {
    class CloseDialogEventTrigger : System.Windows.Interactivity.EventTrigger {
        public CloseDialogEventTrigger() : base(CloseDialogMessenger.eventName) {
            SourceObject = Messenger.CloseDialogMessenger.instance;
        }
    }
}
