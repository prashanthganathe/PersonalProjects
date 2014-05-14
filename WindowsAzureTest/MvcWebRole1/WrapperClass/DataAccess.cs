using Microsoft.WindowsAzure.Storage.Table;
using MvcWebRole.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MvcWebRole1.WrapperClass
{
    public class DataAccess : IDisposable
    {
        string storageAccountConnectionString = ConfigurationManager.AppSettings["UseDevelopmentServer"];
        TableHelper tableDataSave;
        BlobHelper blobDataSave;
        QueueHelper queueDataSave;
        public DataAccess()
        {
            
        }
       
        internal void SaveTable(MailingList mailingList)
        {
            tableDataSave = new TableHelper(storageAccountConnectionString);
            tableDataSave.SaveTable(mailingList);
        }

        internal void DeleteTable(MailingList mailingList)
        {
            tableDataSave = new TableHelper(storageAccountConnectionString);
            tableDataSave.DeleteTable(mailingList);
        }

        internal List<MailingList> RetrivesTables(string rowKey)
        {
            tableDataSave = new TableHelper(storageAccountConnectionString);
            return tableDataSave.RetrivesTables(rowKey);
        }

        internal void SaveBlob(string blobName, HttpPostedFileBase httpPostedFile)
        {
            blobDataSave = new BlobHelper(storageAccountConnectionString);
            blobDataSave.Save(blobName, httpPostedFile);
           
        }

        private void UpdateBlob(string blobName, HttpPostedFileBase httpPostedFile)
        {
            blobDataSave = new BlobHelper(storageAccountConnectionString);
            blobDataSave.UpdateBlob(blobName, httpPostedFile);
            
        }

        internal void SaveQueue(string queueData)
        {
            queueDataSave = new QueueHelper(storageAccountConnectionString);
            queueDataSave.CreateQueueMessage(queueData, TimeSpan.FromDays(2));

        }        

        public void Dispose()
        {
           
        }
    }
}