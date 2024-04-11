/*  Copyright (c) 2024, Janne Papula
    All rights reserved.

    This source code is licensed under the BSD-style license found in the
    LICENSE file in the root directory of this source tree. 
*/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wurmhelper.LogReader
{
    public class LogReader(string path, string skill)
    {
        public event EventHandler<LogEventArgs>? LogEventOccurred;
        private CancellationTokenSource? cts;
        private Task? parseTask;
        private readonly string _path = path;
        private readonly string _skill = skill;

        public bool IsRunning
        {
            get
            {
                if (parseTask == null)
                {
                    return false;
                }
                return parseTask.Status == TaskStatus.Running;
            }
        }

        public void Start()
        {
            DateTime curTime = DateTime.ParseExact("00:00:00", "HH:mm:ss", CultureInfo.InvariantCulture);

            cts = new CancellationTokenSource();
            parseTask = Task.Run(() =>
            {
                FileStream? fs = new(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader? sr = new(fs);

                fs.Seek(0, SeekOrigin.End);

                while (!cts.IsCancellationRequested)
                {
                    if (fs != null && sr != null)
                    {
                        string? line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.Contains(_skill))
                            {
                                string skill = line[(line.IndexOf("to") + 3)..];
                                double skillValue = double.Parse(skill, CultureInfo.InvariantCulture);
                                double sweetSpot = (skillValue * 0.77) + 23.0;
                                double increment = double.Parse(line[(line.IndexOf("by") + 3)..line.IndexOf("to")], CultureInfo.InvariantCulture);

                                LogEventArgs e = new()
                                {
                                    RawLine = line,
                                    Message = line + " Sweetspot: " + sweetSpot.ToString("F4"),
                                    SkillValue = skillValue,
                                    SkillIncrement = increment
                                };
                                OnLogEventOccurred(e);
                            }
                        }
                    }

                    Thread.Sleep(200);
                }
            }, cts.Token);
        }

        public void Stop()
        {
            if (cts != null)
            {
                cts.Cancel();
                try
                {
                    parseTask?.Wait();
                }
                catch (AggregateException ex)
                {
                    foreach (var innerEx in ex.InnerExceptions)
                    {
                        Console.WriteLine(innerEx.Message);
                    }
                }
            }
        }

        protected virtual void OnLogEventOccurred(LogEventArgs e)
        {
            LogEventOccurred?.Invoke(this, e);
        }
    }
}
