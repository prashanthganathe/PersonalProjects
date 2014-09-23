using BlockStyleColor.EnableLiveTile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows8Theme.AWS;
using Windows8Theme.DataModel;
using Windows8Theme.DataModel.Model;
using IsolatedStorageW8;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.Runtime;
using Amazon;
using System.Threading;
using Amazon.S3.Model;
using Amazon.S3.Util;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace BlockStyleColor
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class ExpertHome : BlockStyleColor.Common.LayoutAwarePage
    {
        private readonly int MB_SIZE = (int)Math.Pow(2, 20);
        string ExistingBucketName = "TestingUploadS3";
        public ExpertHome()
        {
            this.InitializeComponent();
            //url,sex,acomment,bcomment,ccomment,dcomment, aflag

            //ms-appdata:///roaming/images/logo.png
            //  DownloadInitialize();
           // CancellationToken ct = new CancellationToken(false);
            //foreach(var item in items.)
            //{ 

            //}
            
        }

        public async Task DownloadInitialize()
        {
            IList<S3Object> items= await GetListofFiles();
           // return await ApplicationData.Current.LocalFolder.CreateFileAsync("tmp.xml", CreationCollisionOption.ReplaceExisting);
            //string saveToo = @"C:\xyz.jpg";
           // StorageFile sf = await StorageFile.GetFileFromPathAsync("cal.png");           
           
          CancellationToken ct = new CancellationToken(false);

         
               foreach (S3Object obj in items.ToList())
               {
                   StorageFolder fol=await Windows.Storage.ApplicationData.Current.RoamingFolder.GetFolderAsync("Download");
                   StorageFile f =await fol.CreateFileAsync(obj.Key);
                   await DownloadFile(f,obj.Key,ct);
               }
           
        }

        public async Task<IList<S3Object>> GetListofFiles()
        {
           
               
                BasicAWSCredentials credentials = new BasicAWSCredentials("AKIAJTADDHY7T7GZXX5Q", "n4xV5B25mt7e6br84H2G9SXBx8eDYTQJgCxoaF49");
                AmazonS3Client s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast1);
                string token = null;
                var result = new List<S3Object>();
                do
                {
                    ListObjectsRequest request = new ListObjectsRequest()
                    {
                        BucketName = ExistingBucketName    
                    };
                    ListObjectsResponse response = await s3Client.ListObjectsAsync(request).ConfigureAwait(false);
                    result.AddRange(response.S3Objects);
                    token = response.NextMarker;

                } while (token != null);              
                
             return result;
            
            
        }
        async Task dummy()
        {
        }
        public async Task DownloadFile(IStorageFile storageFile, string key, CancellationToken cancellationToken)
        {
            
            BasicAWSCredentials credentials = new BasicAWSCredentials("AKIAJTADDHY7T7GZXX5Q", "n4xV5B25mt7e6br84H2G9SXBx8eDYTQJgCxoaF49");
            var s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast1);
            
            //files = await Windows.Storage.DownloadsFolder.CreateFileAsync(); 
            using (var transferUtility = new TransferUtility(s3Client))
            {
                var downloadRequest = new TransferUtilityDownloadRequest
                {
                    BucketName = ExistingBucketName,
                    Key = key,
                    StorageFile = storageFile
                };
                downloadRequest.WriteObjectProgressEvent += OnWriteObjectProgressEvent;
                await transferUtility.DownloadAsync(downloadRequest, cancellationToken);
            }

           
            
        }

        void OnWriteObjectProgressEvent(object sender, WriteObjectProgressArgs e)
        {
            // Process progress update events.
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

        //private void Grid_Tapped_1(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        //{
        //    this.Frame.Navigate(typeof(ItemSubPage));
        //}

        //private void Grid_Tapped_Login(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        //{
        //    //this.Frame.Navigate(typeof(ItemSubPage));
        //    //this.Frame.Navigate(typeof(Login));
        //}

        //private void Grid_Tapped_Audience(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        //{
        //    this.Frame.Navigate(typeof(AudienceInfo));
        //}


        private void Grid_Tapped_Request(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
           // this.Frame.Navigate(typeof(RequestQueue));
            this.Frame.Navigate(typeof(AcqList));
            
        }
        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void pageTitle_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
