using AirfoilOptimizationTool.MainWindow.Messenger;
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

        // ## Airfoil Preview View
        private Airfoil.Airfoil[] _currentPopulation;
        private Airfoil.Airfoil[] _candidates;
        private Point[][] _currentPopulationCurves;
        private Point[][] _candidatesCurves;
        private ObservableCollection<PointCollection> _drawingCurrentPopulationCurves;
        private ObservableCollection<PointCollection> _drawingCandidateCurves;
        private int displayingRangeStartsIndexOfParents;
        private int displayinhRangeStartsIndexOfCandidates;

        private Size _parentsCanvasSize;
        private Size _candidatesCanvasSize;


        // ## Airfoil Detail View
        private PointCollection _drawingDetailCurve;
        private PointCollection _leadingEdgeCurve;
        private PointCollection _camberLineCurve;

        private Size _detailCanvasSize;


        // Event
        private delegate void CanvasSizeWasChangedEventhandler();
        private event CanvasSizeWasChangedEventhandler CanvasSizeWasChanged;

        // Constructor
        public MainWindowViewModel() {
            // Set Displaying Airfoil Preview Range
            displayingRangeStartsIndexOfParents = 0;
            displayinhRangeStartsIndexOfCandidates = 0;

            // Instantiate CanvasSizes
            _parentsCanvasSize = new Size();
            _candidatesCanvasSize = new Size();

            // Instantiate Drawing Curve Collection
            _drawingCurrentPopulationCurves = new ObservableCollection<PointCollection>(new PointCollection[numberOfParentPreviewWindows]);
            _drawingCandidateCurves = new ObservableCollection<PointCollection>(new PointCollection[numberOfCandidatePreviewWindows]);

            // Generate Mock for debugging --------------------------------------------- //
            _currentPopulationCurves = new Point[numberOfParentPreviewWindows][];
            _currentPopulationCurves[0] = new Point[] {
                new Point(1, 0),
                new Point(0.3, 0.05),
                new Point(0, 0),
                new Point(0.5, 0.01),
                new Point(1, 0)
            };
            // ------------------------------------------------------------------------- //

            // Register Callbacks to EventHandler
            CanvasSizeWasChanged += canvasSizeWasChanged;

            updateAirfoilPreviews();
        }

        #region Properties and Setters
        //
        // Airfoil Preview Curves ==================================== //
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
        // Binding Properties =========================================== //
        //
        // ## Airfoil Preview View
        public ObservableCollection<PointCollection> drawingCurrentPopulationCurve {
            get => _drawingCurrentPopulationCurves;
            private set {
                _drawingCurrentPopulationCurves = value;
                notifyPropertyDidChange(nameof(drawingCurrentPopulationCurve));
            }
        }
        public ObservableCollection<PointCollection> drawingCandidatesCurve {
            get => _drawingCandidateCurves;
            private set {
                _drawingCandidateCurves = value;
                notifyPropertyDidChange(nameof(drawingCandidatesCurve));
            }
        }


        // ## Airfoil Detail View
        public PointCollection drawingDetailCurve {
            get => _drawingDetailCurve;
            set {
                _drawingDetailCurve = value;
                notifyPropertyDidChange(nameof(drawingDetailCurve));
            }
        }
        public PointCollection drawingLeadingEdgeCurve {
            get => _leadingEdgeCurve;
            set {
                _leadingEdgeCurve = value;
                notifyPropertyDidChange(nameof(drawingLeadingEdgeCurve));
            }
        }
        public PointCollection drawingCamberLineCurve {
            get => _camberLineCurve;
            set {
                _camberLineCurve = value;
                notifyPropertyDidChange(nameof(drawingCamberLineCurve));
            }
        }

        // ## Genetic Algorithm Configuration
        public TriggerCommand showGAConfigurationDialog => new TriggerCommand(
            () => {
                GAConfigurationMessenger.instance.showDialog();
            }, 
            () => true
        );
        


        //
        // Properties of Canvas size ========================= //
        //
        // ## Airfoil Preview View
        public Size parentsCanvasSize {
            get => _parentsCanvasSize;
            set {
                _parentsCanvasSize = value;
                CanvasSizeWasChanged();
            }
        }
        public Size candidatesCanvasSize {
            get => _candidatesCanvasSize;
            set {
                _candidatesCanvasSize = value;
                CanvasSizeWasChanged();
            }
        }

        // ## Airfoil Detail View
        public Size detailCanvasSize {
            get => _detailCanvasSize;
            set {
                _detailCanvasSize = value;
                CanvasSizeWasChanged();
            }
        }

        //
        // Callbacks ========================================== //

        //
        // Notify when properties in this class be changed
        // 
        private void canvasSizeWasChanged() {
            updateAirfoilPreviews();
        }


        //
        // Updating Previews ================================== //

        //
        // Preview Updater
        //
        private void updateAirfoilPreviews() {
            List<PointCollection> temp_p = new List<PointCollection>();
            List<PointCollection> temp_c = new List<PointCollection>();

            // Generate Points Drawing on each Canvas
            if (currentPopulationCurves != null) {
                for (var i = 0; i < currentPopulationCurves.Length; ++i) {
                    var dcp = makeDrawingCurve(currentPopulationCurves?[i], _parentsCanvasSize);
                    if (dcp != null) temp_p.Add(dcp);
                }
            }
            if (candidatesCurves != null) {
                for (var i = 0; i < candidatesCurves.Length; ++i) {
                    var dcc = makeDrawingCurve(candidatesCurves?[i], _candidatesCanvasSize);
                    if (dcc != null) temp_c.Add(dcc);
                }
            }

            // Update Drawing Curve Points
            //
            // Parents
            ObservableCollection<PointCollection> tempDrawingP = new ObservableCollection<PointCollection>();
            for (int i = 0; i < numberOfParentPreviewWindows; ++i) {
                if (i + displayingRangeStartsIndexOfParents < temp_p.Count) 
                    tempDrawingP.Add(temp_p[i + displayinhRangeStartsIndexOfCandidates]);
                else tempDrawingP.Add(new PointCollection());
            }
            drawingCurrentPopulationCurve = tempDrawingP;
            //
            // Candidates
            ObservableCollection<PointCollection> tempDrawingC = new ObservableCollection<PointCollection>();
            for (int i = 0; i < numberOfCandidatePreviewWindows; ++i) {
                if (i + displayinhRangeStartsIndexOfCandidates < temp_c.Count) 
                    tempDrawingC.Add(temp_c[i + displayinhRangeStartsIndexOfCandidates]);
                else tempDrawingC.Add(new PointCollection());
            }
            drawingCandidatesCurve = tempDrawingC;
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
            return new PointCollection(pointScaler.adjustScale(curve, true));
        }
    }
}
