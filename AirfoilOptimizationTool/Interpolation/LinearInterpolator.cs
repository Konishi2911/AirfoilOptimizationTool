using AirfoilOptimizationTool.Airfoil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AirfoilOptimizationTool.Interpolation
{
    public class LinearInterpolator : IInterpolator
    {
        private Point[] _points;

        public LinearInterpolator(Point[] points) {
            _points = points;
        }

        public Point[] curve(int N) {
            List<Point> temp_p = new List<Point>();

            for (var i = 0; i < N; ++i) {
                temp_p.Add(interpolation((double)i / (double)(N - 1)));
            }
            return temp_p.ToArray();
        }

        public Point[] curveByConstantX(int N) {
            var temp_p = new List<Point>();
            var width = ((Point)_points.max(PointExtensions.Axis.X)).X - ((Point)_points.min(PointExtensions.Axis.X)).X;

            for (var i = 0; i < N; ++i) {
                //var x = width * (double)i / (double)(N - 1) + _points[0].X;
                var x = width * (1 - Math.Cos(i / (double)(N - 1) * 0.5 * Math.PI)) + _points[0].X;
                var y = 0.0;
                if (i == N - 1) {
                    y = _points[_points.Length - 1].Y;
                } 
                else {
                    for (var j = 0; j < _points.Length - 1; ++j) {
                        if (!(_points[j].X <= x && x < _points[j + 1].X)) continue;
                        y = _points[j].Y + (x - _points[j].X) * (_points[j + 1].Y - _points[j].Y) / (_points[j + 1].X - _points[j].X);
                        break;
                    }
                }
                temp_p.Add(new Point(x, y));
            }
            return temp_p.ToArray();
        }

        public Point interpolation(double t) {
            Point point = new Point();
            double ref_t = t * (_points.Length - 1);

            if (t == 1) {
                point = new Point(_points[_points.Length - 1].X, _points[_points.Length - 1].Y);
            } 
            else {
                for (var i = 1; i < _points.Length - 1; ++i) {
                    if (!(i <= ref_t && ref_t <= i + 1)) { continue; }
                    point = new Point(
                            _points[i].X + (_points[i + 1].X - _points[i].X) * (ref_t - i),
                            _points[i].Y + (_points[i + 1].Y - _points[i].Y) * (ref_t - i)
                        );
                }
            }
            return point;
        }
    }
}
