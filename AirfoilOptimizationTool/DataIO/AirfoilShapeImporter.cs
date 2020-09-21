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
        SeparatedValues csv;

        public AirfoilShapeImporter(SeparatedValues values) {
            csv = values!;
        }

        public Airfoil.Airfoil getAirfoil() {
            SeparatedValueToPointConverter converter = new SeparatedValueToPointConverter();
            var tempPoints = converter.convertFrom(csv);

            //
            // Divide into upper and lower part
            //
            List<Point> upperList = new List<Point>();
            List<Point> lowerList = new List<Point>();
            var le = tempPoints.min(PointExtensions.Axis.X);
            var leIndex = tempPoints.find((Point)le);
            for (var i = leIndex; i >= 0; --i) {
                upperList.Add(tempPoints[i]);
            }
            for (var i = leIndex; i < tempPoints.Length; ++i) {
                lowerList.Add(tempPoints[i]);
            }

            //
            // Interpolate upper and lower point to rearrage each points equidistantly.
            //
            var upperInterpolator = new LinearInterpolator(upperList.ToArray());
            var lowerInterpolator = new LinearInterpolator(lowerList.ToArray());

            var upperPoints = upperInterpolator.curve(tempPoints.Length / 2);
            var lowerPoints = lowerInterpolator.curve(tempPoints.Length / 2);
            var points = new List<PairedPoint>();
            for (var i = 0; i < tempPoints.Length / 2; ++i) {
                points.Add(new PairedPoint(upperPoints[i].X, upperPoints[i].Y, lowerPoints[i].Y));
            }


            var airfoil = new Airfoil.Airfoil(points.ToArray());
            airfoil.name = csv.valueName;

            return airfoil;
        }
    }
}
