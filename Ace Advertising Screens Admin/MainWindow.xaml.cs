using Ace_Advertising_Screens_Admin.Entity_Framework;
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

namespace Ace_Advertising_Screens_Admin
{
    public partial class MainWindow : Window
    {

        #region References
        private AdDatabase context;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            context = new AdDatabase();
            InitVenueView();
            InitComboCreateVenues();
        }

        #region Venue View
        #region Initialization Functions
        private void InitVenueView()
        {
            RegisterListeners();
            PopulateVenueList();
        }
        public void PopulateVenueList()
        {
            foreach (Ad ad in context.Ads)
            {
                string venue = ad.Venue;
                if (!ComboSelect.Items.Contains(venue))
                {
                    ComboSelect.Items.Add(venue);
                }
            }
        }
        #endregion
        #region List Functions
        public void PopulateAdList()
        {
            foreach (Ad ad in context.Ads)
            {
                string selectedVenue = GetSelectedVenue();
                string currentVenue = ad.Venue;
                if (selectedVenue == currentVenue)
                {
                    double? id = ad.ID;
                    string url = ad.URL;
                    string company = ad.Company;
                    string contentType = ad.Content_Type;
                    string itemString = id + " | " + company + " | " +
                                        contentType + " | " + url;
                    ListAds.Items.Add(itemString);
                }
            }
        }
        #endregion
        #region Helpers
        private string GetSelectedVenue()
        {
            return ComboSelect.SelectedItem.ToString();
        }
        #endregion
        #region Listeners
        public void OnComboVenueSelect(object sender, SelectionChangedEventArgs args)
        {
            PopulateAdList();
        }
        #endregion
        #endregion

        #region Create Ad
        #region Initialization

        private void InitComboCreateVenues()
        {
            LoopAds(delegate(Ad ad) {
                string venue = ad.Venue;
                if (!ComboCreateVenue.Items.Contains(venue))
                {
                    ComboCreateVenue.Items.Add(venue);
                }
            });
            ComboCreateContentType.Items.Add("Main");
            ComboCreateContentType.Items.Add("Side 1");
            ComboCreateContentType.Items.Add("Side 2");
        }

        #endregion

        #region Listeners
        public void OnCreateAdClick(object sender, EventArgs args)
        {
            string type = ComboCreateContentType.SelectedIndex.ToString();
            string venue = ComboCreateVenue.Items[ComboCreateVenue.SelectedIndex].ToString();
            string company = TxtCreateCompany.Text;
            string url = TxtCreateUrl.Text;
            double duration = Double.Parse(TxtCreateDuration.Text);
            Ad ad = new Ad()
            {
                Venue = venue,
                Company = company,
                URL = url,
                Content_Type = type,
                Duration = duration
            };
            context.Ads.Add(ad);
            context.SaveChanges();
        }

        #endregion
        #endregion

        #region Shared
        public delegate void LoopAdAction(Ad ad);

        public void LoopAds(LoopAdAction action)
        {
            foreach (Ad ad in context.Ads)
            {
                action.Invoke(ad);
            }
        }

        private void RegisterListeners()
        {
            ComboSelect.SelectionChanged += OnComboVenueSelect;
            BtnCreate.Click += OnCreateAdClick;
        }
        #endregion
    }
}