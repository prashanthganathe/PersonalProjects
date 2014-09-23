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
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using BlockStyleColor;
using System.Net;
using Windows8Theme.DataModel;
using Windows8Theme.REST;
using Windows.Storage.Search;

namespace Windows8Theme.Common
{
    public class AutoUpload
    {

        App app;
        public AutoUpload()
        {
            app = App.Current as App;
        }
       
        public static string ExistingBucketName = "TestingUploadS3";
        public static string S3url = "https://s3-us-east-1.amazonaws.com/";
        
        private readonly int MB_SIZE = (int)Math.Pow(2, 20);
        public static double currentStatus;


        public static List<PatientData> patientList = new List<PatientData>();
        public static string StartTime { get; set; }
        public static string EndTime { get; set; }


        public async Task<IReadOnlyList<StorageFile>> GetUploadingFileList()
        {
           // StorageFolder picturesFolder = KnownFolders.PicturesLibrary;
            //Windows.Storage.StorageFolder upload = await Windows.Storage.ApplicationData.Current.RoamingFolder.CreateFolderAsync("Upload",CreationCollisionOption.ReplaceExisting);
            //Windows.Storage.StorageFolder download = await Windows.Storage.ApplicationData.Current.RoamingFolder.CreateFolderAsync("Download", CreationCollisionOption.ReplaceExisting);
          // IReadOnlyCollection<Windows.Storage.StorageFolder> storea =await picturesFolder.GetFoldersAsync();
            Windows.Storage.StorageFolder store = await Windows.Storage.ApplicationData.Current.RoamingFolder.GetFolderAsync("Upload");
           // Windows.Storage.Search.CommonFileQuery search = new Windows.Storage.Search.CommonFileQuery();
             return await store.GetFilesAsync();
            //await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("example.txt");

             //List<string> fileTypeFilter = new List<string>();
             //fileTypeFilter.Add(".jpg");
             //fileTypeFilter.Add(".png");
             //fileTypeFilter.Add(".bmp");
             //fileTypeFilter.Add(".gif");
             //var queryOptions = new QueryOptions(CommonFileQuery.OrderByName, fileTypeFilter);
             //var query = KnownFolders.PicturesLibrary.CreateFileQueryWithOptions(queryOptions);
             //IReadOnlyList<StorageFile> fileList = await query.GetFilesAsync();

        }

        public  void DeletePartiallyUploadedItems()
        {
            int a=  patientList.RemoveAll(x => x.Image1.Status < 100);
            
        }

      
        public async Task InitiateFileUploading()
        {
           // patientList = new List<PatientData>();
            StartTime = DateTime.Now.ToString();
            PatientData PatientDataObj=null;

            var storage = new Setting<List<PatientData>>();           
            IReadOnlyList<StorageFile> files = await GetUploadingFileList();
            var currentUploadCount = new Setting<int>();


            DeletePartiallyUploadedItems();
            int count = 0;
            //var temp = patientList.Select(p => p.Image1.Name).Except(files.Select(x=>x.Name));
             IEnumerable<string> filesTemp;
             if (patientList.Count > 0)
                 filesTemp = files.Select(p => p.Name).Except(patientList.Select(x => x.Image1.Name));
             else
                 filesTemp = files.Select(p => p.Name);

            int newfilecount = 0;
            foreach (var file in files)
            {

                foreach (var newfiles in filesTemp)
                {

                    if (newfiles == file.Name)
                    {
                        if (newfiles.Contains(app.Username))
                        {
                            newfilecount = newfilecount + 1;
                            PatientDataObj = new PatientData();
                            PatientDataObj.ID = count;
                            PatientDataObj.Image1 = new FileUploadCompleted();
                            PatientDataObj.Image1.Name = file.Name;
                            PatientDataObj.Image1.Status = 0;
                            PatientDataObj.Image1.Url = app.NormalBucketURL + file.Name;
                            PatientDataObj.Age = count + 20;
                            PatientDataObj.Sex = count % 2 == 0 ? DataModel.Gender.Male : DataModel.Gender.Female;
                            var properties = await file.GetBasicPropertiesAsync();
                            PatientDataObj.Image1.Size = Convert.ToInt32(properties.Size) / 1000000;
                            patientList.Add(PatientDataObj);
                            await Task.WhenAll(Task.Factory.StartNew(() => UploadFile(file.Name, file, PatientDataObj.Image1)));
                            ////await Task.WhenAll(Task.Run(() => UploadFile(file.Name, file, PatientDataObj.Image1)));   
                            //using (var semaphore = new SemaphoreSlim(MAX_PARALLEL_UPLOADS))
                            //{
                            //}
                        }
                    }

                  }


                  await storage.SaveAsync("UploadStat", patientList);
                  await currentUploadCount.SaveAsync("CurrentUploadCount", newfilecount);  
                  count = count + 1;
                }
            AutoDownload downloadObj = new AutoDownload();
            downloadObj.InitializeDownload();
        }

