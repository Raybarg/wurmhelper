/*  Copyright (c) 2024, Janne Papula
    All rights reserved.

    This source code is licensed under the BSD-style license found in the
    LICENSE file in the root directory of this source tree. 
*/
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
        public double SkillValue { get; set; }
        public double SkillIncrement { get; set; }

        public LogEventArgs()
        {
            RawLine = String.Empty;
            Message = String.Empty;
        }
    }
}
