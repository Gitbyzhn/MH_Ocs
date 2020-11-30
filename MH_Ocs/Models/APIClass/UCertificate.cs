using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MH_Ocs.Models.APIClass
{
    public class UCertificate
    {
        public bool certificate { get; set; }
        public string certificateURL { get; set; }

    }

    

    public class ResponseUCertificate
    {
        public string status { get; set; }
        public UCertificate UCertificate { get; set; }
    }



}