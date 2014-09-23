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
using Windows.UI;
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
    public sealed partial class StatsNew : BlockStyleColor.Common.LayoutAwarePage
    {
        public static MainPage Current;
        public StatsNew()
        {
           this.InitializeComponent();
         
           Refresh();
      
        }

     
        public  async void GetStats()
        {
            ItemGridView.ItemsSource = null;

           // AutoUpload.patientList.Reverse();
            ItemGridView.ItemsSource = AutoUpload.patientList;
            int count = 0;
           // ItemGridViewDownload.ItemsSource = AutoUpload.patientList; 
            if (AutoUpload.patientList.Count > 0)
            {
                double currentStatus = AutoUpload.patientList.Average(p => p.Image1.Status);
               if(currentStatus <30)
                   progressbar.Foreground = new SolidColorBrush(Colors.Orange);
                if(currentStatus >30 && currentStatus < 70)
                   progressbar.Foreground= new SolidColorBrush(Colors.LightBlue);
                if (currentStatus > 70 && currentStatus < 90)
                   progressbar.Foreground= new SolidColorBrush(Colors.LightGreen);
                if (currentStatus > 90 && currentStatus <= 100)
                    progressbar.Foreground = new SolidColorBrush(Colors.Green);
                progressbar.Value = currentStatus;

                uploadinfo.Text = " Total " + AutoUpload.patientList.Count.ToString() + " files. Completed " + progressbar.Value.ToString() + "%";
                try
                {
                    var storage = new Setting<int>();
                     count= await storage.LoadAsync("CurrentUploadCount");
                    uploadinfo.Text = " Total " + count.ToString() + " files. Completed " + progressbar.Value.ToString() + "%";
                }
                catch
                {

                }

                //uploadinfo.Text = " Total " + AutoUpload.patientList.Count.ToString() + " files. Completed " + progressbar.Value.ToString() + "%";
                uploadinfo.Text += "\r\n total Size: " + AutoUpload.patientList.Sum(p => p.Image1.Size).ToString() + " MB";
               // uploadinfo.Text += "\r\n Start time: " + AutoUpload.patientList[0].Image1.UploadStartTime;

                uploadinfo.Text += "\r\n Start time: " + AutoUpload.StartTime;
                if (count > 0)
                {
                    // uploadinfo.Text += "\r\n Endtime time: " + AutoUpload.patientList[AutoUpload.patientList.Count - 1].Image1.UploadEndtime;
                    uploadinfo.Text += "\r\n EndTime :" + AutoUpload.EndTime;
                }
            }
            else
            {
                uploadinfo.Text = "Currently there is no files to get upload to Server.";
            }
        }



        private async void Refresh()
            {
                while (true)
                {
                    if (progressbar.Value < 100)
                    {
                        GetStats();
                        await Task.Delay(5000);                       
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
