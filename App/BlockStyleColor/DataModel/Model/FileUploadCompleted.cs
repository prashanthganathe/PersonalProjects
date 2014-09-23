using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows8Theme.DataModel.Model
{
    public class DownloadFile
    {
        public DownloadFile()
        {
            Status = 0;
        }
       
        public string ID { get; set; }
        public string Name { get; set; }
        public double Size { get; set; }
        public int Status { get; set; }
        public DateTime UploadStartTime { get; set; }
        public DateTime UploadEndtime { get; set; }
         
    }


    public class PatientData
    {
        public FileUploadCompleted Image1 { get; set; }        
        public int ID { get; set; }
        public decimal Age { get; set; }
        public Gender Sex { get; set; }        
    }


    public class FileUploadCompleted
    {
        public FileUploadCompleted()
        {
            Status = 0;
        }
        public string Name { get; set; }
        public String Description { get; set; }
        public DateTime UploadStartTime { get; set; }
        public DateTime UploadEndtime { get; set; }
        public int Size { get; set; }
        public string Url { get; set; }
        public int Status { get; set; }
    }
}
