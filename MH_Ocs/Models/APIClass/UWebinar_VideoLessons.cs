using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MH_Ocs.Models.APIClass
{
    public class UWebinar_VideoLessons
    {

        public int Id { get; set; }
        public string lang { get; set; }

        public string Name { get; set; }
        public string Image { get; set; }
        public int Like { get; set; }

        public int View { get; set; }

        public int minute { get; set; }


    }




    public class Webinar_VideoLessonsCs
    {
  
        public string language { get; set; }
    }


    public class ResponseUWebinar_VideoLs
    {
        public string status { get; set; }
        public List<UWebinar_VideoLessons> Webinar_VideoLessons { get; set; }
    }
}