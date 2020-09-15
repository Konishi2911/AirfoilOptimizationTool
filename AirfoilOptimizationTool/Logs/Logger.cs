using AirfoilOptimizationTool.Logs.Appenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool.Logs {
    public class Logger {
        private string _loggerName;
        private Security _level;
        private List<IAppender> _appenders;

        private static Dictionary<string, Logger> instances;

        private Logger(string name) {
            _loggerName = name;
        }

        //
        // Retrieve a logger named according to the value of name parameter.
        // If the named logger already exist, then returns the instance. Otherwise create new instance than return it.
        public static Logger getLogger(string name) {
            if (instances == null) instances = new Dictionary<string, Logger>();

            if (!instances.ContainsKey(name)) {
                instances.Add(name, new Logger(name));
            }
            return instances[name];
        }

        //
        // Logger Configuration ======================== //
        //
        public void setLevel(Security level) {
            _level = level;
        }
        public void addAppender(IAppender appender) {
            if (_appenders == null) _appenders = new List<IAppender>();

            _appenders.Add(appender);
        }

        //
        // Logging Method ============================== //
        //
        private void log(Security level, string message) {
            if (_appenders == null) return;

            foreach (var appender in _appenders) {
                appender.appendLog(new LogItem(this._loggerName, level, message));
            }
        }
        public void trace(string message) {
            log(Security.Trace, message);
        }
        public void debug(string message) {
            log(Security.Debug, message);
        }
        public void info(string message) {
            log(Security.Info, message);
        }
        public void warn(string message) {
            log(Security.Warning, message);
        }
        public void error(string message) {
            log(Security.Error, message);
        }
        public void fatal(string message) {
            log(Security.Fatal, message);
        }
    }
}
