using AirfoilOptimizationTool.DataIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AirfoilOptimizationTool.General.Converters {
    class SeparatedValueToPointConverter {

        public SeparatedValueToPointConverter() {

        }
        
        /// <summary>
        /// Get Points with array from separated values.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Point[] convertFrom(SeparatedValues sv) {
            if (sv == null) throw new ArgumentNullException(nameof(sv));

            var points = sv.getDoubleValue();

            // Convert to Point array
            List<Point> tempPoints = new List<Point>();
            foreach (var point in points) {
                if (point.Length != 2) throw new ArgumentException("Expected array length is 2, but actual length was " + point.Length.ToString(), nameof(points));

                tempPoints.Add(new Point(point[0], point[1]));
            }

            return tempPoints.ToArray(); 
        }
    }
}
