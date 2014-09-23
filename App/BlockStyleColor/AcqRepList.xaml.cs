using BlockStyleColor.EnableLiveTile;
using IsolatedStorageW8;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Threading;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows8Theme;
using Windows8Theme.Common;
using Windows8Theme.DataModel;
using Windows8Theme.DataModel.Model;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace BlockStyleColor
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class AcqRepList : BlockStyleColor.Common.LayoutAwarePage
    {
        public static MainPage Current;
        App app;
        public static PatientREST selecteditem;
        
        public AcqRepList()
        {
            app = App.Current as App;
            this.InitializeComponent();
            BindGrid();
            //ItemGridView.ItemsSource = PatientDataModel.GetPatients();
           // lblmessage.Text = "";
      
        }

        public void BindGrid()
        {
            ItemGridView.ItemsSource = null;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(app.DomainName);
            StringBuilder strb = new StringBuilder();

            strb.Append("auth_token=" + app.AccessToken);
            strb.Append("&acquirer_id=" + app.UserId);
            strb.Append("&site=" + app.Site);
            strb.Append("&user_name=" + app.Username);
            HttpResponseMessage response = client.GetAsync("/rest/case/reviewedReports?" + strb.ToString()).Result;
            PatientREST patientrest;
            List<PatientREST> pinfo = new List<PatientREST>();
            var temp = "";
            if (response.IsSuccessStatusCode)
            {
                dynamic users = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                foreach (dynamic item in users)
                {
                    patientrest = new PatientREST();
                    foreach (dynamic i in item)
                    {

                        if (i.Name == "id")
                            patientrest.id = Convert.ToString(i.Value.Value);

                        if (i.Name == "patient_file_id")
                            patientrest.patient_file_id = Convert.ToString(i.Value.Value);
                        if (i.Name == "")
                            patientrest.patient_name = i.Value.Value;
                        if (i.Name == "")
                            patientrest.image_left_url = i.Value.Value;
                        if (i.Name == "imageRightUrl")
                        {
                            //patientrest.image_right_url = "ms-appdata:///roaming/Upload/" + i.Value.Value; //Source="ms-appdata:///roaming/upload/acq1_1.jpg"
                            var items = i.Value.Value;
                            string[] temparray = items.ToString().Split('/');
                            //   patientrest.image_right_url = "ms-appdata:///roaming/Upload/"+temparray[4];
                            if (app.UserRole == "acquirer")
                                patientrest.image_right_url = "ms-appdata:///roaming/Upload/" + temparray[4];
                            if (app.UserRole == "expert")
                                patientrest.image_right_url = "ms-appdata:///roaming/Download/" + temparray[4];

                            patientrest.FileName = temparray[4];
                        }

                        if (i.Name == "")
                            patientrest.acquirer_user_id = i.Value.Value;
                        if (i.Name == "")
                            patientrest.auth_token = i.Value.Value;
                        if (i.Name == "")
                            patientrest.site = i.Value.Value;
                        if (i.Name == "")
                            patientrest.workflow_status = i.Value.Value;
                        if (i.Name == "sex")
                            patientrest.sex = i.Value.Value;
                        if (i.Name == "age")
                            patientrest.age = i.Value.Value;
                        if (i.Name == "dateOfBirth")
                            patientrest.dob = i.Value.Value;
                        if (i.Name == "aflag")
                            patientrest.aflag = i.Value.Value;
                        if (i.Name == "bflag")
                            patientrest.bflag = i.Value.Value;
                        if (i.Name == "cflag")
                            patientrest.cflag = i.Value.Value;
                        if (i.Name == "dflag")
                            patientrest.dflag = i.Value.Value;
                        if (i.Name == "acomment")
                            patientrest.acomment = i.Value.Value;
                        if (i.Name == "bcomment")
                            patientrest.bcomment = i.Value.Value;
                        if (i.Name == "ccomment")
                            patientrest.ccomment = i.Value.Value;
                        if (i.Name == "dcomment")
                            patientrest.dcomment = i.Value.Value;
                        if (i.Name == "")
                            patientrest.userId = Convert.ToString(i.Value.Value);
                        if (i.Name == "createdAt")
                            patientrest.CreatedOn = Convert.ToString(i.Value.Value);
                        if (i.Name == "reviewed_date")
                            patientrest.ReviewedDate = Convert.ToString(i.Value.Value);

                    }
                    pinfo.Add(patientrest);
                }
              
            }
            ItemGridView.ItemsSource = null;
            ItemGridView.ItemsSource = pinfo;

        }



        private void Grid_Tapped_info(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(ItemSubPage));
            var model = (PatientREST)((FrameworkElement)sender).DataContext;
            selecteditem = model;
            this.Frame.Navigate(typeof(AcReport));
        }












        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            CreateLiveTile.ShowliveTile(true, "BlockStyleColor Template");
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void Grid_Tapped_1(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ItemSubPage));
        }

        private void Grid_Tapped_Login(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(ItemSubPage));
            //this.Frame.Navigate(typeof(Login));
        }

        private void Grid_Tapped_Audience(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AudienceInfo));
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void pageTitle_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

       

        public UserModel ValidateUser(string username, string pwd)
        {
           return UsersDataModel.ValidateUser(username, pwd);
        }

        private void ItemGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //private void Grid_Tapped_info(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        //{
        //   // this.Frame.Navigate(typeof(RequestQueue));
          
        //    var model = (InitialPatientInfo)((FrameworkElement)sender).DataContext;
        //    selecteditem = model;
        //    this.Frame.Navigate(typeof(ExpertsResponse));
        //}
        

        
    }
}
