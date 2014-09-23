using BlockStyleColor.EnableLiveTile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
using Windows8Theme;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace BlockStyleColor
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class AddUser : BlockStyleColor.Common.LayoutAwarePage
    {
        public AddUser()
        {
           this.InitializeComponent();
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
        //    http://localhost:8080/Carl/rest/newUser?first_name=Sharath&last_name=Kirani&
        //    email=s@s.com&address1=jadsbgbd&address2=dsfdsf&user_role=sdfsdf&
        //        user_name=dsafdsac&
        //        password=12345678&authToken=98a09a5b-889e-44f2-a412-44f4dc13fb8c&site
            App app;
            app = App.Current as App;

            UserREST objRest = new UserREST();
            objRest.address1 = this.txtaddress1.Text;
            objRest.address2 = this.txtaddress2.Text;
            objRest.email = this.txtemail.Text;
            objRest.first_name = this.txtFirstname.Text;
            objRest.last_name = this.txtlastname.Text;
            objRest.site = this.txtSite.Text;
            objRest.user_name = this.username.Text;
            objRest.password=this.txtpassword.Password;
            objRest.user_role = this.userrolecombo.SelectionBoxItem.ToString();
            //objRest.sex=this.combosex.SelectionBoxItem.ToString();
            objRest.status_flag = "1";    


            HttpClient client = new HttpClient();
            StringBuilder strb = new StringBuilder();
            strb.Append("address1=" + objRest.address1);
            strb.Append("&address2=" + objRest.address2);
            strb.Append("&email=" + objRest.email);
            strb.Append("&first_name=" + objRest.first_name);
            strb.Append("&last_name=" + objRest.last_name);
            strb.Append("&site=" + objRest.site);
            strb.Append("&user_name=" + objRest.user_name);
            strb.Append("&password=" + objRest.password);
            strb.Append("&user_role=" + objRest.user_role);
           // strb.Append("&sex=" + objRest.sex);
            strb.Append("&status_flag=" + objRest.status_flag);
            strb.Append("&authToken=" + app.AccessToken);
            HttpResponseMessage response = client.PostAsync(app.DomainName + "/rest/users/newUser?" + strb.ToString(), new StringContent("")).Result;
            if (response.IsSuccessStatusCode)
            {
                
            }
           
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void TextBlock_SelectionChanged_1(object sender, RoutedEventArgs e)
        {

        }

        private void TextBlock_SelectionChanged_2(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
