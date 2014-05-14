using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MvcWebRole1
{
    public class AzureClientWrapper
    {
        protected CloudStorageAccount StorageAccount;
        protected CloudTableClient TableClient;
        protected CloudBlobClient BlobClient;

        
        // don't forget to pass in your Storage account connection string  
        public AzureClientWrapper(string storageAccountConnectionString)
        {
            StorageAccount =  CloudStorageAccount.Parse(storageAccountConnectionString); 
            TableClient = StorageAccount.CreateCloudTableClient();
            BlobClient = StorageAccount.CreateCloudBlobClient();
        }

        public static void CreateTablesQueuesBlobContainers()
        {
            var connectionString = ConfigurationManager.AppSettings["TableConnectionName"];
            var storageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue(connectionString));
            var tableClient = storageAccount.CreateCloudTableClient();
            var mailingListTable = tableClient.GetTableReference(ConfigurationManager.AppSettings["TableMailinglist"]);
            mailingListTable.CreateIfNotExists();
            var messageTable = tableClient.GetTableReference(ConfigurationManager.AppSettings["TableMessage"]);
            messageTable.CreateIfNotExists();
            var blobClient = storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(ConfigurationManager.AppSettings["BlobAzuremailblobcontainer"]);
            blobContainer.CreateIfNotExists();
            var queueClient = storageAccount.CreateCloudQueueClient();
            var subscribeQueue = queueClient.GetQueueReference(ConfigurationManager.AppSettings["QueueAzuremailsubscribequeue"]);
            subscribeQueue.CreateIfNotExists();
        }

        protected byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }  
}