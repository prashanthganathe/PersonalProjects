using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Exchange.WebServices;
using Microsoft.Exchange.WebServices.Data;
using System.Configuration;
using System.Globalization;

namespace Cloudbearing.TimeOffRequestWeb
{
    public class EWSClass
    {
        public bool SetupCalendarEvent(string msg, string srtdate, string enddate, string[] rqdAttendeelist, string[] optionalAttendeelist, string UserID, string UserPassword,string workinghours)
        {
            EWSClass objEWS = new EWSClass();
            ExchangeService objService = objEWS.CreateService(UserID, UserPassword,workinghours);
            IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);
            //DateTime startdate = DateTime.ParseExact(srtdate, "g", culture, DateTimeStyles.None);
            //DateTime endDate = DateTime.ParseExact(enddate, "g", culture, DateTimeStyles.None);
            DateTime startdate = DateTime.Parse(srtdate);
            DateTime endDate = DateTime.Parse(enddate);
            Appointment objAppoint = objEWS.GetAppointment(objService, msg, startdate, endDate);

            return objEWS.CreateCalendarEvent(objService, objAppoint,rqdAttendeelist,optionalAttendeelist);
        }

        public Appointment GetAppointment(ExchangeService objService, string msg, DateTime startdate, DateTime enddate)
        {
            Appointment appointment = new Appointment(objService);
            appointment.Subject = msg;
            appointment.Location = "Office365";
            appointment.Start = startdate;          
            appointment.End = enddate;
            appointment.Body = msg;
            appointment.IsReminderSet = false;
            appointment.IsResponseRequested = false;
            return appointment;
        }

        public ExchangeService CreateService(string UserID, string UserPassword, string workinghours)
        {
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2013);
            service.Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");
            try
            {
                service.UseDefaultCredentials = false;

                string workingHrs =workinghours;///default from web.config



                string sendUserMailAddress = UserID;// ConfigSettings.settings["SenderEmail"];// Config.SenderEmail;
                string sendUserMailPwd = UserPassword;// ConfigSettings.settings["SenderPassword"]; //Config.SenderPassword;
                if (sendUserMailAddress != null && sendUserMailPwd != null)
                {                  
                    service.Credentials = new WebCredentials(sendUserMailAddress, sendUserMailPwd);
                    service.AutodiscoverUrl(sendUserMailAddress, RedirectionUrlValidationCallback);
                }             
            }
            catch
            {
                
            }
            return service;
        }

        public  bool RedirectionUrlValidationCallback(String redirectionUrl)
        {
            bool redirectionValidated = false;
            if (redirectionUrl.Equals("https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml"))
                redirectionValidated = true;
            return redirectionValidated;
        }

      

        public  bool CreateCalendarEvent(ExchangeService service,Appointment objAppointment,string[] rqdAttendeelist,string[] optionalAttendeelist)
        {          
            try
            {             

                ItemView itemView = new ItemView(100);
                itemView.PropertySet = new PropertySet(BasePropertySet.IdOnly);
                List<Appointment> ToCreate = new List<Appointment>();
                Appointment appointment = new Appointment(service);
                appointment.Subject = objAppointment.Subject;// "Calendar Request from Console App";
                appointment.Location = objAppointment.Location;// "Office365";
                appointment.Start = TimeZoneInfo.ConvertTimeToUtc(objAppointment.Start, TimeZoneInfo.Local);
                appointment.End = TimeZoneInfo.ConvertTimeToUtc(objAppointment.End, TimeZoneInfo.Local);// objAppointment.End;
                appointment.Body = objAppointment.Body;
                appointment.IsReminderSet = objAppointment.IsReminderSet;           
                appointment.IsResponseRequested = false;
                foreach(string req in rqdAttendeelist)
                {
                    appointment.RequiredAttendees.Add(req);
                }
                foreach (string opt in optionalAttendeelist)
                {
                    appointment.OptionalAttendees.Add(opt);
                }
                ToCreate.Add(appointment);
                ServiceResponseCollection<ServiceResponse> CreateResponse = service.CreateItems(ToCreate, WellKnownFolderName.Calendar, MessageDisposition.SaveOnly, SendInvitationsMode.SendOnlyToAll);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                service = null;
            }
        }
    }
}

    



