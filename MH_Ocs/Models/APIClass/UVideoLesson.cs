using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MH_Ocs.Models.APIClass
{
    public class UVideoLesson
    {
        public int Id { get; set; }
      
        public string lang { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }
  
        public int Like { get; set; }

        public int Views { get; set; }



        public bool Liked { get; set; }
        public bool Viewed { get; set;}
        public bool Chosen { get; set; }

        public bool FoolLook { get; set;}

        public bool Test { get; set; }

        public bool Task { get; set; }

        public int? previousId { get; set; }
        public int? nextId { get; set; }
        public bool certificate { get; set; }

    }

    public class VideoLessonCs
    {
        public int? Id { get; set; }
    
    }




    public class ResponseVideoL
    {
        public string status { get; set; }
        public UVideoLesson UVideoLesson { get; set; }
    }




   
}