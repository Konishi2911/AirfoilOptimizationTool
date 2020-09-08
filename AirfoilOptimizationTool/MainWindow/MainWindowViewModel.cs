using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        private const int numberOfParentPreviewWindows = 10;
        private const int numberOfCandidatePreviewWindows = 10;

        private Airfoil.Airfoil[] _currentPopulation;
        private Airfoil.Airfoil[] _candidates;
        private Point[][] _currentPopulationCurves;
        private Point[][] _candidatesCurves;
        private PointCollection[] _drawingCurrentPopulationCurves;
        private PointCollection[] _drawingCandidateCurves;

        private ObservableCollection<Size> _parentCanvasSize;
        private ObservableCollection<Size> _candidateCanvasSize;

        // Constructor
        public MainWindowViewModel() {
            // Instantiate ObservableCollections
            _parentCanvasSize = new ObservableCollection<Size>(new Size[numberOfParentPreviewWindows]);
            _candidateCanvasSize = new ObservableCollection<Size>(new Size[numberOfCandidatePreviewWindows]);

            // Register Evnet handlers
            _parentCanvasSize.CollectionChanged += canvasSizeDidChange;
            _candidateCanvasSize.CollectionChanged += canvasSizeDidChange;
            
            // Generate Mock for debugging
            _currentPopulationCurves = new Point[numberOfParentPreviewWindows][];
            _currentPopulationCurves[0] = new Point[] {
                new Point(10, 0),
                new Point(0, 0)
            };

            updateAirfoilPreviews();
        }

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
        public PointCollection[] drawingCurrentPopulationCurve {
            get => _drawingCurrentPopulationCurves;
            private set {
                _drawingCurrentPopulationCurves = value;
                notifyPropertyDidChange(nameof(drawingCurrentPopulationCurve));
            }
        }
        public PointCollection[] drawingCandidatesCurve {
            get => _drawingCandidateCurves;
            private set {
                _drawingCandidateCurves = value;
                notifyPropertyDidChange(nameof(drawingCandidatesCurve));
            }
        }
        public ObservableCollection<Size> parentCanvasSizes {
            get => _parentCanvasSize;
        }
        public ObservableCollection<Size> candidateCanvasSizes {
            get => _candidateCanvasSize;
        }

        // Callbacks

        //
        // Canvas size was changed
        //
        private void canvasSizeDidChange(object sender, NotifyCollectionChangedEventArgs e) {
            updateAirfoilPreviews();
        }

        //
        // Preview Updater
        //
        private void updateAirfoilPreviews() {
            List<PointCollection> temp_p = new List<PointCollection>();
            for (var i = 0; i < currentPopulationCurves.Length; ++i) {
                temp_p.Add(makeDrawingCurve(currentPopulationCurves[i], parentCanvasSizes[i]));
            }
        }

        //
        // Update Airfoil Curves
        // This function will be called when the preview windows that display airfoils will be updated.
        //
        private static Point[][] updateCurves(Airfoil.Airfoil[] airfoils) {
            List<Point[]> temp_points = new List<Point[]>();
            foreach (var airfoil in airfoils) {
                Interpolation.IInterpolator interpolator = new Interpolation.LinearInterpolator(
                    Airfoil.PairedPoint.convertToPointArray(airfoil.airfoilCurve, Airfoil.PairedPoint.Direction.FromUpperTrailing)
                );
                temp_points.Add(interpolator.curve(airfoilPreviewResolutions));
            }
            return temp_points.ToArray();
        }

        //
        // Make airfoil Curve Points on the Display Coordinate
        //
        private static PointCollection? makeDrawingCurve(Point[] curve, Size canvasSize) {
            if (curve == null) return null;

            AirfoilPreview.Scaler pointScaler = new AirfoilPreview.Scaler(canvasSize, curve);
            return new PointCollection(pointScaler.adjustScale(curve));
        }
    }
}
