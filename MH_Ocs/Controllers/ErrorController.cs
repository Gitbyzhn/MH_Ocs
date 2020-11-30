using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MH_Ocs.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Noaccess()
        {
            return View();
        }
    }
}