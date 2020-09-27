using AirfoilOptimizationTool.AirfoilRepresentation;
using AirfoilOptimizationTool.AirfoilRepresentation.Method;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace AirfoilOptimizationTool {
    public class OptimizationManager {
        public const string OptimizationLoggerName = "GAStandardLogger";


        private AirfoilRepresentation.RepresentedAirfoilsManager? _airfoilRepManager;
        private GAProgress.GAProgressManager _gaProgressManager;

        private event EventHandler representedAirfoilsMethodDidSet;
        public event EventHandler representedAirfoilsMethodBeAvailable;
        public event EventHandler optimizationParamtersDidChange;

        public RepresentedAirfoilsManager? representedAirfoilsManager {
            get => _airfoilRepManager;
            set {
                _airfoilRepManager = value;
                
                if (_airfoilRepManager != null) {
                    _airfoilRepManager.optimizationParametersDidChangeByModel += _airfoilRepManager_optimizationParametersDidChange;

                    representedAirfoilsMethodDidSet?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private OptimizationManager() {
            _airfoilRepManager = null;
            _gaProgressManager = new GAProgress.GAProgressManager();

            _gaProgressManager.populationSizeDidChange += _gaProgressManager_populationSizeDidChange;
            representedAirfoilsMethodDidSet += OptimizationManager_representedAirfoilsMethodDidSet;

            _gaProgressManager.populationSize = 10;
        }

        public static OptimizationManager instance { get; } = new OptimizationManager();


        // Event callback
        private void _gaProgressManager_populationSizeDidChange(object sender, EventArgs e) {
            _airfoilRepManager?.setNumberOfAirfoils(_gaProgressManager.populationSize);
        }
        private void _airfoilRepManager_optimizationParametersDidChange(object sender, EventArgs e) {
            optimizationParamtersDidChange?.Invoke(sender, e);
        }
        private void OptimizationManager_representedAirfoilsMethodDidSet(object sender, EventArgs e) {
            _airfoilRepManager?.setNumberOfAirfoils(_gaProgressManager.populationSize);

            representedAirfoilsMethodBeAvailable?.Invoke(sender, e);
        }

    }
}
