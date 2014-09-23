using BlockStyleColor.EnableLiveTile;
using IsolatedStorageW8;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows8Theme.Common;
using Windows8Theme.DataModel;
using Windows8Theme.DataModel.Model;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace BlockStyleColor
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class StatsDownload : BlockStyleColor.Common.LayoutAwarePage
    {
        public static MainPage Current;
        public StatsDownload()
        {
           this.InitializeComponent();
         
           Refresh();
      
        }
        public  void GetStats()
        {
            ItemGridView.ItemsSource = null;
            ItemGridView.ItemsSource = AutoDownload.downloadlist;
           // ItemGridViewDownload.ItemsSource = AutoUpload.patientList; 
            if (AutoDownload.downloadlist.Count != 0)
            {
                double currentStatus = AutoDownload.downloadlist.Average(p => p.Status);
                progressbar.Value = currentStatus;
                uploadinfo.Text = " Total " + AutoDownload.downloadlist.Count.ToString() + " files. Completed " + progressbar.Value.ToString() + "%";
            }
            else
            {
                uploadinfo.Text = "Currently no files to get download from Server.";
            }
        }



        private async void Refresh()
            {
                while (true)
                {
                    if (progressbar.Value < 100)
                    {
                        GetStats();
                        await Task.Delay(2000);
                       
                    }
                    else
                        break;
                   
                   
                }
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

       

        public UserModel ValidateUser(string username, string pwd)
        {
           return UsersDataModel.ValidateUser(username, pwd);
        }

        private void ItemGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        
    }
}
