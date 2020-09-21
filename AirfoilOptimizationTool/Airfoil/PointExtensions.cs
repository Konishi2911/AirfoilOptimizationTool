using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AirfoilOptimizationTool.Airfoil {
    public static class PointExtensions {
        public enum Axis { X, Y }

        private static double axisValue(Point point, Axis axis) {
            if (axis == Axis.X) return point.X;
            else return point.Y;
        }
        public static Point? max(this Point[] points, Axis axis) {
            if (points.Length == 0) return null;

            double maxValue = axisValue(points[0], axis);
            var max = points[0];
            foreach (var point in points) {
                if (maxValue < axisValue(point, axis)) {
                    maxValue = axisValue(point, axis);
                    max = point;
                }
            }

            return max;
        }
        public static Point? min(this Point[] points, Axis axis) {
            if (points.Length == 0) return null;

            double minValue = axisValue(points[0], axis);
            var min = points[0];
            foreach (var point in points) {
                if (minValue > axisValue(point, axis)) {
                    minValue = axisValue(point, axis);
                    min = point;
                }
            }

            return min;
        }
        public static int find(this Point[] source, Point target) {
            for (var i = 0; i < source.Length; ++i) {
                if (source[i] == target) return i;
            }
            return -1;
        }

    }
}
