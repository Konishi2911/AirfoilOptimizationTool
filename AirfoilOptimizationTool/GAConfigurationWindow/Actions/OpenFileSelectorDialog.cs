using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AirfoilOptimizationTool.GAConfigurationWindow.Actions {
    class OpenFileSelectorDialog : System.Windows.Interactivity.TriggerAction<DependencyObject> {
        protected override void Invoke(object parameter) {
            OpenFileDialog ofd = new OpenFileDialog();
            if ((bool)ofd.ShowDialog()) {
                (parameter as Messenger.OpenFileSelectorDialogEventArgs).selectedPath = ofd.FileName;
            }
        }
    }
}
