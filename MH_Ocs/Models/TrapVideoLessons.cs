using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MH_Ocs.Models
{
    public partial class TrapVideoLessons
    {
        public int Id { get; set; }
        public double XId { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Iconimg { get; set; }
        public string Iconimg2 { get; set; }
        public string language { get; set; }
        public Nullable<int> TrapId { get; set; }

       
    }
}