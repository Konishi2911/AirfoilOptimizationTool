using AirfoilOptimizationTool.DataIO;
using AirfoilOptimizationTool.GAConfigurationWindow.AirfoilRepresentation;
using AirfoilOptimizationTool.GAConfigurationWindow.Messenger;
using AirfoilOptimizationTool.Logs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AirfoilOptimizationTool.GAConfigurationWindow {
    class GAConfigurationWindowViewModel : ViewModelBase {
        private static readonly string[] airfoilRepMethod = {
            "Base Airfoil Method" ,
            "B-Spline Method",
            "PARSEC Method"
        };

        private string _selectedRepMethod;
        private ObservableCollection<BasisAirfoilItem> _basisAirfoilItems;
        private Size _canvasSize;

        public GAConfigurationWindowViewModel() {
            // instatntiate and initialize
            _basisAirfoilItems = new ObservableCollection<BasisAirfoilItem>();
            _selectedRepMethod = airfoilRepMethod[0];

            // Regist callbacks 
            PropertyChanged += thisPropertisDidChange;
            _basisAirfoilItems.CollectionChanged += basisAirfoilsDidChange;


            // Generate Mock for debugging --------------------------------------------- //
            var temp = new Point[] {
                new System.Windows.Point(100, 30),
                new System.Windows.Point(0, 30)
            };
            basisAirfoilItems.Add(new BasisAirfoilItem("airfoil", makeDrawingCurve(temp, new Size(376, 47))));
            // ------------------------------------------------------------------------- //

        }

        #region Binding Properties
        //
        // binding Prorperties ======================================================= //
        //

        // ## Airfoil Representation Method
        public string[] airfoilRepresentationMethod => airfoilRepMethod;
        public string selectedRepresentationMethod {
            get => _selectedRepMethod;
            set {
                _selectedRepMethod = value;
                notifyPropertyDidChange(nameof(selectedRepresentationMethod));
            }
        }

        // ## Basis Airfoil Method
        public ObservableCollection<BasisAirfoilItem> basisAirfoilItems => _basisAirfoilItems;
        public Size canvasSize {
            get => _canvasSize;
            set {
                _canvasSize = value;
                notifyPropertyDidChange(nameof(canvasSize));
            }
        }
        public TriggerCommand addButtonDidClick => new TriggerCommand(() => {
            var path = OpenFileSelectorDialogMessenger.instance.showDialog();

            SeparatedValues csv = new SeparatedValues(',');
            csv.openFromFile(path);
        }, () => true);

        #endregion

        #region Callbacks
        //
        // Callbacks ================================================================= //
        //
        
        public void thisPropertisDidChange(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(selectedRepresentationMethod)) {
                if (_selectedRepMethod == "BaseAirfoilMethod") {

                }
            }
        }
        private void basisAirfoilsDidChange(object sender, NotifyCollectionChangedEventArgs e) {
            notifyPropertyDidChange(nameof(basisAirfoilItems));
        }
        #endregion


        //
        // Private
        //

        //
        // Make airfoil Curve Points on the Display Coordinate
        private static System.Windows.Media.PointCollection? makeDrawingCurve(Point[] curve, Size canvasSize) {
            if (curve == null) return null;

            AirfoilPreview.Scaler pointScaler = new AirfoilPreview.Scaler(canvasSize, curve);
            return new System.Windows.Media.PointCollection(pointScaler.adjustScale(curve, true));
        }
    }
}
