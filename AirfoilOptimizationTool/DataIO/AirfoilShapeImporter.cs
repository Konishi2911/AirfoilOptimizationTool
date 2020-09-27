using AirfoilOptimizationTool.Airfoil;
using AirfoilOptimizationTool.General.Converters;
using AirfoilOptimizationTool.Interpolation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AirfoilOptimizationTool.DataIO {
    class AirfoilShapeImporter {
        SeparatedValues? csv;
        Point[] sourcePoints;

        public AirfoilShapeImporter(SeparatedValues values) {
            csv = values!;

            // Calculate Source Points
            SeparatedValueToPointConverter converter = new SeparatedValueToPointConverter();
            sourcePoints = converter.convertFrom(csv);
        }

        public AirfoilShapeImporter(Point[] points) {
            csv = null;
            sourcePoints = points;
        }

        public Airfoil.Airfoil getAirfoil() {
            return getAirfoil(sourcePoints.Length / 2);
        }
        public Airfoil.Airfoil getAirfoil(int N) {
            //
            // Divide into upper and lower part
            //
            List<Point> upperList = new List<Point>();
            List<Point> lowerList = new List<Point>();
            var le = sourcePoints.min(PointExtensions.Axis.X);
            var leIndex = sourcePoints.find((Point)le);
            for (var i = leIndex; i >= 0; --i) {
                upperList.Add(sourcePoints[i]);
            }
            for (var i = leIndex; i < sourcePoints.Length; ++i) {
                lowerList.Add(sourcePoints[i]);
            }

            //
            // Interpolate upper and lower point to rearrage each points equidistantly.
            //
            var upperInterpolator = new LinearInterpolator(upperList.ToArray());
            var lowerInterpolator = new LinearInterpolator(lowerList.ToArray());

            var upperPoints = upperInterpolator.curveByConstantX(N);
            var lowerPoints = lowerInterpolator.curveByConstantX(N);
            var points = new List<PairedPoint>();
            for (var i = 0; i < N; ++i) {
                points.Add(new PairedPoint(upperPoints[i].X, upperPoints[i].Y, lowerPoints[i].Y));
            }


            var airfoil = new Airfoil.Airfoil(points.ToArray());
            airfoil.name = csv?.valueName;

            return airfoil;
        }
    }
}
