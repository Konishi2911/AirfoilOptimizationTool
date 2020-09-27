using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirfoilOptimizationTool.Airfoil;
using AirfoilOptimizationTool.DataIO;

namespace AirfoilOptimizationTool.AirfoilRepresentation.Method {
    class BasisAirfoilsMethod : IAirfoilRepresentationMethod {
        private int numberOfPoints = 200;
        private Airfoil.Airfoil[] _basisAirfoils;

        public int numberOfBasis => _basisAirfoils.Length;
        public Airfoil.Airfoil[] basisAirfoils => _basisAirfoils;

        public string[] captionOfParameter {
            get {
                var tempCaption = new List<string>();
                foreach (var basisAirfoil in _basisAirfoils) {
                    tempCaption.Add(basisAirfoil.name);
                }

                return tempCaption.ToArray();
            }
        }

        //
        // Constructors
        //
        public BasisAirfoilsMethod(Airfoil.Airfoil[] basis) {
            List<Airfoil.Airfoil> airfoils = new List<Airfoil.Airfoil>();

            foreach (var airfoil in basis) {
                var importer = new AirfoilShapeImporter(PairedPoint.convertToPointArray(airfoil.airfoilCurve, PairedPoint.Direction.FromUpperTrailing));
                var tempAirfoil = importer.getAirfoil(numberOfPoints);
                tempAirfoil.name = airfoil.name;
                airfoils.Add(tempAirfoil);
            }

            _basisAirfoils = airfoils.ToArray();
        }


        public int numberOfParameters() {
            return basisAirfoils.Length;
        }
        public Airfoil.Airfoil getAirfoil(double[] weights) {
            if (_basisAirfoils == null) return null;
            if (_basisAirfoils.Length == 0) return null;
            if (_basisAirfoils.Length != weights.Length) return null;

            List<Airfoil.PairedPoint> newCurve = new List<Airfoil.PairedPoint>();
            for (var i = 0; i < numberOfPoints; ++i) {
                double tempX = 0.0, tempY1 = 0.0, tempY2 = 0.0;

                tempX = _basisAirfoils[0].airfoilCurve[i].X;
                for (var j = 0; j < weights.Length; ++j) {
                    tempY1 += _basisAirfoils[j].airfoilCurve[i].Y1 * weights[j];
                    tempY2 += _basisAirfoils[j].airfoilCurve[i].Y2 * weights[j];
                }
                newCurve.Add(new Airfoil.PairedPoint(tempX, tempY1, tempY2));
            }

            return new Airfoil.Airfoil(newCurve.ToArray());
        }
    }
}
