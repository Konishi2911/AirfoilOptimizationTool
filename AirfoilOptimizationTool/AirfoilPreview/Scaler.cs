using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AirfoilOptimizationTool.AirfoilPreview
{
    public class Scaler
    {
        private double _width;
        private double _height;

        private double offsetX;
        private double offsetY;
        private double magnification;

        private Point[][] _pointsArray;

        enum Axis {
            x,
            y
        }

        public Scaler(Canvas canvas, params Point[][] points) {
            _width = canvas.ActualWidth;
            _height = canvas.ActualHeight;

            _pointsArray = points;

            var gmin = min(_pointsArray[0], Axis.x).X;
            var gmax = max(_pointsArray[0], Axis.x).X;
            foreach (var p in _pointsArray) {
                if (min(p, Axis.x).X < gmin) {
                    gmin = min(p, Axis.x).X;
                }
                if (max(p, Axis.x).X > gmax) {
                    gmax = max(p, Axis.x).X;
                }
            }

            var width = gmax - gmin;
            magnification = width == 0 ? 0 : _width / width;
            offsetX = -magnification * gmin;
            offsetY = 0.5 * _height;
        }
        public Scaler(Size size, params Point[][] points) {
            _width = size.Width;
            _height = size.Height;

            _pointsArray = points;

            var gmin = min(_pointsArray[0], Axis.x).X;
            var gmax = max(_pointsArray[0], Axis.x).X;
            foreach (var p in _pointsArray) {
                if (min(p, Axis.x).X < gmin) {
                    gmin = min(p, Axis.x).X;
                }
                if (max(p, Axis.x).X > gmax) {
                    gmax = max(p, Axis.x).X;
                }
            }

            var width = gmax - gmin;
            magnification = width == 0 ? 0 : _width / width;
            offsetX = -magnification * gmin;
            offsetY = 0.5 * _height;
        }

        public Point[] adjustScale(Point[] points) {
            if (points.Length == 0) { return new Point[0]; }

            List<Point> pointSet = new List<Point>();
            foreach (var point in points) {
                pointSet.Add(new Point(
                        point.X * magnification + offsetX,
                        point.Y * magnification + offsetY));
            }
            return pointSet.ToArray();
        }



        private static double component(Point point, Axis axis) {
            if (axis == Axis.x) {
                return point.X;
            } else {
                return point.Y;
            }
        }
        private static Point min(Point[] points, Axis axis) {
            if (points.Length == 0) { return new Point(0, 0); }

            var min = component(points[0], axis);
            var minPoint = points[0];
            for (var i = 0; i < points.Length; ++i) {
                if (min > component(points[i], axis)) {
                    min = component(points[i], axis);
                    minPoint = points[i];
                }
            }

            return minPoint;
        }
        private static Point max(Point[] points, Axis axis) {
            if (points.Length == 0) { return new Point(0, 0); }

            var max = component(points[0], axis);
            var maxPoint = points[0];
            for (var i = 0; i < points.Length; ++i) {
                if (max < component(points[i], axis)) {
                    max = component(points[i], axis);
                    maxPoint = points[i];
                }
            }

            return maxPoint;
        }
    }
}
