using AirfoilOptimizationTool.Logs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool.MainWindow {
    public class ParametersTableGenerator {
        private DataTable parametersTable;

        public event EventHandler ParametersTableSourceDidUpdate;

        public ParametersTableGenerator() {
            OptimizationManager.instance.optimizationParamtersDidChange += Instance_optimizationParamtersDidChange;
            OptimizationManager.instance.representedAirfoilsMethodBeAvailable += Instance_representedAirfoilsMethodBeAvailable;
        }

        public DataTable getParameterTable() {
            var tempParamtersTable = new DataTable();

            // Generate columns
            var numberOfParameters = OptimizationManager.instance.representedAirfoilsManager?.airfoilRepresentationMethod?.numberOfParameters() ?? 0;
            for (var i = 0; i < numberOfParameters; ++i) {
                tempParamtersTable.Columns.Add("Column " + i.ToString());
            }

            // Generate rows
            var numberOfAirfoils = OptimizationManager.instance.representedAirfoilsManager?.numberOfAirfoils ?? 0;
            for (var i = 0; i < numberOfAirfoils; ++i) {
                var tmpRow = tempParamtersTable.NewRow();
                for (var j = 0; j < numberOfParameters; ++j) {

                    // Get Represented Airfoil
                    var representedAirfoils = OptimizationManager.instance.representedAirfoilsManager?.getRepresentedAirfoils();

                    // Set Values into Table
                    tmpRow[j] = representedAirfoils?[i]?.optimizationParameters[j] ?? 0;
                }
                tempParamtersTable.Rows.Add(tmpRow);
            }

            // Regist event handler that detect column change
            tempParamtersTable.RowChanged += TempParamtersTable_RowChanged;

            parametersTable = tempParamtersTable;
            return tempParamtersTable;
        }

        private void TempParamtersTable_RowChanged(object sender, DataRowChangeEventArgs e) {
            var changedRowItems = e.Row.ItemArray;
            var changedValuesList = new List<double>();

            // make changed values list
            try {
                foreach (var item in changedRowItems) {
                    changedValuesList.Add(Convert.ToDouble(item));
                }
            }
            catch (FormatException ex) {
                Logger.getLogger(OptimizationManager.OptimizationLoggerName).warn(ex.Message);
            }
            catch (InvalidCastException ex) {
                Logger.getLogger(OptimizationManager.OptimizationLoggerName).warn(ex.Message);
            }
            catch (OverflowException ex) {
                Logger.getLogger(OptimizationManager.OptimizationLoggerName).warn(ex.Message);
            }

            // Get airfoil manager
            var airfoilManager = OptimizationManager.instance.representedAirfoilsManager;

            // get changed Index
            var changedIndex = parametersTable.Rows.IndexOf(e.Row);

            // edit airfoil
            try {
                airfoilManager.editParamter(changedIndex, changedValuesList.ToArray());
            }
            catch (ArgumentException ex) {
                Logger.getLogger(OptimizationManager.OptimizationLoggerName).warn(ex.Message);
            }
            catch (IndexOutOfRangeException ex) {
                Logger.getLogger(OptimizationManager.OptimizationLoggerName).warn(ex.Message);
            }
        }

        private void Instance_optimizationParamtersDidChange(object sender, EventArgs e) {
            ParametersTableSourceDidUpdate?.Invoke(sender, e);
        }
        private void Instance_representedAirfoilsMethodBeAvailable(object sender, EventArgs e) {
            ParametersTableSourceDidUpdate?.Invoke(sender, e);
        }
    }
}
