
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using Amazon.Runtime;
using System.IO;
using Amazon.S3;
using Windows.Storage;
using Amazon;
using System.Threading;
using Windows8Theme.DataModel.Model;
using IsolatedStorageW8;

namespace Windows8Theme.AWS
{
    public class AmazonFileBucketTransferUtil : AmazonBucket
    {
        public AmazonFileBucketTransferUtil(string existingBucketName, string keyName, string filePath)
        {
            ExistingBucketName = existingBucketName;
            KeyName = keyName;
            Filename = filePath;
        }
        public int currentStatus;

        private BasicAWSCredentials credentials = new BasicAWSCredentials("AKIAJTADDHY7T7GZXX5Q","n4xV5B25mt7e6br84H2G9SXBx8eDYTQJgCxoaF49");
        private readonly int MB_SIZE = (int)Math.Pow(2, 20);
        public async Task UploadFile(string name,IStorageFile storageFile)
        {            
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


        void OnUploadProgressEvent(object sender, UploadProgressArgs e)
        {           
            if(e.PercentDone>1)
            {
                currentStatus = e.PercentDone;
            }
            if (e.PercentDone == 100)
            {
                TransferUtilityUploadRequest c = sender as TransferUtilityUploadRequest;              
                string filePath = "https://s3.amazonaws.com/"+ExistingBucketName+"/" + c.Key;
             //   LogLocalStorage(filePath,c.Key);
            }           
        }
        public async Task LogLocalStorage(string path, string name)
        {
            //List<FileUploadCompleted> loglist = new List<FileUploadCompleted>();
            //loglist.Add(new FileUploadCompleted { name = name, uploadedOn = DateTime.Now, url = path });

            //var applicationData = Windows.Storage.ApplicationData.Current;
            //var localSettings = applicationData.LocalSettings;

            //if(localSettings.Values["uploadlog"] !=null)
            //{
            //    List<FileUploadCompleted> localloglist = (List<FileUploadCompleted>)localSettings.Values["uploadlog"]; 
            //}
            //else
            //{
            //    localSettings.Values["uploadlog"] = loglist;
            //}

            var storage = new Setting<List<FileUploadCompleted>>();
            List<FileUploadCompleted> obj = await storage.LoadAsync("uploadlog");

            List<FileUploadCompleted> loglist;
            if(obj!=null)
            {
                if (obj.Where(p => p.Name != name).ToList().Count < 1)
                {
                    loglist = obj;
                    loglist.Add(new FileUploadCompleted { Name = name,  UploadEndtime = DateTime.Now, Url = path });
                    await storage.SaveAsync("uploadlog", loglist);
                }
            }
            else
            {
                loglist = new List<FileUploadCompleted>();
                await storage.SaveAsync("uploadlog", loglist); 
            }

        }

        
        

        public async Task DownloadFile(IStorageFile storageFile, string bucket, string key, AWSCredentials credentials, CancellationToken cancellationToken)
        {
            var s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast1);
            using (var transferUtility = new TransferUtility(s3Client))
            {
                var downloadRequest = new TransferUtilityDownloadRequest
                {
                    BucketName = bucket,
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

        public async Task AbortUploadFile(BasicAWSCredentials credentials, string bucketname, string key, string uploadid)
        {
            var s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast1);
            await s3Client.AbortMultipartUploadAsync(new AbortMultipartUploadRequest
            {
                BucketName = bucketname,
                Key = key,
                UploadId = uploadid
            });
        }
    }
}

