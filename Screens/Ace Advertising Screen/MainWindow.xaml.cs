using Ace_Advertising_Screen.Content;
using Ace_Advertising_Screen.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Ace_Advertising_Screen
{
    public partial class MainWindow : Window
    {
        #region Variables
        public static string VENUE_NAME = "Test Venue";
        #endregion
        #region Singleton
        public static MainWindow instance;
        #endregion
        #region Initialization
        public MainWindow()
        {
            InitializeComponent();
            instance = this;
            ContentManager.GetManager();
            ContentManager.GetManager().InitContent();
            ShowScreen(Screen.CONTENT);
        }
        #endregion
        #region Funcionality
        #region Registration
        public void Register()
        {
            String venueName = txtVenueName.Text;
            if (venueName.Length > 0)
            {
                InitContent();
                ShowScreen(Screen.CONTENT);
            }
            else
            {
                MessageBox.Show("Please enter a valid venue name!");
            }
        }
        #endregion
        #region Content
        public void SetContent(Enumerators.Content content, Uri src)
        {
            switch (content)
            {
                case Enumerators.Content.MAIN:
                    mediaMain.Source = null;
                    mediaMain.Source = src;
                    break;
                case Enumerators.Content.SIDE_1:
                    mediaSide1.Source = null;
                    mediaSide1.Source = src;
                    break;
                case Enumerators.Content.SIDE_2:
                    mediaSide2.Source = null;
                    mediaSide2.Source = src;
                    break;
            }
        }
        #endregion
        #endregion
        #region Screen Management
        public void ShowScreen(Screen screen) {
            CloseScreens();
            switch (screen)
            {
                case Screen.CONTENT:
                    screenContent.Visibility = Visibility.Visible;
                    break;
                case Screen.REGISTER:
                    screenRegister.Visibility = Visibility.Visible;
                    break;
            }
        }
        public void CloseScreens()
        {
            screenContent.Visibility = Visibility.Hidden;
            screenRegister.Visibility = Visibility.Hidden;
        }
        #endregion
        #region Listeners
        private void BtnRegisterScreen_Click(object sender, RoutedEventArgs e)
        {
            Register();
        }
        #endregion
    }
}