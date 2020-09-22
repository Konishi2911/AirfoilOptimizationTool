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

        public event EventHandler canExecuteChanged;

        public TriggerCommand(Action callback, Func<bool> canExecute) {
            this.callback = callback;
            this.canExecute = canExecute;
        }


        public event EventHandler CanExecuteChanged {
            add => canExecuteChanged += value;
            remove => canExecuteChanged -= value;
        }
        public bool CanExecute(object parameter) {
            return canExecute();
        }
        public void Execute(object parameter) {
            callback();
        }


        public void notifyCanExecuteDidChange() {
            canExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
