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
        private PairedPoint[] _airfoilCurve;
        private Point[] _camberLine;
        private Point[] _thicknessDistribution;

        private double _chord;
        private double _maxCamber;
        private double _maxCamberLoc;
        private double _maxThickness;
        private double _maxThicknessLoc;
       
        private AirfoilCharacteristics _airfoilCharacteristics;

        // Properties
        public double chord => _chord;
        public double maxCamber => _maxCamber;
        public double maxCamberLocation => _maxCamberLoc;
        public double maxThickness => _maxThickness;
        public double maxThicknessLocation => _maxThicknessLoc;
        public PairedPoint[] airfoilCurve { get => _airfoilCurve; }
        public Point[] camberLine { get => _camberLine; }
        public Point[] thicknessDistribution { get => _thicknessDistribution; }

        //
        // Constructors
        //
        public Airfoil(PairedPoint[] curve) {
            _airfoilCurve = curve;

            calculateChord();
            calculateCamber();
            calculateThickness();
        }
        public Airfoil(PairedPoint[] curve, AirfoilCharacteristics characteristics) {
            _airfoilCurve = curve;
            _airfoilCharacteristics = characteristics;

            calculateChord();
            calculateCamber();
            calculateThickness();
        }

        private void calculateChord() {
            _chord = PairedPoint.max(_airfoilCurve, PairedPoint.Axis.X)?.X - PairedPoint.min(_airfoilCurve, PairedPoint.Axis.X)?.X ?? 0;
        }
        private void calculateCamber() {

            // Calculate camberline
            List<Point> camber = new List<Point>();
            foreach (var point in _airfoilCurve) {
                camber.Add(new Point(
                    point.X,
                    (point.Y1 + point.Y2) / 2)
                );
            }
            _camberLine = camber.ToArray();

            // Calculate Max Camber and its Location
            _maxCamber = max(_camberLine, Axis.Y)?.Y ?? 0;
            _maxCamberLoc = max(_camberLine, Axis.Y)?.X ?? 0;

        }
        private void calculateThickness() {

            // Calculate Thickness Distribution
            List<Point> thickness = new List<Point>();
            foreach (var point in _airfoilCurve) {
                thickness.Add(new Point(
                    point.X,
                    point.Y1 - point.Y2)
                );
            }
            _thicknessDistribution = thickness.ToArray();

            // Calculate Max Thickness and its Location
            _maxThickness = max(_thicknessDistribution, Axis.Y)?.Y ?? 0;
            _maxThicknessLoc = max(_thicknessDistribution, Axis.Y)?.X ?? 0;
        }

        // Utilities 

        //
        // Standardize an Airfoil
        //
        public static Airfoil standardize(Airfoil airfoil) {
            List<PairedPoint> curve = new List<PairedPoint>();
            foreach (var point in airfoil._airfoilCurve) {
                curve.Add(new PairedPoint(
                    point.X / airfoil._chord, 
                    point.Y1 / airfoil._chord,
                    point.Y2 / airfoil._chord)
                );
            }

            return new Airfoil(curve.ToArray());
        }

        //
        // Private Utilities =========================================================================== //
        //

        private enum Axis { X, Y }

        private static double axisValue(Point point, Axis axis) {
            if (axis == Axis.X) return point.X;
            else return point.Y;
        }

        private static Point? max(Point[] points, Axis axis) {
            if (points == null) return null;
            if (points!.Length == 0) return null;

            double maxValue = axisValue(points[0], axis);
            Point max = points[0];
            foreach (var point in points) {
                if (maxValue < axisValue(point, axis)) {
                    maxValue = axisValue(point, axis);
                    max = point;
                }
            }

            return max;
        }
        private static Point? min(Point[] points, Axis axis) {
            if (points == null) return null;
            if (points!.Length == 0) return null;

            double minValue = axisValue(points[0], axis);
            Point min = points[0];
            foreach (var point in points) {
                if (minValue > axisValue(point, axis)) {
                    minValue = axisValue(point, axis);
                    min = point;
                }
            }

            return min;
        }

    }
}
