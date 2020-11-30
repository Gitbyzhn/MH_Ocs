using MH_Ocs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MH_Ocs.Controllers
{

    [Authorize(Roles = "admin")]
    public class portalController : Controller
    {
        
        private Entities db = new Entities();

        public ActionResult Users()
        {

            var Users = db.UserInfoes.ToList();
            return View(Users);
        }


        

        //: MODULES ------------------------------------------------------------------------------
        public ActionResult Modules(string language)
        {
            if (language == null) { language = "ru"; }

            ViewBag.language = language;

            var Modules = db.Modules_Property.Where(e => e.lang == language).OrderBy(e => e.Modul.XId).ToList();


            return View(Modules);
        }
        public ActionResult createModule(string language)
        {
            if (language == null) { language = "ru"; }
            ViewBag.language = language;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult createModule(ModuleCs newModule, string language)
        {
            if (language == null) { language = "ru"; }
            ViewBag.language = language;


            string Msg = "error";

            Modul createModule = new Modul();


            try
            {
                HttpPostedFileBase file1 = Request.Files["img1"];
                if (file1.FileName != "" && file1 != null)
                {

                    string ImageName = file1.FileName;
                    createModule.Image = "~/Images/ModulImage/" +DateTime.Now.Date.Year+ DateTime.Now.Date.Month+ DateTime.Now.Date.Day+ DateTime.Now.Hour+ DateTime.Now.Minute+ DateTime.Now.Second+"_"+ ImageName.Replace(" ", "_");
                    file1.SaveAs(HttpContext.Server.MapPath(createModule.Image));

                }



                Modules_Property Modules_PRU = new Modules_Property();
                Modules_PRU.lang = "ru";
                Modules_PRU.Titile = newModule.NameRU;
                createModule.Modules_Property.Add(Modules_PRU);

                if (newModule.NameKZ != null)
                {
                    Modules_Property Modules_PKZ = new Modules_Property();
                    Modules_PKZ.lang = "kz";
                    Modules_PKZ.Titile = newModule.NameKZ;
                    createModule.Modules_Property.Add(Modules_PKZ);
                }




                if (newModule.NameUZ != null)
                {
                    Modules_Property Modules_PUZ = new Modules_Property();
                    Modules_PUZ.lang = "uz";
                    Modules_PUZ.Titile = newModule.NameUZ;
                    createModule.Modules_Property.Add(Modules_PUZ);
                }

                if (newModule.NameKR != null)
                {
                    Modules_Property Modules_PKR = new Modules_Property();
                    Modules_PKR.lang = "kr";
                    Modules_PKR.Titile = newModule.NameKR;
                    createModule.Modules_Property.Add(Modules_PKR);
                }

                if (newModule.NameEN != null)
                {
                    Modules_Property Modules_PEN = new Modules_Property();
                    Modules_PEN.lang = "en";
                    Modules_PEN.Titile = newModule.NameEN;
                    createModule.Modules_Property.Add(Modules_PEN);
                }


                if (newModule.NameTR != null)
                {
                    Modules_Property Modules_PTR = new Modules_Property();
                    Modules_PTR.lang = "tr";
                    Modules_PTR.Titile = newModule.NameTR;
                    createModule.Modules_Property.Add(Modules_PTR);
                }


                Modul_userLevel Modul_level = new Modul_userLevel();
                Modul_level.LevelId = newModule.LevelId;

                createModule.Modul_userLevel.Add(Modul_level);


                createModule.Enable = newModule.publish;

                createModule.XId = newModule.XId;

                db.Moduls.Add(createModule);
                db.SaveChanges();
                Msg = "success";


            }
            catch { }
            ViewBag.AddModuleMsg = Msg;

            return View();
        }



        //: EDITMODULE ---------------------------------------------------------------------------
        public ActionResult editModule(int Id, string language)
        {
            if (language == null) { language = "ru"; }

            Modul Module = db.Moduls.Find(Id);
            ViewBag.language = language;

            ModuleCs ModuleCs = new ModuleCs();
            ModuleCs.ModuleId = Id;
            ModuleCs.LevelId = Module.Modul_userLevel.FirstOrDefault().LevelId;

            Modules_Property Modules_PKZ = Module.Modules_Property.Where(e => e.lang == "kz").FirstOrDefault();
            if (Modules_PKZ != null)
            {
                ModuleCs.NameKZ = Modules_PKZ.Titile;
            }
        


            Modules_Property Modules_PRU = Module.Modules_Property.Where(e => e.lang == "ru").FirstOrDefault();
            if (Modules_PRU != null)
            {
                ModuleCs.NameRU = Modules_PRU.Titile;
            }

            Modules_Property Modules_PUZ = Module.Modules_Property.Where(e => e.lang == "uz").FirstOrDefault();
            if (Modules_PUZ != null)
            {
                ModuleCs.NameUZ = Modules_PUZ.Titile;
            }

            Modules_Property Modules_PKR = Module.Modules_Property.Where(e => e.lang == "kr").FirstOrDefault();
            if (Modules_PKR != null)
            {
                ModuleCs.NameKR = Modules_PKR.Titile;
            }

            Modules_Property Modules_PEN = Module.Modules_Property.Where(e => e.lang == "en").FirstOrDefault();
            if (Modules_PEN != null)
            {
                ModuleCs.NameEN = Modules_PEN.Titile;
            }

            Modules_Property Modules_PTR = Module.Modules_Property.Where(e => e.lang == "tr").FirstOrDefault();
            if (Modules_PTR != null)
            {
                ModuleCs.NameTR = Modules_PTR.Titile;
            }

            ModuleCs.Image = Module.Image;
            ModuleCs.publish = Module.Enable;
            ModuleCs.XId = Module.XId;

            return View(ModuleCs);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult editModule(ModuleCs newModule, string language)
        {
            if (language == null) { language = "ru"; }
            ViewBag.language = language;

            string Msg = "error";

            Modul editModule = db.Moduls.Find(newModule.ModuleId);

            try
            {
                newModule.Image = editModule.Image;

                HttpPostedFileBase file1 = Request.Files["img1"];
                if (file1.FileName != "" && file1 != null)
                {
                    string fullPath = Request.MapPath(editModule.Image);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    string ImageName = file1.FileName;
                    editModule.Image = "~/Images/ModulImage/" + DateTime.Now.Date.Year + DateTime.Now.Date.Month + DateTime.Now.Date.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + "_" + ImageName.Replace(" ", "_");
                    newModule.Image = editModule.Image;
                    file1.SaveAs(HttpContext.Server.MapPath(editModule.Image));

                }

                Modules_Property Modules_PKZ = editModule.Modules_Property.Where(e => e.lang == "kz").FirstOrDefault();
                if (Modules_PKZ != null)
                {
                    Modules_PKZ.Titile = newModule.NameKZ;
                }
                else
                {

                    Modules_Property newModules_PKZ = new Modules_Property();
                    newModules_PKZ.lang = "kz";
                    newModules_PKZ.Titile = newModule.NameKZ;
                    editModule.Modules_Property.Add(newModules_PKZ);

                }


                Modules_Property Modules_PRU = editModule.Modules_Property.Where(e => e.lang == "ru").FirstOrDefault();
                if (Modules_PRU != null)
                {
                    Modules_PRU.Titile = newModule.NameRU;
                }

                Modules_Property Modules_PUZ = editModule.Modules_Property.Where(e => e.lang == "uz").FirstOrDefault();
                if (Modules_PUZ != null)
                {
                    Modules_PUZ.Titile = newModule.NameUZ;
                }
                else
                {

                    Modules_Property newModules_PUZ = new Modules_Property();
                    newModules_PUZ.lang = "uz";
                    newModules_PUZ.Titile = newModule.NameUZ;
                    editModule.Modules_Property.Add(newModules_PUZ);

                }

                Modules_Property Modules_PKR = editModule.Modules_Property.Where(e => e.lang == "kr").FirstOrDefault();
                if (Modules_PKR != null)
                {
                    Modules_PKR.Titile = newModule.NameKR;
                }
                else
                {

                    Modules_Property newModules_PKR = new Modules_Property();
                    newModules_PKR.lang = "kr";
                    newModules_PKR.Titile = newModule.NameKR;
                    editModule.Modules_Property.Add(newModules_PKR);

                }

                Modules_Property Modules_PEN = editModule.Modules_Property.Where(e => e.lang == "en").FirstOrDefault();
                if (Modules_PEN != null)
                {
                    Modules_PEN.Titile = newModule.NameEN;
                }
                else
                {

                    Modules_Property newModules_PEN = new Modules_Property();
                    newModules_PEN.lang = "en";
                    newModules_PEN.Titile = newModule.NameEN;
                    editModule.Modules_Property.Add(newModules_PEN);

                }


                Modules_Property Modules_PTR = editModule.Modules_Property.Where(e => e.lang == "tr").FirstOrDefault();
                if (Modules_PTR != null)
                {
                    Modules_PTR.Titile = newModule.NameTR;
                }
                else
                {

                    Modules_Property newModules_PTR = new Modules_Property();
                    newModules_PTR.lang = "tr";
                    newModules_PTR.Titile = newModule.NameTR;
                    editModule.Modules_Property.Add(newModules_PTR);

                }


                Modul_userLevel Modul_level = editModule.Modul_userLevel.FirstOrDefault();
                Modul_level.LevelId = newModule.LevelId;
                editModule.Enable = newModule.publish;
                editModule.XId = newModule.XId;

                db.SaveChanges();
                Msg = "success";
                ViewBag.EditModuleMsg = Msg;
            }
            catch { }


            return View(newModule);
        }


        //: REMOVEMODULE -------------------------------------------------------------------------
        [HttpPost]
        public ActionResult removemodule(int Id)
        {

            string Msg = "error";

            Modul Module = db.Moduls.Find(Id);


            try
            {


                if (Module.VideoLs.Count > 0)
                {
                    foreach (var videols in Module.VideoLs.ToList())
                    {

                        var Tests = videols.Tests.ToList();
                        db.Tests.RemoveRange(Tests);
                        db.VideoLs.Remove(videols);

                        VideoLEM VideoLEMObj = db.VideoLEMs.Where(e => e.VideoXId == videols.XId).First();
                        db.VideoLEMs.Remove(VideoLEMObj);


                        VideoXL VideoXLObj = db.VideoXLs.Where(e => e.XId == videols.XId).First();
                        db.VideoXLs.Remove(VideoXLObj);


                    }


                }


                db.Modul_userLevel.RemoveRange(Module.Modul_userLevel);
                db.Modules_Property.RemoveRange(Module.Modules_Property);
                db.Moduls.Remove(Module);
                db.SaveChanges();
                Msg = "success";


            }
            catch
            {


            }




            return Json(Msg);

        }



        //: VIDEOLESSONS -------------------------------------------------------------------------
        public ActionResult VideoLessons(string language, int? MId)
        {
            if (language == null) { language = "ru"; }

            var VideoLessons = db.VideoLs.Where(e => e.language == language).ToList();

            if (MId != null && MId != 0)
            {
                VideoLessons = VideoLessons.Where(e => e.ModulId == MId).ToList();
            }




            var Moduls = db.Modules_Property.Where(e => e.lang == language).ToList();

            ViewBag.MId = MId;
            ViewBag.Moduls = Moduls;
            ViewBag.language = language;
            return View(VideoLessons);
        }


        public ActionResult createvlessons(string language)
        {

            ViewBag.language = language;

            var Modules = db.Modules_Property.Where(e => e.lang == "ru").OrderBy(e => e.Modul.XId).ToList();

            var VideoLessons = db.VideoLs.Where(e => e.language == "ru").ToList();

            ViewBag.VideoLessons = VideoLessons;

            return View(Modules);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult createvlessons(VideoL videols, int minute, string language, string XId, bool? publish)
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

            VideoL video = db.VideoLs.Where(e => e.language == language && e.XId == videols.XId).FirstOrDefault();

            if (video == null)
            {

                try
                {

                    if (file1.FileName != "" && file1 != null)
                    {

                        string ImageName = file1.FileName;
                        videols.Iconimg = "~/Images/Limage/" + language + "/" + DateTime.Now.Date.Year + DateTime.Now.Date.Month + DateTime.Now.Date.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + "_" + ImageName.Replace(" ", "_");
                        file1.SaveAs(HttpContext.Server.MapPath(videols.Iconimg));

                    }

                    if (file2.FileName != "" && file2 != null)
                    {

                        string ImageName = file2.FileName;
                        videols.Iconimg2 = "~/Images/Limage/" + language + "/" + DateTime.Now.Date.Year + DateTime.Now.Date.Month + DateTime.Now.Date.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + "_" + ImageName.Replace(" ", "_");
                        file2.SaveAs(HttpContext.Server.MapPath(videols.Iconimg2));

                    }

                    VideoXL videoXL = db.VideoXLs.Where(e => e.XId == videols.XId).FirstOrDefault();

                    if (videoXL == null)
                    {
                        VideoXL newVideoXL = new VideoXL();
                        newVideoXL.XId = videols.XId;
                        newVideoXL.MId = videols.ModulId;
                        newVideoXL.Enable = (bool)publish;
                        db.VideoXLs.Add(newVideoXL);



                        VideoLEM newVideLem = new VideoLEM();
                        newVideLem.Eye = 0;
                        newVideLem.Likes = 0;
                        newVideLem.minute = minute;
                        newVideLem.VideoXId = videols.XId;
                        db.VideoLEMs.Add(newVideLem);
                    }

                    videols.Date = DateTime.Now.AddHours(5);

                    db.VideoLs.Add(videols);
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

            var VideoLessons = db.VideoLs.Where(e => e.language == "ru").ToList();
            var Modules = db.Modules_Property.Where(e => e.lang == "ru").ToList();
            ViewBag.VideoLessons = VideoLessons;
            ViewBag.AddVideoMsg = Msg;

            return View(Modules);
        }



        //: EDITVLESSONS -------------------------------------------------------------------------
        public ActionResult editvlessons(int Id, string language)
        {

            VideoL videols = db.VideoLs.Find(Id);

            if (videols != null)
            {
                if (language == null) { language = "ru"; }
                ViewBag.language = language;


                var Modules = db.Modules_Property.Where(e => e.lang == "ru").OrderBy(e => e.Modul.XId).ToList();
                ViewBag.Modules = Modules;

                return View(videols);

            }

            return RedirectToAction("VideoLessons", new { language = language });





        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult editvlessons(VideoL videols, int minute, string language, bool? publish)
        {

            string Msg = "error";
            VideoL oldVideoL = db.VideoLs.Find(videols.Id);
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
                    oldVideoL.Iconimg = "~/Images/Limage/" + language + "/" + DateTime.Now.Date.Year + DateTime.Now.Date.Month + DateTime.Now.Date.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + "_" + ImageName.Replace(" ", "_");
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
                    oldVideoL.Iconimg2 = "~/Images/Limage/" + language + "/" + DateTime.Now.Date.Year + DateTime.Now.Date.Month + DateTime.Now.Date.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + "_" + ImageName.Replace(" ", "_");
                    file2.SaveAs(HttpContext.Server.MapPath(oldVideoL.Iconimg2));
                }




                oldVideoL.Link = videols.Link;
                oldVideoL.Name = videols.Name;
                oldVideoL.VideoXL.Enable = (bool)publish;


                VideoLEM videolem = oldVideoL.VideoXL.VideoLEMs.FirstOrDefault();
                videolem.minute = minute;

                db.SaveChanges();
                Msg = "success";

            }
            catch { }

            var Modules = db.Modules_Property.Where(e => e.lang == "ru").ToList();
            ViewBag.Modules = Modules;

            ViewBag.Message = Msg;

            return View(oldVideoL);

        }



        //: REMOVELESSONS -------------------------------------------------------------------------
        [HttpPost]
        public ActionResult removevlessons(int Id)
        {

            string Msg = "error";

            VideoL videols = db.VideoLs.Find(Id);

            try
            {

                if (videols != null)
                {
                    if (videols.language == "ru")
                    {
                        VideoXL VideoXl = db.VideoXLs.Find(videols.XId);
                        if (VideoXl.VideoLs.Where(e => e.language != "ru").Count() == 0)
                        {



                            var Tests = videols.Tests.ToList();
                            db.Tests.RemoveRange(Tests);

                            db.VideoLs.Remove(videols);

                            VideoLEM VideoLEMObj = db.VideoLEMs.Where(e => e.VideoXId == videols.XId).First();

                            db.VideoLEMs.Remove(VideoLEMObj);

                            VideoXL VideoXLObj = db.VideoXLs.Where(e => e.XId == videols.XId).First();

                            db.VideoXLs.Remove(VideoXLObj);

                            db.SaveChanges();

                            Msg = "success";

                        }

                    }
                    else
                    {

                        var Tests = videols.Tests.ToList();
                        db.Tests.RemoveRange(Tests);

                        db.VideoLs.Remove(videols);

                        db.SaveChanges();

                        Msg = "success";

                    }
                }




            }
            catch { }




            return Json(Msg);

        }
        

        //: VIDEOTESTS ----------------------------------------------------------------------------
        public ActionResult VideoTests(string language, string Message, int? Vid)
        {


            if (language == null) { language = "ru"; }
            ViewBag.language = language;

            var VideoLessons = db.VideoLs.Where(e => e.language == language).ToList();

            if (Vid != null)
            {

                VideoLessons = VideoLessons.Where(e => e.Id == Vid).ToList();
            }


            ViewBag.Message = Message;


            return View(VideoLessons);
        }


        //: CREATETESTS ----------------------------------------------------------------------------
        public ActionResult createvtests(string language)
        {
            if (language == null) { language = "ru"; }

            var VideoLessons = db.VideoLs.Where(e => e.language == language).OrderBy(e => e.XId).ToList();

            ViewBag.language = language;
            return View(VideoLessons);
        }


        [HttpPost]
        public ActionResult createvtests(Test test, string language)
        {
            if (language == null) { language = "ru"; }
            ViewBag.language = language;

            string Message = "success";

            if (ModelState.IsValid)
            {


                db.Tests.Add(test);
                db.SaveChanges();

            }
            else
            {

                Message = "error";
            }



            var VideoLessons = db.VideoLs.Where(e => e.language == language).ToList();


            ViewBag.Message = Message;


            VideoL videoTest = VideoLessons.Where(e => e.Id == test.LessonId).FirstOrDefault();

            if (videoTest != null)
            {

                ViewBag.LessonId = test.LessonId;

            }


            return View(VideoLessons);
        }


        //: EDITVTESTS -----------------------------------------------------------------------------
        public ActionResult editvtests(int Id, string language)
        {
            if (language == null) { language = "ru"; }
            var VideoLessons = db.VideoLs.Where(e => e.language == language).ToList();

            ViewBag.language = language;


            if (VideoLessons.Count > 0)
            {
                Test sTest = VideoLessons.FirstOrDefault().Tests.FirstOrDefault();

                ViewBag.Test = sTest;
            }


            Test test = db.Tests.Find(Id);

            ViewBag.VideoLessons = VideoLessons;

            return View(test);
        }



        [HttpPost]
        public ActionResult editvtests(Test test, string language)
        {
            if (language == null) { language = "ru"; }
            Test oldTest = db.Tests.Find(test.Id);

            ViewBag.language = language;

            string Msg = "success";

            try
            {
                oldTest.Question = test.Question;
                oldTest.A = test.A;
                oldTest.B = test.B;
                oldTest.C = test.C;
                oldTest.D = test.D;
                oldTest.E = test.E;
                oldTest.Answer = test.Answer;
                oldTest.LessonId = test.LessonId;
                db.SaveChanges();
            }
            catch { Msg = "error"; }




            var VideoLessons = db.VideoLs.Where(e => e.language == language).ToList();


            ViewBag.Message = Msg;
            ViewBag.VideoLessons = VideoLessons;



            return View(test);
        }



        //: REMOVETESTS -----------------------------------------------------------------------------
        public ActionResult removevtests(int Id, string language)
        {
            if (language == null) { language = "ru"; }
            string Msg = "error";

            Test test = db.Tests.Find(Id);
            VideoL videols = db.VideoLs.Find(test.LessonId);
            try
            {
                db.Tests.Remove(test);
                db.SaveChanges();
                Msg = "success";
            }
            catch { }
            return RedirectToAction("VideoTests", "portal", new { language = videols.language, Message = Msg });
        }


        //: GIVE VIDEOLESSONS FOR SELECT MODULE
        public JsonResult VideLsForModule(int Id)
        {



            Modul Module = db.Moduls.Find(Id);

            var VideoLessons = Module.VideoLs.Where(e => e.language == "ru").ToList();

            var VideoLsX = new List<VideoLsX>();

            if (VideoLessons.Count > 0)
            {
                foreach (var video in VideoLessons)
                {
                    VideoLsX.Add(new VideoLsX
                    {
                        XId = video.XId,
                        Name = video.Name
                    });
                }

            }



            return Json(VideoLsX, JsonRequestBehavior.AllowGet);
        }





   


    }



}