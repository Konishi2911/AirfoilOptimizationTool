using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool.Logs {
    public struct LogItem {
        public string loggerName { get; }
        public Security security { get; }
        public string logMessage { get; }

        public LogItem(string loggerName, Security sec, string message) {
            this.loggerName = loggerName;
            security = sec;
            logMessage = message;
        }
    }
}
