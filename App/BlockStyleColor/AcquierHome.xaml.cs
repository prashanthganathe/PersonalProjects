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




// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace BlockStyleColor
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class AcquireHome : BlockStyleColor.Common.LayoutAwarePage
    {
        public AcquireHome()
        {
            this.InitializeComponent();
            InitiateFileUploading();
        }

       
        public async void InitiateFileUploading()
        {
           StorageFolder picturesFolder = KnownFolders.PicturesLibrary;
           // StorageFolder picturesFolder = ApplicationData.Current.LocalFolder;
          //  List<string> latestfiles =await UploadRequired(picturesFolder);
           // if (latestfiles != null)
           progressbar1.Value = 30;
            bool flag= await UploadRequired(picturesFolder);
            if (flag)
            {
              //  if (latestfiles.Count > 0)
              //  {
                    IReadOnlyList<StorageFile> files = await picturesFolder.GetFilesAsync();
                    string bucketname = "TestingUploadS3";
                    foreach (var file in files)
                    {
                         progressbar1.Value = 0;
                         AmazonFileBucketTransferUtil uploadTG = new AmazonFileBucketTransferUtil(bucketname, "key" + Guid.NewGuid().ToString(), file.Path);

                         currentfile.Text = "Currently Uploading file: " + file.Name + ". Completed " + uploadTG.currentStatus.ToString() + "%";
                         await uploadTG.UploadFile(file.Name, file);
                         progressbar1.Value = uploadTG.currentStatus;                       
                    }                    
                    currentfile.Text = "All files are successfully uploaded";
              //  }
            }
        }

        public async Task<Boolean> UploadRequired(StorageFolder picturesFolder)
        {
          
          //  var storage = new Setting<List<FileUploadCompleted>>(); 
            //List<FileUploadCompleted> a = new List<FileUploadCompleted>();
            //storage.SaveAsync("data", a);
            //List<string> touploadfile = new List<string>();
          //  IReadOnlyList<StorageFile> files =  await picturesFolder.GetFilesAsync();
          //  var obj = await storage.LoadAsync("uploadlog");    
          //  if(obj!=null)
            //{
            //   // var result = files.Where(u => obj.Any(l => l.name != u.Name)).Select(p => p.Name);
            //    var result = files.Select(p => p.Name).Except(obj.Select(l => l.name));
            //    return result.ToList();
            //}
            //else
            //{
            //    return files.Select(p=>p.Name).ToList();
            //}


            //if(localSettings.Values["uploadlog"] !=null)
            //{
            //    List<FileUploadCompleted> localdata = localSettings.Values["uploadlog"] as List<FileUploadCompleted>;

            //    var result = files.Where(u => !localdata.Any(l => l.name != u.Name)).Select(p => p.Name);

            //    return result.ToList();
            //    //foreach(var f in files)
            //    //{
            //    //    foreach(var item in localSettings.Values["uploadlog"] as List<FileUploadCompleted>)
            //    //    {
                      
            //    //    }
            //    //}

            //}
            //else
            //{
            //    foreach(var f in files)
            //    {
            //        touploadfile.Add(f.Name);
            //    }
            //}
            ////IReadOnlyList<StorageFile> files = System.IO.d picturesFolder.Path;
            //return touploadfile;
            return true;
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
            //this.Frame.Navigate(typeof(UploadImageTransferUtility));
            //this.Frame.Navigate(typeof(UploadImage));
           
        }
        private void Grid_Tapped_ReportQueue(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ReportQueue));
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

        
    }
}
