using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Windows8Theme.AWS
{
   public abstract class AmazonBucket
    {
       public string ExistingBucketName;// = "*** Provide bucket name ***";
       public string KeyName;// = "*** Provide your object key ***";
       public string Filename;// = "*** Provide file name ***";
    }
}
