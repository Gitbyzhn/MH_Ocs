using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MH_Ocs.Models.APIClass
{
    public class UModuls
    {
        public int Id { get; set;}
        public string Name { get; set;}
        public string Image { get; set;}
        public string lang { get; set;}
        public bool Enable { get; set;}
    }

  
    public class ModulCs {
        public string language { get; set; }
    }


    public class ResponseUModuls
    {
        public string status { get; set; }
        public List<UModuls> UModuls { get; set; }

        public string certificateURL { get; set; }
    }

}