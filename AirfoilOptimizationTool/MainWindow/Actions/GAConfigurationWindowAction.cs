using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AirfoilOptimizationTool.MainWindow.Actions {
    public class GAConfigurationWindowAction : System.Windows.Interactivity.TriggerAction<DependencyObject> {
        protected override void Invoke(object parameter) {
            new GAConfigurationWindow.GAConfigurationWindow().ShowDialog();
        }
    }
}
