using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MH_Ocs.Models.APIClass
{
    public class UVideoLcompletionCs
    {
        public int? VideoId { get; set; }
        public bool? FoolLook { get; set; }
        public int? TestTotal { get; set; }

    }

    public class ResponseUVideoLcompletion
    {
        public string status { get; set; }
        public string msgfoollook { get; set; }
        public string msgtesttotal { get; set; }
        public int? nextId { get; set; }

    }


}