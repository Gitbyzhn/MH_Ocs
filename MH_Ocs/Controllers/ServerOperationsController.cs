using MH_Ocs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MH_Ocs.Controllers
{
    public class ServerOperationsController : Controller
    {



        //private Entities db = new Entities();

        //[AllowAnonymous]
        //public async Task<ActionResult> UserCertMHBussinessGroup()
        //{

           
        //    JVLO jvlo = db
        
        //    return View();
        //}


        //[AllowAnonymous]
        //public async  Task<ActionResult> UserCertMHBussinessGroup()
        //{

        //    var usercert = db.Users_Certificates.Where(e => e.PublicCert == true);

        //    foreach (var UserName in usercert)
        //    {

        //        await markTrainingComplete(UserName.UserInfo.UserName);

        //    }

        //    var userinfo = usercert.Select(e => e.UserInfo).OrderBy(e => e.LevelId).ToList();
        //    return View(userinfo);
        //}




        //public JsonResult UpdateUserCert()
        //{

        //    foreach (var user in db.UserInfoes)
        //    {

        //        var usercert = user.Users_Certificates.ToList();

        //        bool F = false;

        //        if (usercert.Count > 0)
        //        {
        //            Users_Certificates usercertF = usercert.Where(e => e.TypeCert == 1 && e.PublicCert == true).FirstOrDefault();

        //            if (usercertF != null)
        //            {
        //                db.Users_Certificates.RemoveRange(usercert.Where(e => e.Id != usercertF.Id));
        //                F = true;

        //            }

        //            if (!F)
        //            {
        //                Users_Certificates usercertT = usercert.Where(e => e.TypeCert == 1 && e.PublicCert == false).FirstOrDefault();


        //                if (usercertT != null)
        //                {
        //                    db.Users_Certificates.RemoveRange(usercert.Where(e => e.Id != usercertT.Id));
        //                    F = true;

        //                }

        //            }
        //            if (!F)
        //            {

        //                Users_Certificates usercertS = usercert.Where(e => e.PublicCert == true).FirstOrDefault();


        //                if (usercertS != null)
        //                {
        //                    db.Users_Certificates.RemoveRange(usercert.Where(e => e.Id != usercertS.Id));
        //                    F = true;

        //                }

        //            }


        //            if (!F)
        //            {

        //                Users_Certificates usercertG = usercert.Where(e => e.PublicCert == false).FirstOrDefault();


        //                if (usercertG != null)
        //                {
        //                    db.Users_Certificates.RemoveRange(usercert.Where(e => e.Id != usercertG.Id));
        //                    F = true;

        //                }

        //            }

        //        }





        //    }

        //    db.SaveChanges();
        //    return Json("ok", JsonRequestBehavior.AllowGet);
        //}


    }
}