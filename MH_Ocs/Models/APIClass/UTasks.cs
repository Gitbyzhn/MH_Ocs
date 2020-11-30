using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MH_Ocs.Models.APIClass
{
    public class UTasks
    {


        public int Id { get; set; }

        public int VideoLessonId {get;set;}

        public string Task { get; set;}

      


    }


    public class ResponseUTasks
    {
        public string status { get; set; }

        public List<UTasks> Tasks { get; set; }
    }
}