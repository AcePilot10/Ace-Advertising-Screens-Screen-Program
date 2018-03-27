using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using Ace_Advertising_Screen.Enumerators;

namespace Ace_Advertising_Screen.Content
{
    public class ContentTimerManager
    {
        #region Events
        public delegate void ScreenTimerComplete(Enumerators.Content content);
        public static event ScreenTimerComplete OnScreenTimerComplete;
        #endregion
        #region Timer Classes
        public class ScreenTimerBase
        {
            #region Variables
            protected float duration = 0f;
            protected Thread thread;
            private System.Timers.Timer timer;
            protected float currentTime = 0f;
            #endregion
            #region Functions
            private void ThreadStart()
            {
                timer = new System.Timers.Timer(1000);
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
            public virtual void TimerComplete() {}
            public virtual void Init()
            {
                thread = new Thread(new ThreadStart(ThreadStart));
                thread.Start();
            }
            #endregion
        }

        public class ScreenMainTimer : ScreenTimerBase
        {
            public ScreenMainTimer(float duration)
            {
                base.duration = duration;
                Init();
            }
            public override void TimerComplete()
            {
                OnScreenTimerComplete?.Invoke(Enumerators.Content.MAIN);
            }
        }

        public class ScreenSide1Timer : ScreenTimerBase
        {
            public ScreenSide1Timer(float duration)
            {
                base.duration = duration;
                Init();
            }
            public override void TimerComplete()
            {
                OnScreenTimerComplete?.Invoke(Enumerators.Content.SIDE_1);
            }
        }

        public class ScreenSide2Timer : ScreenTimerBase
        {
            public ScreenSide2Timer(float duration)
            {
                base.duration = duration;
                Init();
            }
            public override void TimerComplete()
            {
                OnScreenTimerComplete?.Invoke(Enumerators.Content.SIDE_2);
            }
        }
        #endregion
        #region Singleton
        private static ContentTimerManager instance;
        public static ContentTimerManager GetInstance()
        {
            if (instance == null)
            {
                instance = new ContentTimerManager();
            }
            return instance;
        }
        #endregion
        #region Initialization
        public ContentTimerManager()
        {
            StartNewTimer(Enumerators.Content.MAIN, 5f);
            StartNewTimer(Enumerators.Content.SIDE_1, 5f);
            StartNewTimer(Enumerators.Content.SIDE_2, 5f);
        }
        public void StartNewTimer(Enumerators.Content content, float duration)
        {
            switch (content)
            {
                case Enumerators.Content.MAIN:
                    new ScreenMainTimer(duration);
                    break;
                case Enumerators.Content.SIDE_1:
                    new ScreenSide1Timer(duration);
                    break;
                case Enumerators.Content.SIDE_2:
                    new ScreenSide2Timer(duration);
                    break;
            }
        }
        #endregion
    }
}