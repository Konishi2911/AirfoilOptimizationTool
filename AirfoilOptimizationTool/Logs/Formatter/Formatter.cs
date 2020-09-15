using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool.Logs.Formatter {
    class Formatter : IFormatter {
        private string _format;
        private string _dateFormat;

        public Formatter(string format, string dateFormat = "yyyy/MM/dd HH:mm:ss K") {
            _format = format;
            _dateFormat = dateFormat;
        }

        public string formatLog(LogItem log) {
            string formattedLog = "";

            bool isEscape = false;
            foreach (var c in _format) {
                if (isEscape) {
                    if (c == 'p') formattedLog += log.security.ToString();
                    else if (c == 'd') formattedLog += DateTime.Now.ToString(_dateFormat);
                    else if (c == 'm') formattedLog += log.logMessage;
                    else if (c == '%') formattedLog += '%';

                    isEscape = false;
                } else {
                    if (c == '%') {
                        isEscape = true;
                    } else {
                        formattedLog += c;
                    }
                }
            }
            return formattedLog;
        }
    }
}
