using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MH_Ocs.Models.APIClass
{

    public class VideoStaticsCs
    {
        public int? videoId { get; set; }
        public bool? like { get; set; }
        public bool? viewing { get; set; }

        public bool? chosen { get; set; }
    }
}