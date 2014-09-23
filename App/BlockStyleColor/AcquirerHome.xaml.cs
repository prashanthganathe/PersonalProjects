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
using Windows.Storage.Pickers;
using System.IO;




// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace BlockStyleColor
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class AcquirerHome : BlockStyleColor.Common.LayoutAwarePage
    {
        public AcquirerHome()
        {
            this.InitializeComponent();
          //  InitiateFileUploading();
        }

       
        public async void InitiateFileUploading()
        {
           
           StorageFolder picturesFolder = KnownFolders.DocumentsLibrary;
         
           //string path = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
           //string PhotoPath = path.Substring(0, path.IndexOf("AppData")) + picturesFolder.Name;

            //Windows.Storage.StorageFolder store;
            //FileOpenPicker openPicker = new FileOpenPicker();
            //openPicker.ViewMode = PickerViewMode.Thumbnail;
            ////openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            //openPicker.FileTypeFilter.Add(".jpg");
            //openPicker.FileTypeFilter.Add(".jpeg");
            //openPicker.FileTypeFilter.Add(".png");

            //StorageFile f = await openPicker.PickSingleFileAsync();
           // IReadOnlyList<StorageFile> files = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFilesAsync();
            Windows.Storage.StorageFolder store=await Windows.Storage.ApplicationData.Current.RoamingFolder.GetFolderAsync("Upload");
          // Windows.Storage.StorageFolder store = await StorageFolder.GetFolderFromPathAsync(@"c:\Carl");
            IReadOnlyList<StorageFile> files = await store.GetFilesAsync();

           // string[] filePaths = System.IO.Path.GetFiles(@"c:\MyDir\");

           //progressbar1.Value = 30;
           // IReadOnlyList<StorageFile> files = await picturesFolder.GetFilesAsync();
           // IReadOnlyList<StorageFolder> fol =await picturesFolder.GetFoldersAsync();
            //string bucketname = "TestingUploadS3";
            List<FileUploadCompleted> listUploadFiles = new List<FileUploadCompleted>();
            FileUploadCompleted uploadfile;
           // var storage = new Setting<List<FileUploadCompleted>>();

           //await storage.SaveAsync("Statistics", null);
            foreach (var file in files)
            {
                uploadfile = new FileUploadCompleted();
                   // progressbar1.Value = 0;
                   // AmazonFileBucketTransferUtil uploadTG = new AmazonFileBucketTransferUtil(bucketname, "key" + Guid.NewGuid().ToString(), file.Path);

                    uploadfile.UploadStartTime = DateTime.Now;
                    uploadfile.Name = file.Name;
                    uploadfile.Url = "https://s3-us-east-1.amazonaws.com/"+ExistingBucketName+"/" + file.Name;
                    //try
                    //{
                    //    System.IO.FileInfo f = new FileInfo(file.Path);
                    //    uploadfile.size=f.Length;
                    //}
                    //catch { }
                //uploadfile.size=file.
                   // currentfile.Text = "Currently Uploading file: " + file.Name;
                    await UploadFile(file.Name, file);
                    uploadfile.UploadEndtime = DateTime.Now;
                    listUploadFiles.Add(uploadfile);
                    //await UpdateRDS(uploadfile,patientobj);
                  // await storage.SaveAsync("Statistics", listUploadFiles);
                    //progressbar1.Value = .currentStatus;                       
            }                    
          // currentfile.Text = "All files are successfully uploaded";
          // notify.Text = "";
        //   await storage.SaveAsync("Statistics", listUploadFiles);
        }

        //private async Task<List<string>> GetFiles(StorageFolder folder)
        //{
        //    List<string> files = new List<string>();
        //    StorageFolder fold = folder;
        //    var items = await fold.GetItemsAsync();
        //    foreach (var item in items)
        //    {
        //        if (item.GetType() == typeof(StorageFile))
        //            files.Add(item.Path.ToString());
            
        //    }

        //   await  files;
        //}

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
            //this.Frame.Navigate(typeof(UploadImageTransferUtility));
            //this.Frame.Navigate(typeof(UploadImage));
           
        }
        private void Grid_Tapped_ReportQueue(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AcqRepList));
        }
        private void Grid_Tapped_UploadStats(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
           // this.Frame.Navigate(typeof(Stats));
            this.Frame.Navigate(typeof(StatsNew));
          //  this.Frame.Navigate(typeof(Windows8Theme.PB));

            
        }

        private void Grid_Tapped_DownloadStats(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
          
            this.Frame.Navigate(typeof( StatsNewDownload));
         

            
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

        private readonly int MB_SIZE = (int)Math.Pow(2, 20);
        string ExistingBucketName = "TestingUploadS3";
        public async Task UploadFile(string name, IStorageFile storageFile)
        {
            BasicAWSCredentials credentials = new BasicAWSCredentials("AKIAJTADDHY7T7GZXX5Q","n4xV5B25mt7e6br84H2G9SXBx8eDYTQJgCxoaF49");
            var s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast1);
            var transferUtilityConfig = new TransferUtilityConfig
            {
                ConcurrentServiceRequests = 5,
                MinSizeBeforePartUpload = 20 * MB_SIZE,
            };
            try
            {
                using (var transferUtility = new TransferUtility(s3Client, transferUtilityConfig))
                {
                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        BucketName = ExistingBucketName,
                        Key = name,
                        StorageFile = storageFile,
                        // Set size of each part for multipart upload to 10 MB
                        PartSize = 10 * MB_SIZE
                    };
                    uploadRequest.UploadProgressEvent += OnUploadProgressEvent;
                    await transferUtility.UploadAsync(uploadRequest);
                }
            }
            catch (AmazonServiceException ex)
            {
                //  oResponse.OK = false;
                //   oResponse.Message = "Network Error when connecting to AWS: " + ex.Message;
            }
        }

        public  async void OnUploadProgressEvent(object sender, UploadProgressArgs e)
        {
            if (e.PercentDone > 1)
            {
              //  progressbar1.Value = e.PercentDone;
            }
            if (e.PercentDone == 100)
            {
               // TransferUtilityUploadRequest c = sender as TransferUtilityUploadRequest;
              //  string filePath = "https://s3.amazonaws.com/" + ExistingBucketName + "/" + c.Key;
                await dummy();
                //   LogLocalStorage(filePath,c.Key);
            }
        }
          public async Task dummy()
          {
              string a = "b";
          }

          private void GetStats_Click(object sender, RoutedEventArgs e)
          {
              this.Frame.Navigate(typeof(Stats));
          }
        
    }
}
