using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AirfoilOptimizationTool {
    public class TriggerCommand : ICommand {
        Action callback;
        Func<bool> canExecute;

        public event EventHandler CanExecuteChanged;

        public TriggerCommand(Action callback, Func<bool> canExecute) {
            this.callback = callback;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter) {
            return canExecute();
        }

        public void Execute(object parameter) {
            callback();
        }
    }
}
