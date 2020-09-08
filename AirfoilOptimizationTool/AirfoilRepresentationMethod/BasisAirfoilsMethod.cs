using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool.AirfoilRepresentationMethod {
    class BasisAirfoilsMethod {
        private int numberOfPoints;
        private Airfoil.Airfoil[] _basisAirfoils;
        private double[] weights;

        public int numberOfBasis => _basisAirfoils.Length;

        //
        // Constructors
        //
        public BasisAirfoilsMethod(Airfoil.Airfoil[] basis) {
            List<Airfoil.Airfoil> airfoils = new List<Airfoil.Airfoil>();
            foreach (var airfoil in basis) {
                airfoils.Add(Airfoil.Airfoil.standardize(airfoil));
            }

            _basisAirfoils = airfoils.ToArray();
        }

        public Airfoil.Airfoil getAirfoil(double[] weight) {
            if (_basisAirfoils.Length != weight.Length) return null;

            List<Airfoil.PairedPoint> newCurve = new List<Airfoil.PairedPoint>();
            for (var i = 0; i < numberOfPoints; ++i) {
                double tempX = 0.0, tempY1 = 0.0, tempY2 = 0.0;
                for (var j = 0; j < weight.Length; ++j) {
                    tempX += _basisAirfoils[j].airfoilCurve[i].X * weight[j];
                    tempY1 += _basisAirfoils[j].airfoilCurve[i].Y1 * weight[j];
                    tempY2 += _basisAirfoils[j].airfoilCurve[i].Y2 * weight[j];
                }
                newCurve.Add(new Airfoil.PairedPoint(tempX, tempY1, tempY2));
            }

            return new Airfoil.Airfoil(newCurve.ToArray());
        }
    }
}
