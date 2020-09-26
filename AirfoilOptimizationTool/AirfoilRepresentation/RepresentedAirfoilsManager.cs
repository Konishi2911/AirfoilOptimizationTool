using AirfoilOptimizationTool.AirfoilRepresentation.Method;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool.AirfoilRepresentation {
    public class RepresentedAirfoilsManager {
        private Method.IAirfoilRepresentationMethod representationMethod;

        private RepresentedAirfoil[] airfoils;

        public event EventHandler optimizationParametersDidChange;

        public double[][] optimizationParameters { 
            get {
                List<double[]> tempParams = new List<double[]>();
                foreach(var airfoil in airfoils) {
                    tempParams.Add(airfoil.optimizationParameters);
                }

                return tempParams.ToArray();
            }
        }

        public RepresentedAirfoilsManager(Method.IAirfoilRepresentationMethod method) {
            representationMethod = method;

            airfoils = new RepresentedAirfoil[0];
        }

        //
        // number of airfoils
        public int numberOfAirfoils => airfoils.Length;

        //
        // Set number of airfoils
        public void setNumberOfAirfoils(int value) {
            RepresentedAirfoil[] tempAirfoils = new RepresentedAirfoil[value];
            int iterator;
            for (iterator = 0; iterator < airfoils.Length && iterator < value; ++iterator) {
                tempAirfoils[iterator] = airfoils[iterator];
            }
            for (var i = iterator; i < value; ++i) {
                tempAirfoils[i] = new RepresentedAirfoil(representationMethod, null);
            }
            airfoils = tempAirfoils;

            optimizationParametersDidChange?.Invoke(this, EventArgs.Empty);
        }


        /// <summary>
        /// Edit Optimization Parameter
        /// </summary>
        /// <param name="index"></param>
        /// <param name="parameter"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public void editParamter(int index, double[] parameter) {
            if (parameter.Length != representationMethod.numberOfParameters()) {
                throw new ArgumentException("Invalid Size of Parameter", nameof(parameter));
            }
            try {
                airfoils[index] = new RepresentedAirfoil(representationMethod, parameter);
            }
            catch (IndexOutOfRangeException) {
                throw;
            }
        }

        //
        // Extract Airfoils
        public RepresentedAirfoil[] getRepresentedAirfoils() {
            return airfoils.ToArray();
        }
        public Airfoil.Airfoil[] getAllAirfoils() {
            List<Airfoil.Airfoil> tempAirfoils = new List<Airfoil.Airfoil>();
            foreach (var parameter in optimizationParameters) {
                tempAirfoils.Add(representationMethod.getAirfoil(parameter));
            }

            return tempAirfoils.ToArray();
        }
        public Airfoil.Airfoil getAirfoil(int index) {
            if (index >= optimizationParameters.Length) return null;

            return representationMethod.getAirfoil(optimizationParameters[index]);
        }

        //
        // Get Airfoil Representation Method
        public IAirfoilRepresentationMethod airfoilRepresentationMethod => representationMethod;
    }
}
