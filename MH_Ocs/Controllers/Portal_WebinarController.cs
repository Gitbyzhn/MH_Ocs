using MH_Ocs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MH_Ocs.Controllers
{
    [Authorize(Roles = "admin")]
    public class portal_webinarController : Controller
    {



        private Entities db = new Entities();

        // GET: Portal_Webinar

        //: VIDEOLESSONS -------------------------------------------------------------------------
        public ActionResult VideoLessons(string language)
        {
            if (language == null) { language = "ru"; }

            var VideoLessons = db.Webinar_VideoL.Where(e => e.language == language).ToList();
            ViewBag.language = language;
            return View(VideoLessons);
        }


        public ActionResult createvlessons(string language)
        {

            ViewBag.language = language;

           

            var VideoLessons = db.Webinar_VideoL.Where(e => e.language == "ru").ToList();

          

            return View(VideoLessons);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult createvlessons(Webinar_VideoL videols, int minute, string language, string XId, bool? publish)
        {

            if (language == null) { language = "ru"; }
            ViewBag.language = language;

            if (publish == null)
            {
                publish = false;
            }

            string Msg = "error";

            try
            {
                videols.XId = Convert.ToDouble(XId.Replace(",", "."));
            }
            catch
            {
                Msg = "warning02";
            }


            HttpPostedFileBase file1 = Request.Files["img1"];

            HttpPostedFileBase file2 = Request.Files["img2"];

            Webinar_VideoL video = db.Webinar_VideoL.Where(e => e.language == language && e.XId == videols.XId).FirstOrDefault();

            if (video == null)
            {

                try
                {

                    if (file1.FileName != "" && file1 != null)
                    {

                        string ImageName = file1.FileName;
                        videols.Iconimg = "~/Images/Webinar/" + language + "/" + DateTime.Now.Date.Year + DateTime.Now.Date.Month + DateTime.Now.Date.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + "_" + ImageName.Replace(" ", "_");
                        file1.SaveAs(HttpContext.Server.MapPath(videols.Iconimg));

                    }

                    if (file2.FileName != "" && file2 != null)
                    {

                        string ImageName = file2.FileName;
                        videols.Iconimg2 = "~/Images/Webinar/" + language + "/" + DateTime.Now.Date.Year + DateTime.Now.Date.Month + DateTime.Now.Date.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + "_" + ImageName.Replace(" ", "_");
                        file2.SaveAs(HttpContext.Server.MapPath(videols.Iconimg2));

                    }

                    Webinar_VideoXL videoXL = db.Webinar_VideoXL.Where(e => e.XId == videols.XId).FirstOrDefault();

                    if (videoXL == null)
                    {
                        Webinar_VideoXL newVideoXL = new Webinar_VideoXL();
                        newVideoXL.XId = videols.XId;
                        newVideoXL.Enable = (bool)publish;
                        db.Webinar_VideoXL.Add(newVideoXL);



                        Webinar_VideoLEM newVideLem = new Webinar_VideoLEM();
                        newVideLem.Eye = 0;
                        newVideLem.Likes = 0;
                        newVideLem.minute = minute;
                        newVideLem.VideoXId = videols.XId;
                        db.Webinar_VideoLEM.Add(newVideLem);
                    }


                    videols.Date = DateTime.Now.AddHours(5);

                    db.Webinar_VideoL.Add(videols);
                    db.SaveChanges();
                    Msg = "success";
                }
                catch
                {

                }

            }
            else if (video != null)
            {
                Msg = "warning01";
            }



            if (Msg != "success")
            {
                if (file1.FileName != "" && file1 != null)
                {

                    string fullPath = Request.MapPath(videols.Iconimg);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                }
                if (file2.FileName != "" && file2 != null)
                {

                    string fullPath = Request.MapPath(videols.Iconimg2);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                }
            }

            var VideoLessons = db.Webinar_VideoL.Where(e => e.language == "ru").ToList();
        
       
            ViewBag.AddVideoMsg = Msg;

            return View(VideoLessons);
        }



        //: EDITVLESSONS -------------------------------------------------------------------------
        public ActionResult editvlessons(int Id, string language)
        {

            Webinar_VideoL videols = db.Webinar_VideoL.Find(Id);

            if (videols != null)
            {
                if (language == null) { language = "ru"; }
                ViewBag.language = language;


     

                return View(videols);

            }

            return RedirectToAction("VideoLessons", new { language = language });





        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult editvlessons(Webinar_VideoL videols, int minute, string language, bool? publish)
        {

            string Msg = "error";
            Webinar_VideoL oldVideoL = db.Webinar_VideoL.Find(videols.Id);
            try
            {


                if (language == null) { language = "ru"; }


                if (publish == null)
                {
                    publish = false;
                }

                HttpPostedFileBase file1 = Request.Files["img1"];
                if (file1.FileName != "" && file1 != null)
                {

                    string fullPath = Request.MapPath(oldVideoL.Iconimg);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    string ImageName = file1.FileName;
                    oldVideoL.Iconimg = "~/Images/Webinar/" + language + "/" + DateTime.Now.Date.Year + DateTime.Now.Date.Month + DateTime.Now.Date.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + "_" + ImageName.Replace(" ", "_");
                    file1.SaveAs(HttpContext.Server.MapPath(oldVideoL.Iconimg));

                }


                HttpPostedFileBase file2 = Request.Files["img2"];
                if (file2.FileName != "" && file2 != null)
                {
                    string fullPath = Request.MapPath(oldVideoL.Iconimg2);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    string ImageName = file2.FileName;
                    oldVideoL.Iconimg2 = "~/Images/Webinar/" + language + "/" + DateTime.Now.Date.Year + DateTime.Now.Date.Month + DateTime.Now.Date.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + "_" + ImageName.Replace(" ", "_");
                    file2.SaveAs(HttpContext.Server.MapPath(oldVideoL.Iconimg2));
                }




                oldVideoL.Link = videols.Link;
                oldVideoL.Name = videols.Name;
                oldVideoL.Webinar_VideoXL.Enable = (bool)publish;


                Webinar_VideoLEM videolem = oldVideoL.Webinar_VideoXL.Webinar_VideoLEM.FirstOrDefault();
                videolem.minute = minute;

                db.SaveChanges();
                Msg = "success";

            }
            catch { }

          
       

            ViewBag.Message = Msg;

            return View(oldVideoL);

        }



        //: REMOVELESSONS -------------------------------------------------------------------------
        [HttpPost]
        public ActionResult removevlessons(int Id)
        {

            string Msg = "error";

            Webinar_VideoL videols = db.Webinar_VideoL.Find(Id);

            try
            {

                if (videols != null)
                {
                    if (videols.language == "ru")
                    {
                        Webinar_VideoXL VideoXl = db.Webinar_VideoXL.Find(videols.XId);
                        if (VideoXl.Webinar_VideoL.Where(e => e.language != "ru").Count() == 0)
                        {





                            db.Webinar_VideoL.Remove(videols);

                            Webinar_VideoLEM VideoLEMObj = db.Webinar_VideoLEM.Where(e => e.VideoXId == videols.XId).First();

                            db.Webinar_VideoLEM.Remove(VideoLEMObj);

                            Webinar_VideoXL VideoXLObj = db.Webinar_VideoXL.Where(e => e.XId == videols.XId).First();

                            db.Webinar_VideoXL.Remove(VideoXLObj);

                            db.SaveChanges();

                            Msg = "success";

                        }

                    }
                    else
                    {

                        db.Webinar_VideoL.Remove(videols);

                        db.SaveChanges();

                        Msg = "success";

                    }
                }




            }
            catch { }




            return Json(Msg);

        }

     
    }
}