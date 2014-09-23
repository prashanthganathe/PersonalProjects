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
    public sealed partial class Response : BlockStyleColor.Common.LayoutAwarePage
    {
        public Response()
        {
           this.InitializeComponent();
           InitialPatientInfo obj = AcqList.selecteditem;
           txtInfo.Text = " Sex:" + obj.sex + "   DOB:" + obj.dateOfBirth + "   CreatedOn :" + obj.createdAt;
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

        public ImageSource img { get; set; }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

          InitialPatientInfo obj=  AcqList.selecteditem;
          txtInfo.Text = " Sex:" + obj.sex + "   DOB:" + obj.dateOfBirth + "   CreatedOn :" + obj.createdAt;

         // img = new BitmapImage(new Uri("ms-appx:///Assets/SplashScreen.png"));        
        //  imgsrc1.ImageSource = img;
         
    //             age: "201"
    //createdAt: "2014-08-06"
    //dateOfBirth: null
    //id: "10"
    //imageLeftUrl: "https://s3-us-east-1.amazonaws.com/TestingUploadS3/Canon 5D MarkIII_AF_Small_100_NA_NA_NA__(0000).jpg"
    //imageRightUrl: "https://s3-us-east-1.amazonaws.com/TestingUploadS3/Canon 5D MarkIII_AF_Small_100_NA_NA_NA__(0000).jpg"
    //sex: "Female"
           
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
