using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Ace_Advertising_Screen.Content.Screen_Timer
{
    public class ScreenTimerBase
    {
        #region Variables
        protected float duration = 0f;
        protected Thread thread;
        protected System.Timers.Timer timer;
        protected float currentTime = 0f;
        protected bool running = true;
        #endregion
        #region Functions
        public virtual void Init()
        {
            timer = new System.Timers.Timer(2000);
            timer.Elapsed += OnTimerEvent;
            timer.Start();
        }
        public virtual void OnTimerEvent(Object source, ElapsedEventArgs e)
        {
            currentTime++;
            if (currentTime >= duration)
            {
                timer.Stop();
                timer.Dispose();
                TimerComplete();
            }
        }
        public virtual void TimerComplete(){}
        #endregion
    }
}
