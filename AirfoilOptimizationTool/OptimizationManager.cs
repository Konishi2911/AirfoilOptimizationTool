using AirfoilOptimizationTool.AirfoilRepresentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool {
    public class OptimizationManager {
        private AirfoilRepresentation.AirfoilRepresentationManager _airfoilRepManager;

        public const string OptimizationLoggerName = "GAStandardLogger";

        private OptimizationManager() {
        }

        public static OptimizationManager instance { get; } = new OptimizationManager();

        public AirfoilRepresentationManager representationManager { get; set; }
    }
}
