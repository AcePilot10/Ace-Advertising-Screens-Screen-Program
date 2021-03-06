﻿using Ace_Advertising_Screen.Entity_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Google.Cloud.Storage.V1;
using System.IO;
using System.Windows;
using Google.Apis.Auth.OAuth2;
using System.Diagnostics;

namespace Ace_Advertising_Screen.Content
{
    class ContentManager
    {
        #region Singleton
        private static ContentManager instance;
        public static ContentManager GetManager()
        {
            if (instance == null)
            {
                instance = new ContentManager();
            }
            return instance;
        }
        #endregion
        #region Fields
        private AdContext context;
        const int CONTENT_MAIN = 0;
        const int CONTENT_SIDE_1 = 1;
        const int CONTENT_SIDE_2 = 2;
        const string BUCKET_NAME = "ace_advertising_screens-content";
        const string FILE_NAME_MAIN = "/mainContentImage.jpg";
        const string FILE_NAME_SIDE_1 = "/side1Image.jpg";
        const string FILE_NAME_SIDE_2 = "/side2Image.jpg";
        private string baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
        #region Lists
        public List<int> sidePanel1Content;
        public List<int> sidePanel2Content;
        public List<int> mainPanelContent;
        #endregion
        #region Current Indexes
        public int currentMainIndex = 0;
        public int currentSide1Index = 0;
        public int currentSide2Index = 0;
        #endregion
        #endregion
        #region Initialization
        public ContentManager()
        {
            context = new AdContext();
            sidePanel1Content = new List<int>();
            sidePanel2Content = new List<int>();
            mainPanelContent = new List<int>();
            ContentTimerManager.OnScreenTimerComplete += OnContentTimerComplete;
            LoadContent();
        }
        private void LoadContent()
        {
            try
            {
                foreach (Ad ad in context.Ads)
                {
                    String venue = ad.Venue;
                    if (venue == Properties.Settings.Default.Venue)
                    {
                        int id = ad.ID;
                        int content = int.Parse(ad.Content_Type);
                        switch (content)
                        {
                            case CONTENT_MAIN:
                                mainPanelContent.Add(id);
                                break;
                            case CONTENT_SIDE_1:
                                sidePanel1Content.Add(id);
                                break;
                            case CONTENT_SIDE_2:
                                sidePanel2Content.Add(id);
                                break;
                        }
                    }
                }
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                MessageBox.Show("There was an error accessing the server: " + ex.Message + ". Please fix the issue and restart.");
                MainWindow.instance.Close();
                Application.Current.Shutdown();
                Process.GetCurrentProcess().Kill();
            }
        }
        public void InitContent()
        {
            ContentTimerManager.GetInstance();
            SetNextContent(Enumerators.Content.MAIN);
            SetNextContent(Enumerators.Content.SIDE_1);
            SetNextContent(Enumerators.Content.SIDE_2);
        }
        #endregion
        #region Functions
        #region Downloading Content
        private Uri DownloadContent(string url)
        {
            string location = baseDirectory + url;
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(url, location);
            }
            return new Uri(location);
        }
        private Uri DownloadContentFromBucket(string name)
        {
            string bucketName = "ace_advertising_screens-content";
            StorageClient client = StorageClient.Create();
            FileStream destination = new FileStream(baseDirectory + "/mainContentImage.jpg", FileMode.OpenOrCreate);
            client.DownloadObject(bucketName, name, destination);
            destination.Close();
            return new Uri(baseDirectory + "/mainContentImage.jpg");
        }
        private void DownloadAndSetContent(Enumerators.Content content, String fileName)
        {
            Uri fileLocation = new Uri(baseDirectory + "/StorageProjectKey.json");
            GoogleCredential credential = GoogleCredential.FromFile(fileLocation.LocalPath);
            StorageClient client = StorageClient.Create(credential);
            FileStream destination = null;
            Uri uri = null;
            switch (content)
            {
                case Enumerators.Content.MAIN:
                    destination = new FileStream(baseDirectory + FILE_NAME_MAIN, FileMode.OpenOrCreate);
                    uri = new Uri(baseDirectory + FILE_NAME_MAIN);
                    break;
                case Enumerators.Content.SIDE_1:
                    destination = new FileStream(baseDirectory + FILE_NAME_SIDE_1, FileMode.OpenOrCreate);
                    uri = new Uri(baseDirectory + FILE_NAME_SIDE_1);
                    break;
                case Enumerators.Content.SIDE_2:
                    destination = new FileStream(baseDirectory + FILE_NAME_SIDE_2, FileMode.OpenOrCreate);
                    uri = new Uri(baseDirectory + FILE_NAME_SIDE_2);
                    break;
            }
            client.DownloadObject(BUCKET_NAME, fileName, destination);
            destination.Close();
            MainWindow.instance.SetContent(content, uri);
        }
        private string GetUrl(int id)
        {
            foreach (Ad ad in context.Ads)
            {
                if (ad.ID == id)
                {
                    return ad.URL;
                }
            }
            return null;
        }
        public void SetNextContent(Enumerators.Content content)
        {
            switch (content)
            {
                case Enumerators.Content.MAIN:
                    if (currentMainIndex <= mainPanelContent.Count - 1)
                    {
                        string fileName = GetUrl(mainPanelContent[currentMainIndex]);
                        DownloadAndSetContent(content, fileName);
                        currentMainIndex++;
                    }
                    else
                    {
                        currentMainIndex = 0;
                        SetNextContent(content);
                    }
                    break;
                case Enumerators.Content.SIDE_1:
                    if (currentSide1Index <= sidePanel1Content.Count - 1)
                    {
                        String fileName = GetUrl(sidePanel1Content[currentSide1Index]);
                        DownloadAndSetContent(content, fileName);
                        currentSide1Index++;
                    }
                    else
                    {
                        currentSide1Index = 0;
                        SetNextContent(content);
                    }
                    break;
                case Enumerators.Content.SIDE_2:
                    if (currentSide2Index <= sidePanel2Content.Count - 1)
                    {
                        String fileName = GetUrl(sidePanel2Content[currentSide2Index]);
                        DownloadAndSetContent(content, fileName);
                        currentSide2Index++;
                    }
                    else
                    {
                        currentSide2Index = 0;
                        SetNextContent(content);
                    }
                    break;
            }
        }
        #endregion
        public double? GetDuration(int adId)
        {
            foreach (Ad ad in context.Ads)
            {
                string venue = ad.Venue;
                int id = ad.ID;
                if (venue == Properties.Settings.Default.Venue)
                {
                    if (id == adId)
                    {
                        return ad.Duration;
                    }
                }
            }
            return 5;
        }
        public void ResetContent()
        {
            //1: Clear arrays
            //2: Repopulate arrays
            //3: Restart
            mainPanelContent.Clear();
            sidePanel1Content.Clear();
            sidePanel2Content.Clear();
            LoadContent();
            InitContent();
        }
        public void OnContentTimerComplete(Enumerators.Content content)
        {
            Application.Current.Dispatcher.Invoke(delegate ()
            {
                SwitchContent(content);
            });
        }
        private void SwitchContent(Enumerators.Content content)
        {
            switch (content)
            {
                case Enumerators.Content.MAIN:
                    SetNextContent(Enumerators.Content.MAIN);
                    int idMain = mainPanelContent[currentMainIndex-1];
                    double? durationMain = GetDuration(idMain);
                    ContentTimerManager.GetInstance().StartNewTimer(Enumerators.Content.MAIN, (float)durationMain);
                    break;
                case Enumerators.Content.SIDE_1:
                    SetNextContent(Enumerators.Content.SIDE_1);
                    int idSide1 = sidePanel1Content[currentSide1Index-1];
                    double? durationSide1 = GetDuration(idSide1);
                    ContentTimerManager.GetInstance().StartNewTimer(Enumerators.Content.SIDE_1, (float)durationSide1);
                    break;
                case Enumerators.Content.SIDE_2:
                    SetNextContent(Enumerators.Content.SIDE_2);
                    int idSide2 = sidePanel2Content[currentSide2Index-1];
                    double? durationSide2 = GetDuration(idSide2);
                    ContentTimerManager.GetInstance().StartNewTimer(Enumerators.Content.SIDE_2, (float)durationSide2);
                    break;
            }
        }
        #endregion
    }
}