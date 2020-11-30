using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MH_Ocs.Models.APIClass
{
    public class UTraining_Modules
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string lang { get; set; }

    }


   

    public class Training_ModulesCs
    {
        public string language { get; set; }
    }


    public class ResponseUTraining_Modules
    {
        public string status { get; set; }
        public List<UTraining_Modules> Training_Modules { get; set; }
    }
}