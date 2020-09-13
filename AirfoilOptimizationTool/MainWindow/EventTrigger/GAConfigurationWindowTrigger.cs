using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool.MainWindow.EventTrigger {
    public class GAConfigurationWindowTrigger : System.Windows.Interactivity.EventTrigger {
        public GAConfigurationWindowTrigger() : base(Messenger.GAConfigurationMessenger.eventName) {
            SourceObject = Messenger.GAConfigurationMessenger.instance;
        }
    }
}
