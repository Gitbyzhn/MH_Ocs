using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MH_Ocs.Models.APIClass
{
    public class UVideoLessons
    {
        public int Id { get; set; }
        public int MId { get; set; }
        public string lang { get; set; }

        public string Name { get; set; }
        public string Image { get; set; }
        public int Like { get; set; }

        public int View { get; set; }

        public int minute { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }

        public bool Enable { get; set; }


    }

    public class VideoLessonsCs
    {
        public int? MId { get; set; }
        public string language { get; set; }
    }


    public class ResponseVideoLs
    {
        public string status { get; set; }
        public List<UVideoLessons> UVideoLessons { get; set; }
    }
}