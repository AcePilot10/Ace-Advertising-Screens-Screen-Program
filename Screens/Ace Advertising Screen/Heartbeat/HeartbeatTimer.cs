using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Ace_Advertising_Screen.Heartbeat
{
    class HeartbeatTimer
    {
        #region Fields
        private Timer timer;
        private int duration;
        private int currentTime = 0;
        public delegate void Heartbeat();
        public event Heartbeat OnHeartbeat;
        #endregion
        #region Functions
        public HeartbeatTimer()
        {
            duration = Properties.Settings.Default.HeartbeatInterval;
            timer = new Timer
            {
                Interval = 2000
            };
            timer.Elapsed += TimerElapsed;
        }
        public void Start()
        {
            currentTime = 0;
            timer.Start();
        }
        public void TimerElapsed(Object source, ElapsedEventArgs e)
        {
            currentTime++;
            if (currentTime == duration)
            {
                OnHeartbeat?.Invoke();
                Start();
            }
        }
        #endregion
    }
}