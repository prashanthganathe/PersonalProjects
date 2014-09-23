﻿using BlockStyleColor.Common;
using BlockStyleColor.SettingsFlyouts;
using IsolatedStorageW8;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.System.Threading;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows8Theme.Common;
using Windows8Theme.DataModel.Model;


using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

// The Grid App template is documented at http://go.microsoft.com/fwlink/?LinkId=234226

namespace BlockStyleColor
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {

      

        public string RedirectUri { get; set; }
        public string DomainName { get; set; }
        public string ClientID { get; set; }
        public string Resource { get; set; }
        public string Code { get; set; }
        public string AccessToken { get; set; }
        public long UserId { get; set; }
        public string Site { get; set; }
        public string Username { get; set; }
        public string Password{get;set;}
        public string UserRole { get; set; }
        public string NormalBucketURL { get; set; }   
        double settingsWidth = 370;
        Popup settingsPopup;
        private readonly int Minutes = 3;
        public const int MAX_PARALLEL_UPLOADS = 5;
    

        /// <summary>
        /// Initializes the singleton Application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public  App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            RedirectUri = "http://whatevah";
            DomainName = "http://cloud-prototype.elasticbeanstalk.com";
       // http://carlzeiss-22eez2iyng.elasticbeanstalk.com";//"treyresearch1.onmicrosoft.com";

            //http://carlzeiss-22eez2iyng.elasticbeanstalk.com/rest/users/authenticate?username=abcd&password=123456789
            ClientID = "4a491cff-73a9-4a45-b274-b5836a723b14";
            Resource = "http://localhost:8643/";
            Code = AccessToken = string.Empty;
            NormalBucketURL = "https://s3.amazonaws.com/TestingUploadS3";          
           
           //  temp();
           // loadAccessToken();            
           // AutoStart();

            //NetworkUtility obj = new NetworkUtility();
            //ConnectionProfile prof = new ConnectionProfile();
            //var dataUsage = connectionProfile.getLocalUsage(StartTime, EndTime, States);
            //string ts = obj.GetConnectionProfile(prof);

            ThreadPoolTimer threadPoolTimer = ThreadPoolTimer.CreatePeriodicTimer((source) =>
            {
                if (AccessToken!=string.Empty || AccessToken!=null)
                {
                    if (IsInternet())
                    {
                       
                             AutoStart();
                    }
                }               
            }, TimeSpan.FromMinutes(Minutes));        
        }

        //public void loadAccessToken()
        //{
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri(DomainName);
        //    // Add an Accept header for JSON format.
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    //strb.Append("&user_name=" + app.Username);
        //    HttpResponseMessage response = client.GetAsync("/rest/users/authenticate?username="+this.UserId+"&password="+this.Password+"&user_name=" + this.Username).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        dynamic users = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
        //        foreach (dynamic item in users)
        //        {
        //            //JsonConvert.DeserializeObject(item.Value);
        //            if (item.Name == "auth_token")                   
        //                AccessToken = item.Value.Value;
        //            if (item.Name == "site")
        //                Site = item.Value.Value;
        //            if (item.Name == "userid")
        //                UserId = item.Value.Value; 
        //             if (item.Name == "username")
        //                Username = item.Value.Value;
        //             if (item.Name == "user_role")
        //                UserRole=item.Value.Value;                    
        //        }
        //    }           
        //}

        public  bool IsInternet()
        {
            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            return internet;
        }

        public async void temp()
        {
            //var storage = new Setting<List<PatientData>>();
            //await storage.SaveAsync("UploadStat", null);
        }

        public  async void   AutoStart()
        {
            try
            {
                if (this.UserId != 0)
                {
                    AutoUpload uploadObj = new AutoUpload();
                    await uploadObj.InitiateFileUploading();
                    //string a= uploadObj.currentStatus;
                    AutoDownload downloadObj = new AutoDownload();
                    downloadObj.InitializeDownload();
                   
                }
            }
            catch
            {

            }
        }

    

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            // Do not repeat app initialization when already running, just ensure that
            // the window is active
            if (args.PreviousExecutionState == ApplicationExecutionState.Running)
            {
                Window.Current.Activate();

                return;
            }

            // Create a Frame to act as the navigation context and associate it with
            // a SuspensionManager key
            var rootFrame = new Frame();
            SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                // Restore the saved session state only when appropriate
                await SuspensionManager.RestoreAsync();
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), "AllGroups"))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Place the frame in the current Window and ensure that it is active
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }
        
        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            SettingsPane.GetForCurrentView().CommandsRequested += App_CommandsRequested;
        }

        void App_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            UICommandInvokedHandler handler = new UICommandInvokedHandler(onSettingsCommand);
            SettingsCommand aboutCommand = new SettingsCommand("AU", "About Us", handler);
            args.Request.ApplicationCommands.Add(aboutCommand);

            SettingsCommand contactuCommand = new SettingsCommand("CU", "Contact Us", handler);
            args.Request.ApplicationCommands.Add(contactuCommand);

            SettingsCommand privacyCommand = new SettingsCommand("PP", "Privacy Policy", handler);
            args.Request.ApplicationCommands.Add(privacyCommand);

            SettingsCommand termsCommand = new SettingsCommand("TC", "Terms and Conditions", handler);
            args.Request.ApplicationCommands.Add(termsCommand);
        }

        private void onSettingsCommand(IUICommand command)
        {
            Rect windowBounds = Window.Current.Bounds;
            settingsPopup = new Popup();
            settingsPopup.Closed += settingsPopup_Closed;
            Window.Current.Activated += Current_Activated;
            settingsPopup.IsLightDismissEnabled = true;
            Page settingPage = null;

            switch (command.Id.ToString())
            {
                case "AU":
                    settingPage = new AboutUs();
                    break;
                case "CU":
                    settingPage = new ContactUs();
                    break;
                case "PP":
                    settingPage = new PrivacyPolicy();
                    break;
                case "TC":
                    settingPage = new TermsAndConditions();
                    break;
            }
            settingsPopup.Width = settingsWidth;
            settingsPopup.Height = windowBounds.Height;

            // Add the proper animation for the panel.
            settingsPopup.ChildTransitions = new TransitionCollection();
            settingsPopup.ChildTransitions.Add(new PaneThemeTransition()
            {
                Edge = (SettingsPane.Edge == SettingsEdgeLocation.Right) ?
                       EdgeTransitionLocation.Right :
                       EdgeTransitionLocation.Left
            });
            if (settingPage != null)
            {
                settingPage.Width = settingsWidth;
                settingPage.Height = windowBounds.Height;
            }

            // Place the SettingsFlyout inside our Popup window.
            settingsPopup.Child = settingPage;

            // Let's define the location of our Popup.
            settingsPopup.SetValue(Canvas.LeftProperty, SettingsPane.Edge == SettingsEdgeLocation.Right ? (windowBounds.Width - settingsWidth) : 0);
            settingsPopup.SetValue(Canvas.TopProperty, 0);
            settingsPopup.IsOpen = true;
        }

        void Current_Activated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.Deactivated)
            {
                settingsPopup.IsOpen = false;
            }
        }

        void settingsPopup_Closed(object sender, object e)
        {
            Window.Current.Activated -= Current_Activated;
        }
    }
}