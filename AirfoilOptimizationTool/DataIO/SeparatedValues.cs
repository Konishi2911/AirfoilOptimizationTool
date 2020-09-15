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
        private char delimiter;
        private string[][] value;

        private Logger logger;

        public SeparatedValues(char delimiter) {
            this.delimiter = delimiter;

            logger = Logger.getLogger(nameof(SeparatedValues));
        }

        public bool openFromFile(string path) {
            try {
                var lineText = File.ReadAllText(path).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                List<string[]> tempValue = new List<string[]>();
                foreach (var text in lineText) {
                    tempValue.Add(text.Split(delimiter));
                }
                value = tempValue.ToArray();

                return true;

            } catch (ArgumentNullException e) {
                logger.error(e.Message);
            } catch (PathTooLongException e) {
                logger.error(e.Message);
            } catch (DirectoryNotFoundException e) {
                logger.error(e.Message);
            } catch (UnauthorizedAccessException e) {
                logger.error(e.Message);
            } catch (FileNotFoundException e) {
                logger.error(e.Message);
            }

            return false;
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
                        logger.error(e.Message);
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
                        logger.error(e.Message);
                    }
                }

                intValue.Add(temp.ToArray());
            }

            return intValue.ToArray();
        }
    }
}
