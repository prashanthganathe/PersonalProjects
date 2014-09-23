using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows8Theme.DataModel
{
    public class UsersDataModel
    {
        private static bool isDataAdded = false;
        private static ObservableCollection<UserModel> _users_data = new ObservableCollection<UserModel>();

        public static void FillData()
        {
            if (!isDataAdded)
            {
                _users_data.Add(new UserModel { UserId = "001", FirstName = "User1", LastName = "LName1", UserRole = Rolename.Acquirer, UserName = "Username1", Password = "Test123", Site ="Site1" });
                _users_data.Add(new UserModel { UserId = "002", FirstName = "User2", LastName = "LName2", UserRole = Rolename.Acquirer, UserName = "Username2", Password = "Test123" , Site="Site1"});
                _users_data.Add(new UserModel { UserId = "003", FirstName = "User3", LastName = "LName3", UserRole = Rolename.Acquirer, UserName = "Username3", Password = "Test123", Site="Site1" });

                _users_data.Add(new UserModel { UserId = "004", FirstName = "User4", LastName = "LName4", UserRole = Rolename.Administrator, UserName = "Username4", Password = "Test123", Site = "Site2" });
                _users_data.Add(new UserModel { UserId = "005", FirstName = "User5", LastName = "LName5", UserRole = Rolename.Administrator, UserName = "Username5", Password = "Test123", Site = "Site2" });
                _users_data.Add(new UserModel { UserId = "006", FirstName = "User6", LastName = "LName6", UserRole = Rolename.Administrator, UserName = "Username6", Password = "Test123", Site = "Site2" });

                _users_data.Add(new UserModel { UserId = "007", FirstName = "User7", LastName = "LName7", UserRole = Rolename.Expert, UserName = "Username7", Password = "Test123", Site = "Site3" });
                _users_data.Add(new UserModel { UserId = "008", FirstName = "User8", LastName = "LName8", UserRole = Rolename.Expert, UserName = "Username8", Password = "Test123", Site = "Site3" });
                _users_data.Add(new UserModel { UserId = "009", FirstName = "User9", LastName = "LName9", UserRole = Rolename.Expert, UserName = "Username9", Password = "Test123", Site = "Site3" });
            }
          
        }

        public static ObservableCollection<UserModel> GetUsers()
        {
            FillData();
            return _users_data;
        }

        public static ObservableCollection<UserModel> UsersData
        {
            get
            {
                  FillData();
                  return new ObservableCollection<UserModel>();
            }
            
        }

        public static UserModel ValidateUser(string username, string password)
        {
            FillData();
            return _users_data.Where(p => p.UserName == username && p.Password == password).FirstOrDefault();
        }


        public static ObservableCollection<UserModel> GetUsersByType(Rolename rolename)
        {
            ObservableCollection<UserModel> users = new ObservableCollection<UserModel>();
            List<UserModel> filteredUsers = _users_data.Where(p => p.UserRole == rolename).ToList();
            foreach (UserModel usr in filteredUsers)
            {
                if(usr!=null)
                users.Add(usr);
            }
            return users;
        }
    }
}
