using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace CaptureBody
{
    class Timer
    {
        int incrementSeconds;
        int incrementMinuts;
        string timerVar;

        public Timer()
        {
            ResetTimer();
        }

        public void StartTimer()
        {
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += dtTTicker;
            dt.Start();
        }
        public void ResetTimer()
        {
            timerVar = "00:00";
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

            timerVar = timerMin.ToString() + ":" + timerSec.ToString();
        }

        public string TimerVar
        {
            get => this.timerVar;
            set => this.timerVar = value;
        }
    }
}
