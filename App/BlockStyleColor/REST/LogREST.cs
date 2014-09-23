using BlockStyleColor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Windows8Theme.REST
{
    public  class LogREST
    {
        public static bool LogEvent(string EventName, string description, string username)
        {
            App app;
            app = App.Current as App;
            HttpClient client = new HttpClient();
            StringBuilder strb = new StringBuilder();
            strb.Append("name=" + EventName);
            strb.Append("&description=" + description);
            strb.Append("&username=" + username);
            HttpResponseMessage response = client.PostAsync(app.DomainName + "/rest/users/event?" + strb.ToString(), new StringContent("")).Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
                return false;   
        }
    }
}