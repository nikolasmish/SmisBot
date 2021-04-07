using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SmisBot.Util
{
        public class TimerUtil
        {
            private Stopwatch stopWatch = new Stopwatch();

            public void Start()
            {
                stopWatch.Start();
            }

            public void Stop()
            {
                stopWatch.Stop();
            }

            public void Reset()
            {
                stopWatch.Reset();
            }

            public string getTime()
            {
                return stopWatch.Elapsed.ToString((@"%d\:hh\:mm\:ss"));
            }

            public string getMS()
            {
                return stopWatch.ElapsedMilliseconds.ToString();
            }
        }
    
}
