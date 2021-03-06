﻿using Ace_Advertising_Screen.Content;
using Ace_Advertising_Screen.Enumerators;
using System;
using System.Configuration;
using System.Reflection;
using System.Resources;
using System.Windows;

namespace Ace_Advertising_Screen
{
    public partial class MainWindow : Window
    {
        #region Singleton
        public static MainWindow instance;
        #endregion
        #region Initialization
        public MainWindow()
        {
            InitializeComponent();
            instance = this;
            CheckRegistration();
        }
        private void InitContent()
        {
            ContentManager.GetManager();
            ContentManager.GetManager().InitContent();
            ShowScreen(Screen.CONTENT);
        }
        #endregion
        #region Funcionality
        #region Registration
        private void CheckRegistration()
        {
            String venue = Properties.Settings.Default.Venue;
            if (venue.Length > 0)
            {
                InitContent();
            }
            else
            {
                MessageBox.Show("Welcome to Ace Advertising Screens! To continue, please register this client with a venue name");
                ShowScreen(Screen.REGISTER);
            }
        }
        public void Register()
        {
            String venueName = txtVenueName.Text;
            if (venueName.Length > 0)
            {
                Properties.Settings.Default.Properties["Venue"].DefaultValue = venueName;
                Properties.Settings.Default.Venue = venueName;
                Properties.Settings.Default.Save();
                MessageBox.Show("Succesfully Registered: " + Properties.Settings.Default.Properties["Venue"].DefaultValue);
                InitContent();
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
        public void ShowScreen(Screen screen)
        {
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