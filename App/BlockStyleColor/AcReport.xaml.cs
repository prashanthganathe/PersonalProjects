using BlockStyleColor.EnableLiveTile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Windows8Theme.DataModel;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace BlockStyleColor
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class AcReport : BlockStyleColor.Common.LayoutAwarePage
    {
        public AcReport()
        {
           this.InitializeComponent();
           ShowDetails();
         //  var t = RequestQueue.selecteditem;
        }

        public void ShowDetails()
        {
            var t = AcqRepList.selecteditem as PatientREST;
            Details.Text="ReviewedDate : "+ t.ReviewedDate+"\r\n";
            Details.Text+="sex : "+ t.sex+"\r\n";
            Details.Text+="Status : Reviewed\r\n";
            //Details.Text+="Site : "+t.site+"\r\n";
            Details.Text+="Age : "+ t.age+"\r\n";
            Details.Text+="aflag : "+ t.aflag+"\r\n";
             Details.Text+="bflag : "+ t.bflag+"\r\n";
             Details.Text+="cflag : "+ t.cflag+"\r\n";
             Details.Text+="dflag : "+ t.dflag+"\r\n";
            Details.Text+="acomment : "+ t.acomment+"\r\n";
            Details.Text+="bcomment : "+ t.bcomment+"\r\n";
            Details.Text+="ccomment : "+ t.ccomment+"\r\n";
            Details.Text+="dcomment : "+ t.dcomment+"\r\n";

            BitmapImage bitmapImage = new BitmapImage();
          // img.Width = bitmapImage.DecodePixelWidth = 80; //natural px width of image source
           // don't need to set Height, system maintains aspect ratio, and calculates the other
           // dimension, so long as one dimension measurement is provided
           bitmapImage.UriSource = new Uri(t.image_right_url);
            imgtiger.Source= bitmapImage;
               
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          //  string username = this.txtusername.Text;
           // string password = this.txtpassword.Password;
            var loggeduser = new UserModel();// ValidateUser(username, password);
            if (loggeduser!=null)
            {
                switch (loggeduser.UserRole)
                {
                    case  Rolename.Administrator:
                        {
                            this.Frame.Navigate(typeof(AdminHome));
                            break;
                        }
                    case Rolename.Acquirer:
                        {
                            this.Frame.Navigate(typeof(AcquirerHome));
                            break;
                        }
                    case Rolename.Expert:
                        {
                            this.Frame.Navigate(typeof(ExpertHome));
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                
            }
        }

        public UserModel ValidateUser(string username, string pwd)
        {
           return UsersDataModel.ValidateUser(username, pwd);
        }

        private void TextBlock_SelectionChanged_1(object sender, RoutedEventArgs e)
        {

        }

        private void TextBlock_SelectionChanged_2(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
