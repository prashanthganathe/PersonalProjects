using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows8Theme.DataModel.Interface;

namespace Windows8Theme.DataModel.Abstract
{
    public class Person: IPerson
    {
        public string PersonId { get; set { value = new Guid().ToString(); } }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DOB { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime CreatedOn { get; set { value = DateTime.Now; } }

        public int getAge()
        {
            if (DOB != null)
                return DateTime.Now.Year - DOB.Year;
            else
                return -1;
        }

        public string getFullName()
        {
            return FirstName + " " + LastName;
        }
   }


}
