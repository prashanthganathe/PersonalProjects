using BlockStyleColor.EnableLiveTile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows8Theme.DataModel;
using Newtonsoft;
using Newtonsoft.Json;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace BlockStyleColor
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : BlockStyleColor.Common.LayoutAwarePage
    {
        public static MainPage Current;
        App app;
        public MainPage()
        {
           this.InitializeComponent();
           app = App.Current as App;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           // this.Frame.Navigate(typeof(UserList));
            //this.Frame.Navigate(typeof(Response));
            string username = this.txtusername.Text;
            string password = this.txtpassword.Password;
            var loggeduser = ValidateUser(username, password);
          //  this.Frame.Navigate(typeof(AdminHome));
            if (loggeduser != null)
            {
                switch (app.UserRole)
                {
                    case "admin":
                        {
                            this.Frame.Navigate(typeof(AdminHome));
                            break;
                        }
                    case "acquirer":
                        {
                            this.Frame.Navigate(typeof(AcquirerHome));
                            break;
                        }
                    case "expert":
                        {
                            this.Frame.Navigate(typeof(ExpertHome));
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

            }
        }

        public async void CreateFolder()
        {
            try
            {
                Windows.Storage.StorageFolder store = await Windows.Storage.ApplicationData.Current.RoamingFolder.CreateFolderAsync("Upload",Windows.Storage.CreationCollisionOption.OpenIfExists);
                Windows.Storage.StorageFolder store2 = await Windows.Storage.ApplicationData.Current.RoamingFolder.CreateFolderAsync("Download", Windows.Storage.CreationCollisionOption.OpenIfExists);
              
            }
            catch(Exception ex)
            {
               
            }
            
        }

        public UserModel ValidateUser(string username, string pwd)
        {
            app.AccessToken = app.Site= app.Username = app.UserRole = null;
            app.UserId = 0;



           HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(app.DomainName);
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync("/rest/users/authenticate?username="+this.txtusername.Text+"&password="+this.txtpassword.Password).Result;
            if (response.IsSuccessStatusCode)
            {
                CreateFolder();
                dynamic users = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                foreach(dynamic item in users)
                {
                    //JsonConvert.DeserializeObject(item.Value);
                    if (item.Name == "auth_token")
                        app.AccessToken = item.Value.Value;
                    if (item.Name == "site")
                        app.Site = item.Value.Value;
                    if (item.Name == "userid")
                        app.UserId = item.Value.Value;
                    if (item.Name == "username")
                        app.Username = item.Value.Value;
                    if (item.Name == "user_role")
                        app.UserRole = item.Value.Value;
                    app.Password = this.txtpassword.Password;
                  
                }
                if (app.IsInternet())
                {
                   // if (app.UserRole == "acquirer")
                    app.AutoStart();
                }
            }
            return new UserModel();
        }        
    }
}
