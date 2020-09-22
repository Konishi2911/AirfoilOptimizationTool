using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool.AirfoilRepresentation.Method {
    public interface IAirfoilRepresentationMethod {
        public Airfoil.Airfoil getAirfoil(double[] weight);
    }
}
