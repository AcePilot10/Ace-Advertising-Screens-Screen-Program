using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace_Advertising_Screen.Content.Screen_Timer
{
    public class ScreenMainTimer : ScreenTimerBase
    {
        public ScreenMainTimer(float duration)
        {
            base.duration = duration;
            Init();
        }

        public override void TimerComplete()
        {
            ContentTimerManager.TriggerOnScreenTimerComplete(Enumerators.Content.MAIN);
            base.TimerComplete();
        }
    }
}