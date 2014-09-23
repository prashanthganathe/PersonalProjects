using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows8Theme.DataModel.Interface
{
    public interface IPerson
    {
        string PersonId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        DateTime DOB { get; set; }
        string Usernaem { get; set; }
        string Password { get; set; }
        DateTime CreatedOn { get; set; }
        
    }

    public interface ISite
    {
        string SiteName { get; set; }
    }
}
