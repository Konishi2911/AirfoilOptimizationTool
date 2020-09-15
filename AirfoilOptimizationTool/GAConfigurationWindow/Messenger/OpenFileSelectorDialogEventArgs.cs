using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool.GAConfigurationWindow.Messenger {
    public class OpenFileSelectorDialogEventArgs : EventArgs {
        public string selectedPath { get; set; }
    }
}
