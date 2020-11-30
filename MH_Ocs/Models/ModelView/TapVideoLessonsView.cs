using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MH_Ocs.Models.ModelView
{
    public class TapVideoLessonsView
    {

        public Nullable<int> Id { get; set; }

        public string Modul { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Iconimg { get; set; }
        public string Iconimg2 { get; set; }
        public string language { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }

    }
}