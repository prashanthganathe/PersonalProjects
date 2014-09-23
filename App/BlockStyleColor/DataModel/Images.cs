using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows8Theme.DataModel
{
    public class ImageModel
    {

        public int ImageID { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string RequestComment { get; set; }
        public string ResponseComment { get; set; }
        public string PID { get; set; }
    }
}
