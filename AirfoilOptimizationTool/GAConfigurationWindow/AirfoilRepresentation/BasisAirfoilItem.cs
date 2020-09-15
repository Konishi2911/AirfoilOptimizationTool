using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AirfoilOptimizationTool.GAConfigurationWindow.AirfoilRepresentation {
    public class BasisAirfoilItem {

        public BasisAirfoilItem() {

        }
        public BasisAirfoilItem(string name, PointCollection curve) {
            airfoilName = name;
            airfoilCurve = curve;
        }

        #region Binding Propreties
        //
        // Binding Prorpertis =============================================== //
        //
        public string airfoilName { get; set; }
        public PointCollection airfoilCurve { get; set; }
        #endregion
    }
}
