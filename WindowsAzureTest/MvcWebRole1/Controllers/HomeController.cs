using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;
using MvcWebRole.Models;
using MvcWebRole1.WrapperClass;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcWebRole1.Models;
using EncryptionDecryption;

namespace MvcWebRole1.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        //public ActionResult Index()
        //{
        //    return View();
        //}

        CloudTable mailingListTable;// It has to be removed

        public HomeController()
        {
        }

        private MailingList FindRow(string partitionKey, string rowKey)
        {
            var retrieveOperation = TableOperation.Retrieve<MailingList>(partitionKey, rowKey);
            var retrievedResult = mailingListTable.Execute(retrieveOperation);
            var mailingList = retrievedResult.Result as MailingList;
            if (mailingList == null)
            {
                throw new Exception("No mailing list found for: " + partitionKey);
            }

            return mailingList;
        }

        //
        // GET: /MailingList/

        public ActionResult Index()
        {
            TableRequestOptions reqOptions = new TableRequestOptions()
            {
                MaximumExecutionTime = TimeSpan.FromSeconds(1.5),
                RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(3), 3)
            };

            List<MailingList> lists;
            try
            {
                DataAccess dataLayer = new DataAccess();
                lists = dataLayer.RetrivesTables("RowKey");

            }
            catch (StorageException se)
            {
                ViewBag.errorMessage = "Timeout error, try again. ";
                Trace.TraceError(se.Message);
                return View("Error");
            }

            return View(lists);
        }

        //
        // GET: /MailingList/Create

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult CreateBlob()
        {
            return View();
        }

        public ActionResult CreateQueue()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateQueue(Subscriber subscriber)
        {
            if (ModelState.IsValid)
            {
                DataAccess dataLayer = new DataAccess();
                dataLayer.SaveQueue(subscriber.ListName);
                return RedirectToAction("Index");
            }

            return View(subscriber);
        }

        //
        // POST: /MailingList/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MailingList mailingList)
        {
            if (ModelState.IsValid)
            {
                using (MD5CryptoServiceExample md5CryptoService = new MD5CryptoServiceExample())
                {
                    MailingList encrptionMail = new MailingList()
                    {
                        Description = md5CryptoService.Encrypt(mailingList.Description, false),
                        FromEmailAddress = md5CryptoService.Encrypt(mailingList.FromEmailAddress, false),
                        ListName = mailingList.ListName,
                    };

                    DataAccess dataLayer = new DataAccess();
                    dataLayer.SaveTable(encrptionMail);
                }                

                return RedirectToAction("Index");

            }

            return View(mailingList);
        }


        //
        // GET: /MailingList/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBlob(string test123, HttpPostedFileBase test, HttpPostedFileBase file)
        {
            DataAccess data = new DataAccess();
            data.SaveBlob(DateTime.Now.ToString() + ".txt", file);

            return RedirectToAction("CreateBlob");
        }

        public ActionResult Edit(string partitionKey, string rowKey)
        {
            var mailingList = FindRow(partitionKey, rowKey);
            return View(mailingList);
        }

        //
        // POST: /MailingList/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string partitionKey, string rowKey, MailingList editedMailingList)
        {
            if (ModelState.IsValid)
            {
                var mailingList = new MailingList();
                UpdateModel(mailingList);
                try
                {
                    var replaceOperation = TableOperation.Replace(mailingList);
                    mailingListTable.Execute(replaceOperation);
                    return RedirectToAction("Index");
                }
                catch (StorageException ex)
                {
                    if (ex.RequestInformation.HttpStatusCode == 412)
                    {
                        // Concurrency error
                        var currentMailingList = FindRow(partitionKey, rowKey);
                        if (currentMailingList.FromEmailAddress != editedMailingList.FromEmailAddress)
                        {
                            ModelState.AddModelError("FromEmailAddress", "Current value: " + currentMailingList.FromEmailAddress);
                        }
                        if (currentMailingList.Description != editedMailingList.Description)
                        {
                            ModelState.AddModelError("Description", "Current value: " + currentMailingList.Description);
                        }
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                            + "was modified by another user after you got the original value. The "
                            + "edit operation was canceled and the current values in the database "
                            + "have been displayed. If you still want to edit this record, click "
                            + "the Save button again. Otherwise click the Back to List hyperlink.");
                        ModelState.SetModelValue("ETag", new ValueProviderResult(currentMailingList.ETag, currentMailingList.ETag, null));
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(editedMailingList);
        }

        //
        // GET: /MailingList/Delete/5

        public ActionResult Delete(string partitionKey, string rowKey)
        {
            var mailingList = FindRow(partitionKey, rowKey);
            return View(mailingList);
        }

        //
        // POST: /MailingList/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string partitionKey)
        {
            // Delete all rows for this mailing list, that is, 
            // Subscriber rows as well as MailingList rows.
            // Therefore, no need to specify row key.
            var query = new TableQuery<MailingList>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
            var listRows = mailingListTable.ExecuteQuery(query).ToList();
            var batchOperation = new TableBatchOperation();
            int itemsInBatch = 0;
            foreach (MailingList listRow in listRows)
            {
                batchOperation.Delete(listRow);
                itemsInBatch++;
                if (itemsInBatch == 100)
                {
                    mailingListTable.ExecuteBatch(batchOperation);
                    itemsInBatch = 0;
                    batchOperation = new TableBatchOperation();
                }
            }
            if (itemsInBatch > 0)
            {
                mailingListTable.ExecuteBatch(batchOperation);
            }
            return RedirectToAction("Index");
        }

    }
}
