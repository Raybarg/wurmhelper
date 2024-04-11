/*  Copyright (c) 2024, Janne Papula
    All rights reserved.

    This source code is licensed under the BSD-style license found in the
    LICENSE file in the root directory of this source tree. 
*/
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
using wurmhelper.LogReader;

ConsoleColor below = ConsoleColor.DarkRed;
ConsoleColor above = ConsoleColor.Green;
int IncrementCount = 0;
double avgIncrement = 0.0;

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
    avgIncrement = avgIncrement * IncrementCount;
    IncrementCount++;
    avgIncrement += e.SkillIncrement;
    avgIncrement = avgIncrement / IncrementCount;
    if (e.SkillIncrement < avgIncrement)
    {
        Console.ForegroundColor = below;
    }
    else
    {
        Console.ForegroundColor = above;
    }
    Console.WriteLine(e.Message + " ["+ avgIncrement.ToString()+"]");
}
