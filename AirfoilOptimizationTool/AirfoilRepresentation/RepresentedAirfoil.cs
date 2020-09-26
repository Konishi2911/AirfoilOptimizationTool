using AirfoilOptimizationTool.AirfoilRepresentation.Method;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace AirfoilOptimizationTool.AirfoilRepresentation {
    public class RepresentedAirfoil {
        private IAirfoilRepresentationMethod representationMethod;
        private Airfoil.Airfoil _airfoil;
        private double[] parameter;

        public Airfoil.Airfoil airfoil => _airfoil;
        public double[] optimizationParameters {
            get => parameter;
        }

        public RepresentedAirfoil(IAirfoilRepresentationMethod method, double[]? parameter) {
            representationMethod = method;
            this.parameter = parameter ?? new double[method.numberOfParameters()];

            _airfoil = representationMethod.getAirfoil(this.parameter);
        }
    }
}
