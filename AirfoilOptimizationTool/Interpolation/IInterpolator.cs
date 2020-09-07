using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AirfoilOptimizationTool.Interpolation
{
    interface IInterpolator
    {
        Point interpolation(double t);
        Point[] curve(int N);
    }
}
