using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AirfoilOptimizationTool
{
    public class MainWindowViewModel: ViewModelBase
    {
        private const int airfoilPreviewResolutions = 100;

        private Airfoil.Airfoil[] _currentPopulation;
        private Airfoil.Airfoil[] _candidates;
        private Point[][] _currentPopulationCurves;
        private Point[][] _candidatesCurves;
        private PointCollection[] _drawingCurrentPopulationCurves;
        private PointCollection[] _drawingCandidateCurves;

        private Size[] _canvasSize;

        #region Properties and Setters
        //
        // Properties and Setters
        //
        public Point[][] currentPopulationCurves {
            get => _currentPopulationCurves;
            private set {
                _currentPopulationCurves = value;
                notifyPropertyDidChange(nameof(currentPopulationCurves));
            }
        }
        public Point[][] candidatesCurves {
            get => _candidatesCurves;
            private set {
                _candidatesCurves = value;
                notifyPropertyDidChange(nameof(candidatesCurves));
            }
        }
        public void setNewPopulation(Airfoil.Airfoil[] cp) {
            _currentPopulation = cp;
            _currentPopulationCurves = MainWindowViewModel.updateCurves(_currentPopulation);
        }
        public void setNewCandidates(Airfoil.Airfoil[] cp) {
            _candidates = cp;
            _candidatesCurves = MainWindowViewModel.updateCurves(_candidates);
        }
        #endregion

        //
        // Binding Properties
        //
        public PointCollection[] drawingCurrentPopulationCurves {
            get => _drawingCurrentPopulationCurves;
            private set {
                _drawingCurrentPopulationCurves = value;
                notifyPropertyDidChange(nameof(drawingCurrentPopulationCurves));
            }
        }
        public PointCollection[] drawingCandidateCurves {
            get => _drawingCandidateCurves;
            private set {
                _drawingCandidateCurves = value;
                notifyPropertyDidChange(nameof(drawingCandidateCurves));
            }
        }
        public Size[] canvasSize {
            get => _canvasSize;
            set {
                _canvasSize = value;
            }
        }

        //
        // Preview Updater
        //
        private void updateAirfoilPreviews() {
            List<PointCollection> temp_p = new List<PointCollection>();
            for (var i = 0; i < currentPopulationCurves.Length; ++i) {
                temp_p.Add(makeDrawingCurve(currentPopulationCurves[i], canvasSize[i]));
            }
        }

        //
        // Update Airfoil Curves
        //
        private static Point[][] updateCurves(Airfoil.Airfoil[] airfoils) {
            List<Point[]> temp_points = new List<Point[]>();
            foreach (var airfoil in airfoils) {
                Interpolation.IInterpolator interpolator = new Interpolation.LinearInterpolator(airfoil.airfoilCurve);
                temp_points.Add(interpolator.curve(airfoilPreviewResolutions));
            }
            return temp_points.ToArray();
        }

        //
        // Make airfoil Curve Points in the Display Coordinate
        //
        private static PointCollection makeDrawingCurve(Point[] curve, Size canvasSize) {
            AirfoilPreview.Scaler pointScaler = new AirfoilPreview.Scaler(canvasSize, curve);

            return new PointCollection(pointScaler.adjustScale(curve));
        }
    }
}
