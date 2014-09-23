

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
using Amazon.S3;
using Amazon.S3.Model;
using System.Threading;
using Windows.Networking.BackgroundTransfer;
using Windows.Security.Credentials;

namespace Windows8Theme.AWS
{
    public class AmazonFileBucketBackground : AmazonBucket
    {
        public AmazonFileBucketBackground(string existingBucketName, string keyName, string filename)
        {
            ExistingBucketName = existingBucketName;
            KeyName = keyName;
            Filename = filename;
           
        }

        private void UploadFileToS3(string filePath,string bucketname)
        {
            var awsAccessKey = accesskey;
            var awsSecretKey = secretkey;
            var existingBucketName = bucketname;
            var client =  Amazon.AWSClientFactory.CreateAmazonS3Client(awsAccessKey, awsSecretKey, RegionEndpoint.USEast1);

            var uploadRequest = new TransferUtilityUploadRequest
            {
                FilePath = filePath,
                BucketName = existingBucketName,
                CannedACL = S3CannedACL.PublicRead
            };

            var fileTransferUtility = new TransferUtility(client);
            fileTransferUtility.UploadAsync(uploadRequest);
        } 

        BasicAWSCredentials basicAwsCredentials;
         private const string accesskey = "AKIAJTADDHY7T7GZXX5Q";//"AKIAIVMK5XV3GURYM7ZA";
        private const string secretkey = "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";//"9Xgy0PwXhz6sjL3hS9QUIHr/SsJIKNxBdNlCyJh1";
      
       private const string bucketurl = "https://s3-us-west-2.amazonaws.com/";

       // private const string bucketurl = "http://s3.amazonaws.com/";

       // private const string bucketurl = "com.mf.carl-prototype.s3.amazonaws.com";
        #region Upload
        public async Task UploadFile(IReadOnlyList<StorageFile> files)
        {
            try
            {
               

                basicAwsCredentials = new BasicAWSCredentials(accesskey, secretkey);
                List<BackgroundTransferContentPart> parts = new List<BackgroundTransferContentPart>();
                for (int i = 0; i < files.Count; i++)
                {
                    BackgroundTransferContentPart part = new BackgroundTransferContentPart(files[i].Name, files[i].Name);
                    this.Filename = files[i].Name;
                    part.SetFile(files[i]);
                    parts.Add(part);
                }
                //Uri uri = new Uri(bucketurl + ExistingBucketName + "/");
                //Uri uri = new Uri("https://com.mf.carl-prototype.s3-us-west-2.amazonaws.com/");
                Uri uri = new Uri("https://s3.amazonaws.com/" + ExistingBucketName +"/");
              //  Uri uri = new Uri("https://"+ExistingBucketName+".s3-us-west-2.amazonaws.com/" );
            
                BackgroundUploader uploader = new BackgroundUploader();

                PasswordCredential pwdCredential = new PasswordCredential();
                pwdCredential.UserName = accesskey;
                pwdCredential.Password = secretkey;
                uploader.ServerCredential = pwdCredential;
               // uploader.Method = "POST";
       
               // uploader.SetRequestHeader("Content-Type", "multipart/form-data;boundary=34");
                  uploader.ServerCredential = new PasswordCredential{ UserName=accesskey, Password= secretkey};
                UploadOperation upload = await uploader.CreateUploadAsync(uri, parts);
                // Attach progress and completion handlers.
                await HandleUploadAsync(upload, true);
            }
            catch(Exception ex)
            {

            }
        }

        private async Task HandleUploadAsync(UploadOperation upload, bool start)
        {
            try
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                Progress<UploadOperation> progressCallback = new Progress<UploadOperation>();
                if (start)
                {
                    // Start the upload and attach a progress handler.
                    await upload.StartAsync().AsTask(cts.Token, progressCallback);
                }
                else
                {
                    // The upload was already running when the application started, re-attach the progress handler.
                    await upload.AttachAsync().AsTask(cts.Token, progressCallback);
                }

                ResponseInformation response = upload.GetResponseInformation();
                //  Log(String.Format("Completed: {0}, Status Code: {1}", upload.Guid, response.StatusCode));
            }
            catch (TaskCanceledException)
            {
                // Log("Upload cancelled.");
            }
            catch (Exception ex)
            {
                //LogException("Error", ex);
            }
        }

        #endregion

      
        
        
        #region Download
        public async Task DowloadFile()
        {

            Uri Source;
             
            Uri.TryCreate(bucketurl+ExistingBucketName+"/"+ Filename, UriKind.Absolute, out Source);
            string destination = DateTime.Now.Ticks.ToString();
            StorageFile destinationFile;
            destinationFile = await KnownFolders.VideosLibrary.CreateFileAsync(destination, CreationCollisionOption.GenerateUniqueName);

            BackgroundDownloader downloader = new BackgroundDownloader();
            //PasswordCredential a = new PasswordCredential();
            //a.UserName = accesskey;
            //a.Password = secretkey;
            //downloader.ServerCredential = a;
            //downloader.ServerCredential = new PasswordCredential()
            //downloader.ServerCredential.UserName = accesskey;
            //downloader.ServerCredential.Password = secretkey;
            DownloadOperation download = downloader.CreateDownload(Source, destinationFile);
            await download.StartAsync();
        }
        #endregion

    }

}