using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows8Theme.DataModel
{
     
    public enum Gender
    {
        Male=1,
        Female=2
    }
    public class PatientModel
    {
        public string PatientID{get;set;}
        public string  FirstName{get;set;}
        public string LastName{get;set;}
        public DateTime DateOfBirth{get;set;}
        public Gender Sex{get;set;}
        public ImageModel LeftEye{get;set;}
        public ImageModel RightEye{get;set;}
        public DateTime CreatedOn{get;set;}
        public String Site { get; set; }
        public UserModel Acquirer{get;set;}
        public UserModel Reader { get; set; }
        public WorkflowStatus Status{get;set;}

        public string FullName { get; set; }
        
    }
}
