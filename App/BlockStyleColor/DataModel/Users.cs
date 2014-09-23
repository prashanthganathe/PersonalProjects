using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows8Theme.DataModel
{
    public class Users
    {
         public Users() { }
         public ObservableCollection<UserModel> FilteredUsers { get; set; }
    }
}
