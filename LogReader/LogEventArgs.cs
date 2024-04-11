using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wurmhelper.LogReader
{
    public class LogEventArgs
    {
        public string RawLine { get; set; }
        public string Message { get; set; }

        public LogEventArgs()
        {
            RawLine = String.Empty;
            Message = String.Empty;
        }
    }
}
