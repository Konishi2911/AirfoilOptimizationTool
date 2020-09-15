using AirfoilOptimizationTool.Logs.Formatter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool.Logs.Appenders {
    class TextFieldAppender : Appender {
        private IFormatter formatter;

        public string logString { get; private set; }

        public event LogReadyEventHandler ReadyToLog;

        public override void appendLog(LogItem log) {
            logString = formatter?.formatLog(log) ?? null;

            // Fire Event
            if (logString != null) {
                ReadyToLog(this, new LogReadyEventArgs(logString));
            }
        }

        public override void setFormat(IFormatter format) {
            formatter = format;
        }

        public override void setFormat(string format) {
            formatter = new Formatter.Formatter(format);
        }
    }
}
