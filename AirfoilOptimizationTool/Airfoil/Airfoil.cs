using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AirfoilOptimizationTool.Airfoil
{
    public class Airfoil
    {
        private Point[] _airfoilCurve;
        private Point[] _camberPoints;
        private Point[] _thicknessDistribution;
       
        private AirfoilCharacteristics _airfoilCharacteristics;

        public Point[] airfoilCurve { get => _airfoilCurve; }
        public Point[] camberPoints { get => _camberPoints; }
        public Point[] thicknessDistribution { get => _thicknessDistribution; }

    }
}
