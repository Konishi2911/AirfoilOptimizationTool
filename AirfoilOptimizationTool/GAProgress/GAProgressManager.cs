using AirfoilOptimizationTool.AirfoilRepresentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool.GAProgress {
    class GAProgressManager {
        private int _generation;

        private int _populationSize;

        private RepresentedAirfoil[] _parentAirfoils;
        private RepresentedAirfoil[] _candidateAirfoils;

        public event EventHandler populationSizeDidChange;

        public int populationSize { 
            get => _populationSize;
            set {
                _populationSize = value;
                populationSizeDidChange?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
