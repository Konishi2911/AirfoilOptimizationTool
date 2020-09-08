using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool.Airfoil {
    public struct PairedPoint {
        public double X;
        public double Y1;
        public double Y2;

        public enum Axis { X, Y1, Y2 }

        public PairedPoint(double X, double Y1, double Y2) {
            this.X = X;
            this.Y1 = Y1;
            this.Y2 = Y2;
        }


        // Utilities

        private static double axisValue(PairedPoint point, Axis axis) {
            if (axis == Axis.X) return point.X;
            else if (axis == Axis.Y1) return point.Y1;
            else return point.Y2;
        }
        public static PairedPoint? max(PairedPoint[] points, Axis axis) {
            if (points.Length == 0) return null;

            double maxValue = axisValue(points[0], axis);
            PairedPoint max = points[0];
            foreach (var point in points) {
                if (maxValue < axisValue(point, axis)) {
                    maxValue = axisValue(point, axis);
                    max = point;
                }
            }

            return max;
        }
        public static PairedPoint? min(PairedPoint[] points, Axis axis) {
            if (points.Length == 0) return null;

            double minValue = axisValue(points[0], axis);
            PairedPoint min = points[0];
            foreach (var point in points) {
                if (minValue > axisValue(point, axis)) {
                    minValue = axisValue(point, axis);
                    min = point;
                }
            }

            return min;
        }

        //
        // Convert to Point Array
        //
        public enum Direction { FromUpperTrailing, FromLowerTrailing, FromUpperLeading, FromLowerLeading }
        public static System.Windows.Point[] convertToPointArray(PairedPoint[] pPoints, Direction d) {
            List<System.Windows.Point> points = new List<System.Windows.Point>();

            switch (d) {
                case Direction.FromLowerLeading:
                    for (var i = 0; i < pPoints.Length; ++i) {
                        points.Add(new System.Windows.Point(pPoints[i].X, pPoints[i].Y2));
                    }
                    for (var i = pPoints.Length - 1; i >= 0; --i) {
                        points.Add(new System.Windows.Point(pPoints[i].X, pPoints[i].Y1));
                    }
                    break;

                case Direction.FromLowerTrailing:
                    for (var i = pPoints.Length - 1; i >= 0; --i) {
                        points.Add(new System.Windows.Point(pPoints[i].X, pPoints[i].Y2));
                    }
                    for (var i = 0; i < pPoints.Length; ++i) {
                        points.Add(new System.Windows.Point(pPoints[i].X, pPoints[i].Y1));
                    }
                    break;

                case Direction.FromUpperLeading:
                    for (var i = 0; i < pPoints.Length; ++i) {
                        points.Add(new System.Windows.Point(pPoints[i].X, pPoints[i].Y1));
                    }
                    for (var i = pPoints.Length - 1; i >= 0; --i) {
                        points.Add(new System.Windows.Point(pPoints[i].X, pPoints[i].Y2));
                    }
                    break;

                case Direction.FromUpperTrailing:
                    for (var i = pPoints.Length - 1; i >= 0; --i) {
                        points.Add(new System.Windows.Point(pPoints[i].X, pPoints[i].Y1));
                    }
                    for (var i = 0; i < pPoints.Length; ++i) {
                        points.Add(new System.Windows.Point(pPoints[i].X, pPoints[i].Y2));
                    }
                    break;
            }
            return points.ToArray();
        }
    }
}
