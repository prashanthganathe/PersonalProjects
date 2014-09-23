using BlockStyleColor.EnableLiveTile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows8Theme.AWS;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace BlockStyleColor
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class UploadImage : BlockStyleColor.Common.LayoutAwarePage
    {
        public UploadImage()
        {
           this.InitializeComponent();
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await UploadBackgroundAmazon();

           
        }

        public async Task UploadBackgroundAmazon()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add("*");
            IReadOnlyList<StorageFile> file = await openPicker.PickMultipleFilesAsync();
            if (file != null)
                await AmazonUploadFile(file);
        }

        private async Task AmazonUploadFile(IReadOnlyList<StorageFile> files)
        {
            string bucketname = "TestingUploadS3";
            AmazonFileBucketBackground uploadBG = new AmazonFileBucketBackground(bucketname, "key" + Guid.NewGuid().ToString(), "");

            await uploadBG.UploadFile(files);

            //AmazonFileBucketAsync s3FileBucket;
            //BasicAWSCredentials basicAwsCredentials = new BasicAWSCredentials("AKIAIVMK5XV3GURYM7ZA", "9Xgy0PwXhz6sjL3hS9QUIHr/SsJIKNxBdNlCyJh1");
            //foreach (var f in file)
            //{
            //    s3FileBucket = new AmazonFileBucketAsync("com.mf.carl-prototype", Guid.NewGuid().ToString(), "");
            //    await s3FileBucket.UploadFile(basicAwsCredentials, Guid.NewGuid().ToString(), f);
            //}
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void TextBlock_SelectionChanged_1(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
