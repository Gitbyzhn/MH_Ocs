using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MH_Ocs.Models.APIClass
{
    public class UTraining_VideoLessons
    {

        public int Id { get; set; }
        public int MId { get; set; }
        public string lang { get; set; }

        public string Name { get; set; }
        public string Image { get; set; }
        public int Like { get; set; }

        public int View { get; set; }

        public int minute { get; set; }

       
    }


    

    public class Training_VideoLessonsCs
    {
        public int? MId { get; set; }
        public string language { get; set; }
    }


    public class ResponseUTraining_VideoLs
    {
        public string status { get; set; }
        public List<UTraining_VideoLessons> Training_VideoLessons { get; set; }
    }


}