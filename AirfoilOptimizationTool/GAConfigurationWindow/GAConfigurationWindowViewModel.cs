using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool.GAConfigurationWindow {
    class GAConfigurationWindowViewModel {
        private static readonly string[] airfoilRepMethod = { 
            "Base Airfoil Method" ,
            "B-Spline Method",
            "PARSEC Method"
        };

        #region Binding Properties
        //
        // Airfoil Representation Method
        public string[] airfoilRepresentationMethod => airfoilRepMethod;
        #endregion



    }
}
