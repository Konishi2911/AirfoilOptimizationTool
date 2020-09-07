using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AirfoilOptimizationTool.Airfoil
{
    public class AirfoilCharacteristics
    {
        private Point[] _liftVsAngle;
        private Point[] _dragVsAngle;

        public Point[] liftVsAngle { get => _liftVsAngle; }
        public Point[] dragVsAngle { get => _dragVsAngle; }

    }
}
