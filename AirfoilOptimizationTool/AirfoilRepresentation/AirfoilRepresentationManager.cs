﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool.AirfoilRepresentation {
    class AirfoilRepresentationManager {
        private Method.IAirfoilRepresentationMethod representationMethod;

        private List<double[]> optimizingParameters;

        AirfoilRepresentationManager(Method.IAirfoilRepresentationMethod method) {
            representationMethod = method;
        }

        public void addParameter(double[] parameter) {
            optimizingParameters.Add(parameter);
        }

        //
        // Extract Airfoils
        public Airfoil.Airfoil[] getAllAirfoils() {
            List<Airfoil.Airfoil> tempAirfoils = new List<Airfoil.Airfoil>();
            foreach (var parameter in optimizingParameters) {
                tempAirfoils.Add(representationMethod.getAirfoil(parameter));
            }

            return tempAirfoils.ToArray();
        }
        public Airfoil.Airfoil getAirfoil(int index) {
            if (index >= optimizingParameters.Count) return null;

            return representationMethod.getAirfoil(optimizingParameters[index]);
        }
    }
}