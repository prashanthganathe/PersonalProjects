using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows8Theme.DataModel
{
    public class PatientDataModel
    {
        private static bool isDataAdded = false;
        public static ObservableCollection<PatientModel> _patients_data = new ObservableCollection<PatientModel>();
        public static void FillData()
        {
            if (!isDataAdded)
            {
                _patients_data.Add(new PatientModel { PatientID = "001", FirstName = "User1", LastName = "LName1", FullName = "FullName1", DateOfBirth = new DateTime(1980, 3, 21), Site = "Site1",Status= WorkflowStatus.Uploaded, RightEye = new ImageModel { Url = "Assets/right.jpg" }, LeftEye = new ImageModel { Url = "Assets/left.jpg" } });
                _patients_data.Add(new PatientModel { PatientID = "002", FirstName = "User2", LastName = "LName2", FullName = "FullName2", DateOfBirth = new DateTime(1981, 3, 21), Site = "Site1", Status = WorkflowStatus.Uploaded, RightEye = new ImageModel { Url = "Assets/right.jpg" }, LeftEye = new ImageModel { Url = "Assets/left.jpg" } });
                _patients_data.Add(new PatientModel { PatientID = "003", FirstName = "User3", LastName = "LName3", FullName = "FullName3", DateOfBirth = new DateTime(1982, 3, 21), Site = "Site1", Status = WorkflowStatus.Uploaded, RightEye = new ImageModel { Url = "Assets/right.jpg" }, LeftEye = new ImageModel { Url = "Assets/left.jpg" } });

                _patients_data.Add(new PatientModel { PatientID = "004", FirstName = "User4", LastName = "LName4", FullName = "FullName4", DateOfBirth = new DateTime(1983, 3, 22), Site = "Site2", Status = WorkflowStatus.Uploaded, RightEye = new ImageModel { Url = "Assets/right.jpg" }, LeftEye = new ImageModel { Url = "Assets/left.jpg" } });
                _patients_data.Add(new PatientModel { PatientID = "005", FirstName = "User5", LastName = "LName5", FullName = "FullName5", DateOfBirth = new DateTime(1984, 3, 22), Site = "Site2", Status = WorkflowStatus.Uploaded, RightEye = new ImageModel { Url = "Assets/right.jpg" }, LeftEye = new ImageModel { Url = "Assets/left.jpg" } });
                _patients_data.Add(new PatientModel { PatientID = "006", FirstName = "User6", LastName = "LName6", FullName = "FullName6", DateOfBirth = new DateTime(1985, 3, 22), Site = "Site2", Status = WorkflowStatus.Uploaded, RightEye = new ImageModel { Url = "Assets/right.jpg" }, LeftEye = new ImageModel { Url = "Assets/left.jpg" } });

                _patients_data.Add(new PatientModel { PatientID = "007", FirstName = "User7", LastName = "LName7", FullName = "FullName7", DateOfBirth = new DateTime(1986, 3, 23), Site = "Site3", Status = WorkflowStatus.Reviewed, RightEye = new ImageModel { Url = "Assets/right.jpg" }, LeftEye = new ImageModel { Url = "Assets/left.jpg" } });
                _patients_data.Add(new PatientModel { PatientID = "008", FirstName = "User8", LastName = "LName8", FullName = "FullName8", DateOfBirth = new DateTime(1987, 3, 23), Site = "Site3", Status = WorkflowStatus.Reviewed, RightEye = new ImageModel { Url = "Assets/right.jpg" }, LeftEye = new ImageModel { Url = "Assets/left.jpg" } });
                _patients_data.Add(new PatientModel { PatientID = "009", FirstName = "User9", LastName = "LName9", FullName = "FullName9", DateOfBirth = new DateTime(1988, 3, 23), Site = "Site3", Status = WorkflowStatus.Reviewed, RightEye = new ImageModel { Url = "Assets/right.jpg" }, LeftEye = new ImageModel { Url = "Assets/left.jpg" } });
            }
        }

        public static ObservableCollection<PatientModel> GetPatients()
        {
            FillData();
            return _patients_data;
        }


        }

    }
    


