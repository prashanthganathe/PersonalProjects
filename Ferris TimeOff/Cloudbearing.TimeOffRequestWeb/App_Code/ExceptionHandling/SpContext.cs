using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudbearing.TimeOffRequestWeb
{
    public class SpContext
    {
        public string HostWebUrl { get; set; }
        public string AppWebUrl { get; set; }
        public string ContextTokenString { get; set; }
        public string ServerUrl { get; set; }
    }
}