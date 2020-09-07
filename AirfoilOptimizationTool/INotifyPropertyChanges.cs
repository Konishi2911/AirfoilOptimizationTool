using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool
{
    interface INotifyPropertyChanges
    {
        event PropertyChangedEventHandler propertyChanged;

        void notifyPropertyDidChange(string propertyName);
    }
}
