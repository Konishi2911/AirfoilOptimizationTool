using AirfoilOptimizationTool.Logs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Converters;

namespace AirfoilOptimizationTool.DataIO {
    class SeparatedValues {
        private string name;
        private char delimiter;
        private string[][] value;

        public string valueName => name;

        public SeparatedValues(char delimiter) {
            this.delimiter = delimiter;
        }

        /// <summary>
        /// Open a Separated Value file from path and than close the file.
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public void openFromFile(string path) {
            try {
                name = Path.GetFileNameWithoutExtension(path);
                var lineText = File.ReadAllText(path).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                List<string[]> tempValue = new List<string[]>();
                foreach (var text in lineText) {
                    tempValue.Add(text.Split(delimiter));
                }
                value = tempValue.ToArray();

            } catch (ArgumentNullException) {
                throw;
            } catch (PathTooLongException) {
                throw;              
            } catch (DirectoryNotFoundException) {
                throw;
            } catch (UnauthorizedAccessException) {
                throw;
            } catch (FileNotFoundException) {
                throw;
            }
        }

        /// <summary>
        /// Get separated Value as double type array.
        /// The Elements that are invalid format of number will be MaxValue.
        /// </summary>
        /// <returns></returns>
        public int[][] getIntValue() {
            List<int[]> intValue = new List<int[]>();
            foreach(var val in value) {

                List<int> temp = new List<int>();
                foreach(var v in val) {
                    try {
                        temp.Add(System.Convert.ToInt32(v));
                    }
                    catch (FormatException e) {
                        temp.Add(Int32.MaxValue);
                    }
                }

                intValue.Add(temp.ToArray());
            }

            return intValue.ToArray();
        }

        /// <summary>
        /// Get separated Value as double type array.
        /// The Elements that are invalid format of number will be NAN.
        /// </summary>
        /// <returns></returns>
        public double[][] getDoubleValue() {
            List<double[]> intValue = new List<double[]>();
            foreach (var val in value) {

                List<double> temp = new List<double>();
                foreach (var v in val) {
                    try {
                        temp.Add(System.Convert.ToDouble(v));
                    } catch (FormatException e) {
                        temp.Add(Double.NaN);
                    }
                }

                intValue.Add(temp.ToArray());
            }

            return intValue.ToArray();
        }
    }
}
