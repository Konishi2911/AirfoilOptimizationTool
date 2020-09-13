using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool.GAProgress {
    class GAProgressManager {
        private int _generation;

        private Airfoil.Airfoil[] _parentAirfoils;
        private Airfoil.Airfoil[] _candidateAirfoils;

        private AirfoilRepresentation.Method.IAirfoilRepresentationMethod representationMethod;
    }
}