        //public async Task  UploadFileAsync(string name, IStorageFile storageFile, FileUploadCompleted fileinfo)
        //{
        //   await  Task.Factory.StartNew(() => UploadFile( name,  storageFile,  fileinfo));

        //}

        public async void UploadFile(string name, IStorageFile storageFile, FileUploadCompleted fileinfo)
        {
            fileinfo.UploadStartTime = DateTime.Now;
            BasicAWSCredentials credentials = new BasicAWSCredentials("AKIAJTADDHY7T7GZXX5Q", "n4xV5B25mt7e6br84H2G9SXBx8eDYTQJgCxoaF49");
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
                        PartSize = 10 * MB_SIZE,
                        CannedACL = S3CannedACL.PublicRead                       
                    };
                   
                    uploadRequest.UploadProgressEvent += OnUploadProgressEvent;
                   // fileinfo.Status = currentStatus.ToString();
                    try
                    {
                        await transferUtility.UploadAsync(uploadRequest);
                    }
                    catch
                    {

                    }
                   // fileinfo.UploadEndtime = DateTime.Now;
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                    ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                //    Console.WriteLine("Check the provided AWS Credentials.");
                //    Console.WriteLine(
                //        "For service sign up go to http://aws.amazon.com/s3");
                }
                else
                {
                    //Console.WriteLine(
                    //    "Error occurred. Message:'{0}' when writing an object"
                    //    , amazonS3Exception.Message);
                }
            }
        }

        public void OnUploadProgressEvent(object sender, UploadProgressArgs e)
        {
            var currentobject = sender as Amazon.S3.Transfer.TransferUtilityUploadRequest;
            var s = patientList.Where(p => p.Image1.Name == currentobject.Key).FirstOrDefault();
            if (s != null)
            {
                if (e.PercentDone > 1)
                    s.Image1.Status = e.PercentDone;// +"%";
                if (e.PercentDone == 100)
                {
                   // LogREST.LogEvent("UploadEvent","File +"s.Image1.Name+" uploaded.", app.Username);
                    //LogREST objlog=new  LogREST();
                    LogREST.LogEvent("UploadCompleted", "File " + s.Image1.Name + " uploaded.", app.Username);
                    CreatePatient(s);
                    s.Image1.UploadEndtime = DateTime.Now;
                    s.Image1.Status = e.PercentDone;
                    EndTime = DateTime.Now.ToString();

                }

            }
        }


        public bool CreatePatient(PatientData s)
        {
            PatientREST obj = new PatientREST();
            obj.auth_token = app.AccessToken;
            obj.acquirer_user_id = s.ID.ToString();
            s.Image1.Name = s.Image1.Name.Replace(" ", "+");
            obj.image_left_url = app.NormalBucketURL +"/"+ s.Image1.Name;
            obj.image_right_url = app.NormalBucketURL+"/"+s.Image1.Name;
            obj.patient_file_id = s.ID.ToString();
            obj.patient_name = app.Username;

            obj.site = app.Site;


            HttpClient client = new HttpClient();
            StringBuilder strb = new StringBuilder();
            strb.Append("auth_token=" + obj.auth_token);
            strb.Append("&acquirer_user_id=" + app.UserId);
            strb.Append("&image_left_url=" + obj.image_left_url);
            strb.Append("&image_right_url=" + obj.image_right_url);
            strb.Append("&patient_file_id=" + obj.patient_file_id);
            strb.Append("&patient_name=" + obj.patient_name);
            strb.Append("&site=" + obj.site);
            strb.Append("&workflow_status=" + ActivityType.ImageUploadCompleted);
            strb.Append("&age=" + "21");
            strb.Append("&sex=" + Gender.Female);
            strb.Append("&user_name=" + app.Username);
           // StringContent con = new StringContent("");
          


            HttpResponseMessage response = client.PostAsync(app.DomainName + "/rest/case/new?" + strb.ToString(), new StringContent("")).Result;
            if (response.IsSuccessStatusCode)
            {
                LogREST.LogEvent("MetaData Upload info", "File " + s.Image1.Name + " uploaded.", app.Username);
                //dynamic users = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                // app.AccessToken=
                //foreach (string typeStr in users.Type[0])
                //{

                //}

            }
            
            return true;
        }


            
        }
    }

