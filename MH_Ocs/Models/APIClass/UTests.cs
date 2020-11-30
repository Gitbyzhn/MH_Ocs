using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MH_Ocs.Models.APIClass
{
    public class UTest
    {
        public string Question { get; set; }

        public string A { get; set; }

        public string B { get; set; }

        public string C { get; set; }

        public string D { get; set; }

        public string E { get; set; }

    

        public int? Answer { get; set; }
    }

    public class TestCs
    {
        public int?Id { get; set; }
    }


    public class ResponseUTest
    {
        public string status { get; set; }
        
        public int Total { get; set; }
        public List<UTest> UTest { get; set; }
    }

}