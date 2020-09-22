using AirfoilOptimizationTool.AirfoilPreview;
using AirfoilOptimizationTool.AirfoilRepresentation;
using AirfoilOptimizationTool.AirfoilRepresentation.Method;
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
        private Visibility _basisAirfoilConfiguratorVisibility;

        private BasisAirfoilConfigManager basisAirfoilMethodConfigManager;
        private Size _canvasSize;
        private int selectedBasisAirfoilItemIndex1;

        public GAConfigurationWindowViewModel() {
            // instatntiate and initialize
            _basisAirfoilConfiguratorVisibility = Visibility.Visible;
            basisAirfoilMethodConfigManager = new BasisAirfoilConfigManager();
            _selectedRepMethod = airfoilRepMethod[0];

            // Regist callbacks 
            PropertyChanged += thisPropertisDidChange;
            basisAirfoilMethodConfigManager.readyStatusDidChange += BasisAirfoilMethodConfigManager_readyStatusDidChange;

            // Regist TriggerCommands
            closingWindow = new TriggerCommand(closingWindowExecute, closingWindowCanExecute);
            applyButtonDidClick = new TriggerCommand(applyButtonExecute, applyButtonCanExecute);
            cancelButtonDidClick = new TriggerCommand(cancelButtonExecute, cancelButtonCanExecute);
            addButtonDidClick = new TriggerCommand(addButtonExecute, addButtunCanExecute);
            removeButtonDidClick = new TriggerCommand(removeButtonExecute, removeButtonCenExecute);

            // Load Current Configuration 

            // Basis Airfoil Method ---------------------------------------------------- //
            var basisAirfoilMethod = OptimizationManager.instance.representationManager?.airfoilRepresentationMethod as BasisAirfoilsMethod;
            if (basisAirfoilMethod != null) {
                foreach (var airfoil in basisAirfoilMethod?.basisAirfoils) {
                    basisAirfoilItems.Add(new BasisAirfoilItem(airfoil));
                }
            }
            // ------------------------------------------------------------------------- //
            // B-Spline Method --------------------------------------------------------- //

            // ------------------------------------------------------------------------- //
            // PARSEC Method ----------------------------------------------------------- //

            // ------------------------------------------------------------------------- //




            // Generate Mock for debugging --------------------------------------------- //

            // ------------------------------------------------------------------------- //

        }


        #region Binding Properties
        //
        // binding Prorperties ======================================================= //
        //

        //
        // ## General 
        //
        public TriggerCommand closingWindow { get; }
        private void closingWindowExecute() {
            OpenFileSelectorDialogMessenger.instance.disconnect();
            CloseDialogMessenger.instance.disconnect();
        }
        private bool closingWindowCanExecute() {
            return true;
        }

        public TriggerCommand applyButtonDidClick { get; }
        private void applyButtonExecute() {
            IAirfoilRepresentationMethod method = null;

            //
            // Basis airfoil Method
            if (selectedRepresentationMethod == airfoilRepMethod[0]) {

                Logger.getLogger(OptimizationManager.OptimizationLoggerName).info("Basis Airfoil Method was Configured. Basis Airfoils are showed in following List.");

                var tempAirfoils = new List<Airfoil.Airfoil>();
                foreach (var airfoilItem in basisAirfoilItems) {
                    tempAirfoils.Add(airfoilItem.airfoil);

                    Logger.getLogger(OptimizationManager.OptimizationLoggerName).info(" >> " + airfoilItem.airfoilName);
                }

                method = new BasisAirfoilsMethod(tempAirfoils.ToArray());

            }
            //
            // B-Spline Method
            else if (selectedRepresentationMethod == airfoilRepMethod[1]) {

            }
            //
            // PARSEC Method
            else {

            }

            OptimizationManager.instance.representationManager = new AirfoilRepresentationManager(method);

            // Close Window
            CloseDialogMessenger.instance.requestClosingDialog();

        }
        private bool applyButtonCanExecute() {
            //
            // Basis airfoil Method
            if (selectedRepresentationMethod == airfoilRepMethod[0]) {
                return basisAirfoilMethodConfigManager.isReady;
            }
            //
            // B-Spline Method
            else if (selectedRepresentationMethod == airfoilRepMethod[1]) {
                return false;
            }
            //
            // PARSEC Method
            else {
                return false;
            }
        }

        public TriggerCommand cancelButtonDidClick { get; }
        private void cancelButtonExecute() {
            CloseDialogMessenger.instance.requestClosingDialog();
        }
        private bool cancelButtonCanExecute() {
            return true;
        }

        //
        // ## Airfoil Representation Method
        //
        public string[] airfoilRepresentationMethod => airfoilRepMethod;
        public string selectedRepresentationMethod {
            get => _selectedRepMethod;
            set {
                _selectedRepMethod = value;
                notifyPropertyDidChange(nameof(selectedRepresentationMethod));
            }
        }

        //
        // ### Basis Airfoil Method
        //
        public Visibility BasisAirfoilConfiguratorVisibility {
            get => _basisAirfoilConfiguratorVisibility;
            set {
                _basisAirfoilConfiguratorVisibility = value;
                notifyPropertyDidChange(nameof(BasisAirfoilConfiguratorVisibility));
            }
        }
        public ObservableCollection<BasisAirfoilItem> basisAirfoilItems => basisAirfoilMethodConfigManager.basisAirfoilItems;
        public int selectedBasisAirfoilItemIndex { 
            get => selectedBasisAirfoilItemIndex1;
            set {
                selectedBasisAirfoilItemIndex1 = value;
                removeButtonDidClick.notifyCanExecuteDidChange();
            }
        }

        public TriggerCommand addButtonDidClick { get; }
        private void addButtonExecute() {
            var path = OpenFileSelectorDialogMessenger.instance.showDialog();
            SeparatedValues csv = new SeparatedValues(',');

            Logger.getLogger(OptimizationManager.OptimizationLoggerName).debug("Open CSV File from \"" + path + "\"");

            try {
                // Open CSV
                csv.openFromFile(path);

                // Create Airfoil Instance
                var airfoil = new AirfoilShapeImporter(csv).getAirfoil();

                // Add new basis Airfoil item
                basisAirfoilItems.Add(new BasisAirfoilItem(airfoil));
            }
            catch (ArgumentNullException e) {
                Logger.getLogger(OptimizationManager.OptimizationLoggerName).error(e.Message);
            }
            catch (System.IO.PathTooLongException e) {
                Logger.getLogger(OptimizationManager.OptimizationLoggerName).error(e.Message);
            }
            catch (System.IO.DirectoryNotFoundException e) {
                Logger.getLogger(OptimizationManager.OptimizationLoggerName).error(e.Message);
            }
            catch (UnauthorizedAccessException e) {
                Logger.getLogger(OptimizationManager.OptimizationLoggerName).error(e.Message);
            }
            catch (System.IO.FileNotFoundException e) {
                Logger.getLogger(OptimizationManager.OptimizationLoggerName).error(e.Message);
            }
        }
        private bool addButtunCanExecute() {
            return true;
        }

        public TriggerCommand removeButtonDidClick { get; }
        private void removeButtonExecute() {
            basisAirfoilItems.Remove(basisAirfoilItems[selectedBasisAirfoilItemIndex]);
        }
        private bool removeButtonCenExecute() {
            return selectedBasisAirfoilItemIndex != -1;
        }

        #endregion

        #region Callbacks
        //
        // Callbacks ================================================================= //
        //

        public void thisPropertisDidChange(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(selectedRepresentationMethod)) {
                if (_selectedRepMethod == airfoilRepMethod[0]) {
                    BasisAirfoilConfiguratorVisibility = Visibility.Visible;
                } else {
                    BasisAirfoilConfiguratorVisibility = Visibility.Collapsed;
                }

                // Update Apply Button Status
                applyButtonDidClick.notifyCanExecuteDidChange();
            }
        }

        private void BasisAirfoilMethodConfigManager_readyStatusDidChange(object sender, EventArgs e) {
            applyButtonDidClick.notifyCanExecuteDidChange();
        }
        #endregion
    }
}
