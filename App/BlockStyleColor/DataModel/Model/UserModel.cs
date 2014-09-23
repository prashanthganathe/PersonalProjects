using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows8Theme.DataModel
{
  
    public enum Rolename
    { 
        Acquirer,
        Expert,
        Administrator
    }
    
     public class UserModel
    {
        // Patient object contains following properties
        public string UserId { get; set; }        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Rolename UserRole { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public string Site { get; set; }

        private string _RoleImg;
        public string RoleImg
        {
            get;
            set;
            //get { return _RoleImg; }
            //set
            //{
            //    _RoleImg = "Assets/user.png"; ;
            //    if (this.UserRole == Rolename.Acquirer)
            //        _RoleImg = "Assets/acquire.png";
            //    if (this.UserRole == Rolename.Administrator)
            //        _RoleImg = "Assets/acquire.png";
            //    if (this.UserRole == Rolename.Expert)
            //        _RoleImg = "Assets/acquire.png";
            //}
        }

        private string _RoleString;
        public string RoleString
        {
            get;
            set;
            //get { return _RoleString; }
            //set
            //{
            //    value = "";
            //    if (this.UserRole == Rolename.Expert)
            //        _RoleString = "Expert";
            //    if (this.UserRole == Rolename.Administrator)
            //        _RoleString = "Administrator";
            //    if (this.UserRole == Rolename.Acquirer)
            //        _RoleString = "Acquirer";

            //}
        }
        private DateTime CreatedOn = new DateTime();
    }
}
