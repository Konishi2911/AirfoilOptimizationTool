using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool.Logs.Formatter {

    //
    // %l       LoggerName
    // %p       Logging priority
    // %d       Date                Note that format is complaint to DateTime structure.
    // %m       Log message
    // %%       write '%'
    //
    public interface IFormatter {
        string formatLog(LogItem log);
    }
}
