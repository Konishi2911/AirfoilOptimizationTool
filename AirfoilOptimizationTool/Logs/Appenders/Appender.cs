using AirfoilOptimizationTool.Logs.Formatter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool.Logs.Appenders {
    abstract class Appender : IAppender {
        public class LogReadyEventArgs : EventArgs {
            public string logMessage { get; private set; }

            public LogReadyEventArgs() : base() {

            }
            public LogReadyEventArgs(string message) : base() {
                logMessage = message;
            }
        }

        public delegate void LogReadyEventHandler(object sender, LogReadyEventArgs e);


        public Security logLevel { get; set; }
        public void appendLog(LogItem log) {
            if (log.security <= logLevel ) {
                appendLogImprementation(log);
            }
        }
        protected abstract void appendLogImprementation(LogItem log);

        public abstract void setFormat(IFormatter format);

        public abstract void setFormat(string format);
    }
}
