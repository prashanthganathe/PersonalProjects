using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System.IO;
using System.Configuration; 

namespace MvcWebRole1.WrapperClass
{
    public class BlobHelper : AzureClientWrapper
    {
        CloudBlobClient blobClient;
        CloudBlobContainer blobContainer;
        public BlobHelper(string storageAccountConnectionString) : base(storageAccountConnectionString)
        {
            blobClient = base.StorageAccount.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference(ConfigurationManager.AppSettings["BlobAzuremailblobcontainer"]);
            blobContainer.CreateIfNotExists();
        }        

        internal  void Save(string blobName, HttpPostedFileBase httpPostedFile)
        {
                        
            // Retrieve reference to a blob.
            var blob = blobContainer.GetBlockBlobReference(blobName);
            // Create the blob or overwrite the existing blob by uploading a local file.
            using (var fileStream = httpPostedFile.InputStream)
            {
                blob.UploadFromStream(fileStream);
            }
        }

        internal void UpdateBlob(string blobName, HttpPostedFileBase httpPostedFile)
        {
            // Retrieve reference to a blob.
            var blob = blobContainer.GetBlockBlobReference(blobName);
            // Create the blob or overwrite the existing blob by uploading a local file.
            using (var fileStream = httpPostedFile.InputStream)
            {
                blob.UploadFromStream(fileStream);
            }
        }

        internal void DeleteBlob(string blobName)
        {
            var blob = blobContainer.GetBlockBlobReference(blobName);
            blob.Delete();
        }
    }  
}