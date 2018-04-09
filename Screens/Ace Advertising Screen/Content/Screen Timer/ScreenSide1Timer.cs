using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace_Advertising_Screen.Content.Screen_Timer
{

    public class ScreenSide1Timer : ScreenTimerBase
    {
        public ScreenSide1Timer(float duration)
        {
            base.duration = duration;
            Init();
        }

        public override void TimerComplete()
        {
            ContentTimerManager.TriggerOnScreenTimerComplete(Enumerators.Content.SIDE_1);
            base.TimerComplete();
        }
    }
}