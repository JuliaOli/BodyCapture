using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Diagnostics;
using System.Timers;
namespace CaptureBody
{
    class TimerCounter
    {
        private Timer aTimer = new Timer();
        int incrementSeconds;
        int incrementMinuts;
        string timerVar;

        public TimerCounter()
        {
            aTimer.Elapsed += dtTTicker;
        }

        public void StartTimer()
        {
            aTimer.Interval = 1000;
            aTimer.Start();
        }

        public void ResetTimer()
        {
            timerVar = "Timer: 00:00";
            incrementMinuts = 0;
            incrementSeconds = 0;
        }

        private void dtTTicker(object sender, EventArgs e)
        {
            ++incrementSeconds;
            string timerSec = incrementSeconds.ToString();
            string timerMin = incrementMinuts.ToString();

            if (incrementSeconds == 60)
            {
                incrementSeconds = 0;
                ++incrementMinuts;
            }
            if (incrementMinuts < 10)
            {
                timerMin = '0' + incrementMinuts.ToString();
            }
            if (incrementSeconds < 10)
            {
                timerSec = '0' + incrementSeconds.ToString();
            }

            timerVar = "Timer: " + timerMin.ToString() + ":" + timerSec.ToString();
            
        }

        public string TimerVar
        {
            get => this.timerVar;
            //set => this.timerVar = value;
        }
        public int IncrementSeconds
        {
            get => this.incrementSeconds;
        }
        public int IncrementMinuts
        {
            get => this.incrementMinuts;
        }
    }
}
