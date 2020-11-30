using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MH_Ocs.Models.APIClass
{
    public class UWebinar_VideoLesson
    {

        public int Id { get; set; }

        public string lang { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        public int Like { get; set; }

        public int View { get; set; }

        public bool Liked { get; set; }

        public bool Viewed { get; set; }

        public int? previousId { get; set; }
        public int? nextId { get; set; }

    }





    public class Webinar_VideoLessonCs
    {
        public int? Id { get; set; }

    }


    public class ResponseUWebinar_VideoL
    {
        public string status { get; set; }
        public UWebinar_VideoLesson Webinar_VideoLesson { get; set; }
    }
}