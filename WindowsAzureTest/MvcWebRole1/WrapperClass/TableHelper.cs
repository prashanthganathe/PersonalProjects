using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using MvcWebRole.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MvcWebRole1.WrapperClass
{
    public class TableHelper : AzureClientWrapper  
    {
        
        private CloudTable mailingListTable;
        protected TableServiceContext tableContext;
        private CloudTableClient tableClient;


        public TableHelper(string storageAccountConnectionString)
            : base(storageAccountConnectionString)
        {

            tableClient = base.StorageAccount.CreateCloudTableClient();
            mailingListTable = tableClient.GetTableReference(ConfigurationManager.AppSettings["TableMailinglist"]);
            mailingListTable.CreateIfNotExists();
        }

        internal void SaveTable(MailingList mailingList)
        {
            var insertOperation = TableOperation.Insert(mailingList);
            mailingListTable.Execute(insertOperation);
        }

        internal void DeleteTable(MailingList mailingList)
        {
            var insertOperation = TableOperation.Delete(mailingList);
            mailingListTable.Execute(insertOperation);
        }

        internal List<MailingList> RetrivesTables(string rowKey)
        {
            TableRequestOptions reqOptions = new TableRequestOptions()
            {
                MaximumExecutionTime = TimeSpan.FromSeconds(1.5),
                RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(3), 3)
            };
            var query = new TableQuery<MailingList>().Where(TableQuery.GenerateFilterCondition(rowKey, QueryComparisons.Equal, ConfigurationManager.AppSettings["TableMailinglist"]));            
           return mailingListTable.ExecuteQuery(query, reqOptions).ToList();
        }

        internal void Update(MailingList mailingList)
        {
            var insertOperation = TableOperation.Replace(mailingList);
            mailingListTable.Execute(insertOperation);
        }

        internal void Retrieve(string partitionKey, string rowKey)
        {
            var insertOperation = TableOperation.Retrieve(partitionKey, rowKey);
            mailingListTable.Execute(insertOperation);
        }
    }
}