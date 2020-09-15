using AirfoilOptimizationTool.AirfoilRepresentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool {
    class OptimizationManager {
        private AirfoilRepresentation.AirfoilRepresentationManager _airfoilRepManager;

        private OptimizationManager() {
        }

        public static OptimizationManager instance { get; } = new OptimizationManager();
    }
}
