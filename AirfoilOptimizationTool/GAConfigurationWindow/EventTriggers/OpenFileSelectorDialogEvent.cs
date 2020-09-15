using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool.GAConfigurationWindow.EventTriggers {
    class OpenFileSelectorDialogEvent : System.Windows.Interactivity.EventTrigger {
        public OpenFileSelectorDialogEvent() : base(Messenger.OpenFileSelectorDialogMessenger.eventName) {
            SourceObject = Messenger.OpenFileSelectorDialogMessenger.instance;
        }
    }
}
