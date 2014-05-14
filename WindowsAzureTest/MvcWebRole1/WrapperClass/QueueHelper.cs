using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MvcWebRole1.WrapperClass
{
    public class QueueHelper:AzureClientWrapper
    {
        private CloudQueue subscribeQueue;
        private CloudQueueClient queueClient;

        public QueueHelper(string storageAccountConnectionString)
            : base(storageAccountConnectionString)
        {

            queueClient = base.StorageAccount.CreateCloudQueueClient();
            subscribeQueue = queueClient.GetQueueReference(ConfigurationManager.AppSettings["QueueAzuremailsubscribequeue"]);
            subscribeQueue.CreateIfNotExists();            
        }

        public void CreateQueueMessage(string newSubscriber, TimeSpan time)
        {
            subscribeQueue.AddMessage(new CloudQueueMessage(newSubscriber));
        }

        private void DeleteMessage()
        {
            subscribeQueue.Delete();
        }

        private void UpdateMessage(string newSubscriber)
        {
            CloudQueueMessage message = subscribeQueue.GetMessage();
            message.SetMessageContent("Updated contents.");
            subscribeQueue.UpdateMessage(message,
                TimeSpan.FromSeconds(0.0),  // Make it visible immediately.
                MessageUpdateFields.Content | MessageUpdateFields.Visibility);
        }

        private void RetrieveMessage(string newSubscriber)
        {
            CloudQueueMessage message = subscribeQueue.GetMessage();            
        }
    }
}