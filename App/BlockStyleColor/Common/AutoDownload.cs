using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IsolatedStorageW8;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.Runtime;
using Amazon;
using Windows.Storage.Pickers;
using System.IO;
using Windows.Storage;
using Windows8Theme.DataModel.Model;
using System.Threading;
using Amazon.S3.Model;
using BlockStyleColor;

namespace Windows8Theme.Common
{
    public class AutoDownload
    {
        App app;
        public AutoDownload()
        {
            app = App.Current as App;
        }
        public static string ExistingBucketName = "TestingUploadS3";
        public static string S3url = "https://s3-us-east-1.amazonaws.com/";        
        private readonly int MB_SIZE = (int)Math.Pow(2, 20);
        public static List<DownloadFile> downloadlist = new List<DownloadFile>();
        public static string StartTime { get; set; }
        public static string EndTime { get; set; }


        public  void DeletePartiallyDownloadedItems()
        {
            downloadlist.RemoveAll(x => x.Status < 100);
        }

        public async void InitializeDownload()
        {
            var storage = new Setting<List<DownloadFile>>();
            StartTime = DateTime.Now.ToString();
            try
            {
                IList<S3Object> items = await GetListofFiles();
                DownloadFile DownloadFileObj;
                CancellationToken ct = new CancellationToken(false);
                DeletePartiallyDownloadedItems();
                  IEnumerable<string> filesTemp;
                 if (downloadlist.Count > 0)
                         filesTemp = items.Select(p => p.Key).Except(downloadlist.Select(x => x.Name));
                 else
                     filesTemp = items.Select(p => p.Key);

                foreach (S3Object obj in items.ToList())
                {
                    StorageFolder fol = await Windows.Storage.ApplicationData.Current.RoamingFolder.GetFolderAsync("Download");
                    try
                    {
                        foreach (var newfiles in filesTemp)
                        {
                            try
                            {
                                if (newfiles == obj.Key)
                                {
                                   // if (newfiles.Contains(app.Username))
                                   // {
                                        // await DownloadFile(f, obj.Key, ct);
                                        DownloadFileObj = new DownloadFile { Name = obj.Key, Size = obj.Size / 1000000 };
                                        downloadlist.Add(DownloadFileObj);
                                        StorageFile f = await fol.CreateFileAsync(obj.Key, CreationCollisionOption.OpenIfExists);
                                        await Task.WhenAll(Task.Factory.StartNew(() => DownloadFile(f, DownloadFileObj, ct)));
                                        //await Task.WhenAll(Task.Run(() => DownloadFile(f, DownloadFileObj, ct)));

                                        await storage.SaveAsync("DownloadStat", downloadlist);
                                    //}
                                }
                            }
                            catch { }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch { }

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


        public async Task DownloadFile(IStorageFile storageFile, DownloadFile DownloadFileObj, CancellationToken cancellationToken)
        {
            BasicAWSCredentials credentials = new BasicAWSCredentials("AKIAJTADDHY7T7GZXX5Q", "n4xV5B25mt7e6br84H2G9SXBx8eDYTQJgCxoaF49");
            var s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast1);
            //files = await Windows.Storage.DownloadsFolder.CreateFileAsync(); 
            DownloadFileObj.UploadStartTime = DateTime.Now;
            using (var transferUtility = new TransferUtility(s3Client))
            {
                var downloadRequest = new TransferUtilityDownloadRequest
                {
                    BucketName = ExistingBucketName,
                    Key = DownloadFileObj.Name,
                    StorageFile = storageFile
                };
                downloadRequest.WriteObjectProgressEvent += OnWriteObjectProgressEvent;
                await transferUtility.DownloadAsync(downloadRequest, cancellationToken);
            }
        }

        void OnWriteObjectProgressEvent(object sender, WriteObjectProgressArgs e)
        {
            var currentobject = sender as Amazon.S3.Transfer.TransferUtilityDownloadRequest;
            var s = downloadlist.Where(p => p.Name == currentobject.Key).FirstOrDefault();
            if (s != null)
            {
                if (e.PercentDone > 1)
                {
                    s.Status = e.PercentDone;
                }
                if (e.PercentDone == 100)
                {
                    UpdateDownloads(s);
                    s.UploadEndtime = DateTime.Now;
                    s.Status = e.PercentDone;
                    EndTime = DateTime.Now.ToString();
                }
            }
            // Process progress update events.
           // downloadlist
        }

        public bool UpdateDownloads(DownloadFile s)
        {
            return true;
        }

  
    }
}
