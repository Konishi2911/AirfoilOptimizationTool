using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool
{
    public class ViewModelBase : INotifyPropertyChanges
    {
        public event PropertyChangedEventHandler propertyChanged;

        public void notifyPropertyDidChange(string propertyName) 
        {
            propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
