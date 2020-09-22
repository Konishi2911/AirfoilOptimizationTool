using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AirfoilOptimizationTool.GAConfigurationWindow.Actions {
    class CloseDialogAction : System.Windows.Interactivity.TriggerAction<DependencyObject> {
        protected override void Invoke(object parameter) {
            Window.GetWindow(AssociatedObject).Close();
        }
    }
}
