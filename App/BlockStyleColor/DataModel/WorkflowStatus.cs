using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows8Theme.DataModel
{
  
    public enum WorkflowStatus
    {
        NotInitiated=0 ,
        Initiated=1,
        Active=2,
        Unknown=3,
        Suspended=4,
        Rejected=5,
        Completed=6,
        Uploaded=7,
        Reviewed=8
    }
}
