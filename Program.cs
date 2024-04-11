/*  Copyright (c) 2024, Janne Papula
    All rights reserved.

    This source code is licensed under the BSD-style license found in the
    LICENSE file in the root directory of this source tree. 
*/
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
using wurmhelper.LogReader;

if (args.Length < 2)
{
    Console.WriteLine("Usage: WurmHelper <path> <skill>");
    return 1;
}
var path = args[0];
var skill = args[1];

var lr = new LogReader(path, skill);
lr.LogEventOccurred += lr_LogEventOccurred;
lr.Start();

while (!Console.KeyAvailable)
{
    // Well, eh... just wait.. add more statistics logging here
    System.Threading.Thread.Sleep(1000);
}
lr.Stop();
return 0;

void lr_LogEventOccurred(object? sender, LogEventArgs e)
{
    Console.WriteLine(e.Message);
}
