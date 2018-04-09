using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using Ace_Advertising_Screen.Enumerators;
using Ace_Advertising_Screen.Content.Screen_Timer;

namespace Ace_Advertising_Screen.Content
{
    public class ContentTimerManager
    {
        #region Events
        public delegate void ScreenTimerComplete(Enumerators.Content content);
        public static event ScreenTimerComplete OnScreenTimerComplete;
        public static void TriggerOnScreenTimerComplete(Enumerators.Content content)
        {
            OnScreenTimerComplete?.Invoke(content);
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
        #region Variables
        private Thread thread;
        private Entity_Framework.AdContext context;
        #endregion
        #region Initialization
       
        /*
        private float GetTimerDuration(Enumerators.Content contentType)
        {
            float time = 4f;
            switch (contentType)
            {
                case Enumerators.Content.MAIN:
                    int index = ContentManager.GetManager().currentMainIndex;
                    String adUrl = ContentManager.GetManager().mainPanelContent[index];

                    break;
                case Enumerators.Content.SIDE_1:

                    break;
                case Enumerators.Content.SIDE_2:

                    break;
            }
        }
        */
       
        private void DebugPrintMainScreenDuration()
        {
            int index = ContentManager.GetManager().currentMainIndex;
            //String adUrl = ContentManager.GetManager().mainPanelContent[index];
            String adUrl = "cute-dog.jpg";

            Entity_Framework.Ad ad = null;

            foreach (Entity_Framework.Ad currentAd in context.Ads)
            {
                String currentAdUrl = currentAd.URL;
                if (adUrl == currentAdUrl)
                {
                    ad = currentAd;
                }
            }

            double? duration = ad.Duration;
            MessageBox.Show("Main Content Duration: " + duration);

        }

        public ContentTimerManager()
        {
            thread = new Thread(new ThreadStart(StartContent))
            {
                Name = "Content Timer Thread"
            };
            context = new Entity_Framework.AdContext();
            thread.Start();
        }
        
        public void StartContent()
        {
            StartNewTimer(Enumerators.Content.MAIN, 5f);
            //DebugPrintMainScreenDuration();
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
            Console.WriteLine("Switched content on screen: " + content);
        }
        #endregion
    }
}