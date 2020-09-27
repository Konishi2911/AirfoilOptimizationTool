using AirfoilOptimizationTool.AirfoilPreview;
using AirfoilOptimizationTool.Logs;
using AirfoilOptimizationTool.Logs.Appenders;
using AirfoilOptimizationTool.MainWindow;
using AirfoilOptimizationTool.MainWindow.Messenger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable enable

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
        private ObservableCollection<PointCollection> _currentPopulationCurves;
        private ObservableCollection<PointCollection> _candidatesCurves;
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

        // ## Parameters View
        private ParametersTableGenerator _parametersTableGenerator;
        private DataTable _parametersTable;

        // ## Log View
        private string _logMessage;


        // Event
        private delegate void CanvasSizeWasChangedEventhandler();
        private event CanvasSizeWasChangedEventhandler CanvasSizeWasChanged;

        // Constructor
        public MainWindowViewModel() {
            // Set Displaying Airfoil Preview Range
            displayingRangeStartsIndexOfParents = 0;
            displayinhRangeStartsIndexOfCandidates = 0;

            // Instantiate
            _parentsCanvasSize = new Size();
            _candidatesCanvasSize = new Size();

            _drawingCurrentPopulationCurves = new ObservableCollection<PointCollection>(new PointCollection[numberOfParentPreviewWindows]);
            _drawingCandidateCurves = new ObservableCollection<PointCollection>(new PointCollection[numberOfCandidatePreviewWindows]);

            _parametersTableGenerator = new ParametersTableGenerator();
            _parametersTable = new DataTable();

            //Setup Logger
            TextFieldAppender appender = new TextFieldAppender();
            appender.ReadyToLog += Appender_ReadyToLog;
            appender.logLevel = Security.Info;
            appender.setFormat("[%d] [%p] : %m");
            Logger.getLogger("GAStandardLogger").addAppender(appender);

            // Generate Mock for debugging --------------------------------------------- //

            // ------------------------------------------------------------------------- //

            // Register Callbacks to EventHandler
            CanvasSizeWasChanged += canvasSizeWasChanged;
            _parametersTableGenerator.ParametersTableSourceDidUpdate += _parametersTableGenerator_ParametersTableSourceDidUpdate;

            updateAirfoilPreviews();

            Logger.getLogger("GAStandardLogger").info("Start Application...");
        }

        private void Appender_ReadyToLog(object sender, Appender.LogReadyEventArgs e) {
            logMessage += e.logMessage;
        }

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

        //
        // ## Optimization Parameters View


        public DataTable optimizationParameters {
            get {
                return _parametersTable;
            }
            set {
                _parametersTable = value;
                notifyPropertyDidChange(nameof(optimizationParameters));
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

        // ## Log View
        public string logMessage {
            get => _logMessage;
            set {
                _logMessage = value;
                notifyPropertyDidChange(nameof(logMessage));
            }
        }

        // ## Menu: Genetic Algorithm Configuration
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

        #region Callbacks
        //
        // Callbacks ========================================== //

        //
        // Notify when properties in this class be changed
        // 
        private void canvasSizeWasChanged() {
            updateAirfoilPreviews();
        }
        

        //
        // Parameters Table Did update
        private void _parametersTableGenerator_ParametersTableSourceDidUpdate(object sender, EventArgs e) {

            // Update Parameters TableView
            if (!((ParametersTableGenerator)sender).isUnderProcessing) {
                optimizationParameters = _parametersTableGenerator.getParameterTable();
            }

            // Update Airfoils Curve 
            updateAirfoilPreviews();
        }

        #endregion


        //
        // Updating Previews ================================== //

        //
        // Preview Updater
        //
        private void updateAirfoilPreviews() {
            // Parents
            _currentPopulationCurves?.Clear();
            var manager = OptimizationManager.instance.representedAirfoilsManager;
            if (manager != null) {

                var tempCollection = new ObservableCollection<PointCollection>();
                foreach (var airfoil in manager!.getAllAirfoils()) {

                    // configure Converter
                    var converter = new AirfoilPreviewCurveConverter(airfoil)!;
                    converter.setPreviewRegionSize(parentsCanvasSize);

                    tempCollection.Add(converter.getDrawingOutlineCurve());
                }
                drawingCurrentPopulationCurve = tempCollection;
            }
        }
    }
}
