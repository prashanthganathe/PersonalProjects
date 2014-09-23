using Amazon.Runtime;
using BlockStyleColor.EnableLiveTile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
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
    public sealed partial class AudienceInfo : BlockStyleColor.Common.LayoutAwarePage
    {
        public AudienceInfo()
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
            CreateLiveTile.ShowliveTile(false, "BlockStyleColor Template");
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

     

        private async void Upload(object sender, RoutedEventArgs e)
        {
            try
            {
                await UploadBackgroundAmazon();
            }
            catch(Exception ex)
            {

            }
        }

        public async Task UploadBackgroundAmazon()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add("*");
            IReadOnlyList<StorageFile> file = await openPicker.PickMultipleFilesAsync();
            if(file!=null)
                await AmazonUploadFile(file);
        }

        private async Task AmazonUploadFile(IReadOnlyList<StorageFile> files)
        {
          
            AmazonFileBucketBackground uploadBG = new AmazonFileBucketBackground("com.mf.carl-prototype", "key" + Guid.NewGuid().ToString(), "");
            
            await uploadBG.UploadFile(files);

            //AmazonFileBucketAsync s3FileBucket;
            //BasicAWSCredentials basicAwsCredentials = new BasicAWSCredentials("AKIAIVMK5XV3GURYM7ZA", "9Xgy0PwXhz6sjL3hS9QUIHr/SsJIKNxBdNlCyJh1");
            //foreach (var f in file)
            //{
            //    s3FileBucket = new AmazonFileBucketAsync("com.mf.carl-prototype", Guid.NewGuid().ToString(), "");
            //    await s3FileBucket.UploadFile(basicAwsCredentials, Guid.NewGuid().ToString(), f);
            //}
        }

        private async void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await downloadFromAmazon();           
        }

        public async Task downloadFromAmazon()
        {
            AmazonFileBucketBackground AWSBucketBG = new AmazonFileBucketBackground("com.mf.carl-prototype", "key" + Guid.NewGuid().ToString(), "0a8ea2cc-6f81-4d2e-8f4e-96e80ecf0620");
            await AWSBucketBG.DowloadFile();

        }



        private async void UploadTransferUtility(object sender, RoutedEventArgs e)
        {
            await UploadAmazonTransferUtil();
        }
        public async Task UploadAmazonTransferUtil()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add("*");
            IReadOnlyList<StorageFile> file = await openPicker.PickMultipleFilesAsync();
            if (file != null)
                await AmazonUploadFileTransfer(file);
        }

        private async Task AmazonUploadFileTransfer(IReadOnlyList<StorageFile> file)
        {
            // AmazonFileBucketBackground uploadBG = new AmazonFileBucketBackground("com.mf.carl-prototype","key"+Guid.NewGuid().ToString(),"");
            //  await uploadBG.UploadFile(file);

            AmazonFileBucketTransferUtil s3FileBucket;
            BasicAWSCredentials basicAwsCredentials = new BasicAWSCredentials("AKIAIVMK5XV3GURYM7ZA", "9Xgy0PwXhz6sjL3hS9QUIHr/SsJIKNxBdNlCyJh1");
            foreach (var f in file)
            {
                s3FileBucket = new AmazonFileBucketTransferUtil("com.mf.carl-prototype", Guid.NewGuid().ToString(), "");
                await s3FileBucket.UploadFile(Guid.NewGuid().ToString(), f);
            }
        }

        
    }
}
