using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace_Advertising_Screen.Heartbeat
{
    class Heartbeat
    {
        private HeartbeatTimer heartbeatTimer;
        
        public void Init()
        {
            heartbeatTimer = new HeartbeatTimer();
            heartbeatTimer.OnHeartbeat += OnHeartbeat;
            heartbeatTimer.Start();
            Console.WriteLine("Heartbeat has started!");
        }

        private void OnHeartbeat()
        {
            Console.WriteLine("*--Heartbeat--*");
        }
    }
}