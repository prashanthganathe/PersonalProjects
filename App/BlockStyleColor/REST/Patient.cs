using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows8Theme
{


    //    http://localhost:8080/Carl/rest/newUser?first_name=Sharath&last_name=Kirani&
    //    email=s@s.com&address1=jadsbgbd&address2=dsfdsf&user_role=sdfsdf&
    //        user_name=dsafdsac&
    //        password=12345678&authToken=98a09a5b-889e-44f2-a412-44f4dc13fb8c


    public class UserREST
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string user_role { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        public string site { get; set; }
        public string sex { get; set; }
        public string status_flag { get; set; }
    }
    public class PatientREST
    {
      public string id { get; set; }
      public string   patient_file_id{get;set;}
      public string patient_name{get;set;}
      public string  image_left_url{get;set;}
      public string image_right_url{get;set;}
      public string acquirer_user_id{get;set;}
      public string auth_token { get; set; }
      public string site { get; set; }
      public string workflow_status { get; set; }
      public string sex { get; set; }
      public string age { get; set; }
      public string dob { get; set; }
      public bool aflag { get; set; }
      public bool bflag { get; set; }
      public bool cflag { get; set; }
      public bool dflag { get; set; }
      public string acomment { get; set; }
      public string bcomment { get; set; }
      public string ccomment { get; set; }
      public string dcomment { get; set; }
      public string userId { get; set; }
      public string CreatedOn { get; set; }
      public string ReviewedDate { get; set; }
      public string FileName { get; set; }

    }

    public class InitialPatientInfo
    {
        public string dateOfBirth { get; set; }
        public string id { get; set; }
        public string imageRightUrl{get;set;}//": "https://s3-us-east-1.amazonaws.com/TestingUploadS3/AVTPike145B_JpgImage_Bright2_01-01-1901__(0000).jpg",
        public string sex { get; set; }//"sex": "Female",
        public string createdAt{get;set;}//": "2014-08-07",
        public string age{get;set;}//": "201",
        public string imageLeftUrl { get; set; }
        public string workflow_status { get; set; }
        public string FileName { get; set; }
    }
}
