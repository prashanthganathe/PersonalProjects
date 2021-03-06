﻿using BlockStyleColor.EnableLiveTile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows8Theme;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace BlockStyleColor
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class ExpertsResponse : BlockStyleColor.Common.LayoutAwarePage
    {
        App app;
        public ExpertsResponse()
        {
           this.InitializeComponent();
           InitialPatientInfo obj = AcqList.selecteditem;
           txtInfo.Text = "File:"+ obj.FileName+"\r\n Sex:" + obj.sex + "   DOB:" + obj.dateOfBirth + "   CreatedOn :" + obj.createdAt;
           app = App.Current as App;

         
           BitmapImage bitmapImage = new BitmapImage();
          // img.Width = bitmapImage.DecodePixelWidth = 80; //natural px width of image source
           // don't need to set Height, system maintains aspect ratio, and calculates the other
           // dimension, so long as one dimension measurement is provided
           bitmapImage.UriSource = new Uri(obj.imageLeftUrl);


           imageshow.Source = bitmapImage;
            // img = new BitmapImage(new Uri("ms-appx:///Assets/SplashScreen.png"));        
            //  imgsrc1.ImageSource = img;
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            CreateLiveTile.ShowliveTile(true, "BlockStyleColor Template");
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void Grid_Tapped_1(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ItemSubPage));
        }

        private void Grid_Tapped_Login(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(ItemSubPage));
            //this.Frame.Navigate(typeof(Login));
        }

        private void Grid_Tapped_Audience(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AudienceInfo));
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void pageTitle_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }


        private void Home_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            StringBuilder strb = new StringBuilder();
            strb.Append("patient_id=" + AcqList.selecteditem.id);
            strb.Append("&acomment=" + this.acomment.Text);
            strb.Append("&bcomment=" + this.bcomment.Text);
            strb.Append("&ccomment=" + this.ccomment.Text);
            strb.Append("&dcomment=" + this.dcomment.Text);
            strb.Append("&aflag=" + aflag.IsOn);
            strb.Append("&bflag=" + bflag.IsOn);
            strb.Append("&cflag=" + cflag.IsOn);
            strb.Append("&dflag=" + dflag.IsOn);          
            strb.Append("&auth_token=" + app.AccessToken);
            strb.Append("&expertID=" + app.UserId);
            strb.Append("&user_name=" + app.Username);
            HttpResponseMessage response = client.PostAsync(app.DomainName + "/rest/case/uploadReview?" + strb.ToString(), new StringContent("")).Result;
            if (response.IsSuccessStatusCode)
            {
              //  this.Frame.Navigate(typeof(ExpertHome));
                this.txtinfo.Text = "Successfully Comments submitted.";
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void TextBlock_SelectionChanged_1(object sender, RoutedEventArgs e)
        {

        }

        private void TextBlock_SelectionChanged_2(object sender, RoutedEventArgs e)
        {
                    }

        private void TextBlock_SelectionChanged_3(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        private void TextBlock_SelectionChanged_4(object sender, RoutedEventArgs e)
        {

        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
