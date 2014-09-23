using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows8Theme.DataModel
{
    public class LogModel
    {
        public UserModel   User{get;set;}
        public PatientModel Patient{get;set;}
        public ActivityType Activity {get;set;} 
        public DateTime EventTime{get;set;}
    }
}
