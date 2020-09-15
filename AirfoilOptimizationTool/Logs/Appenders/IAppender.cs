using AirfoilOptimizationTool.Logs.Formatter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool.Logs.Appenders {
    public interface IAppender {

        // 
        // Configuration ================================= //
        void setFormat(IFormatter format);
        void setFormat(string format);

        //
        // Logging ======================================= //
        void appendLog(LogItem log);
    }
}
