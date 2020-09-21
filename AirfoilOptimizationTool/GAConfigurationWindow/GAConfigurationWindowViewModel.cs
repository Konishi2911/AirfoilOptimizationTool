using AirfoilOptimizationTool.AirfoilPreview;
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
            //_basisAirfoilItems.CollectionChanged += basisAirfoilsDidChange;


            // Generate Mock for debugging --------------------------------------------- //
            var temp = new Point[] {
                new System.Windows.Point(100, 30),
                new System.Windows.Point(0, 30)
            };
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

        public TriggerCommand addButtonDidClick => new TriggerCommand(() => {
            var path = OpenFileSelectorDialogMessenger.instance.showDialog();

            SeparatedValues csv = new SeparatedValues(',');

            Logger.getLogger("GAStandardLogger").debug("Open CSV File from \"" + path + "\"");
            try {
                //
                // Open CSV
                csv.openFromFile(path);

                //
                // Create Airfoil Instance
                var airfoil = new AirfoilShapeImporter(csv).getAirfoil();
                var basisAirfoilItem = new BasisAirfoilItem(airfoil);

                //
                // Regist Event Callback
                basisAirfoilItem.canvasSizeDidChange += canvasSizeDidChange;

                //
                // Add new basis Airfoil item
                basisAirfoilItems.Add(basisAirfoilItem);
            }
            catch (ArgumentNullException e) {
                Logger.getLogger("GAStandardLogger").error(e.Message);
            }
            catch (System.IO.PathTooLongException e) {
                Logger.getLogger("GAStandardLogger").error(e.Message);
            }
            catch (System.IO.DirectoryNotFoundException e) {
                Logger.getLogger("GAStandardLogger").error(e.Message);
            }
            catch (UnauthorizedAccessException e) {
                Logger.getLogger("GAStandardLogger").error(e.Message);
            }
            catch (System.IO.FileNotFoundException e) {
                Logger.getLogger("GAStandardLogger").error(e.Message);
            }

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
        private void canvasSizeDidChange(in BasisAirfoilItem sender, EventArgs _) {
            var converter = new AirfoilPreviewCurveConverter(sender.airfoil, sender.canvasSize);
            var newCurve = converter.getDrawingOutlineCurve();

            sender.airfoilCurve = newCurve;
        }
        #endregion
    }
}
