using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Configuration;

namespace MvcWebRole1
{
    public class StorageWrapper
    {
        
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
    }
}