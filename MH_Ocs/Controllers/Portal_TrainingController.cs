using MH_Ocs.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MH_Ocs.Controllers
{
    [Authorize(Roles = "admin")]
    public class portal_trainingController : Controller
    {


        private Entities db = new Entities();

        //: MODULES ------------------------------------------------------------------------------
        public async Task<ActionResult> Modules(string language)
        {
            if (language == null) { language = "ru"; }

            ViewBag.language = language;

            var Modules = await db.Training_Property.Where(e => e.lang == language).OrderBy(e => e.Training.XId).ToListAsync();


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
        public async Task<ActionResult> createModule(TrainingCs newModule, string language)
        {
            if (language == null) { language = "ru"; }
            ViewBag.language = language;


            string Msg = "error";

            Training createModule = new Training();


            try
            {
                HttpPostedFileBase file1 = Request.Files["img1"];
                if (file1.FileName != "" && file1 != null)
                {

                    string ImageName = file1.FileName;
                    createModule.Image = "~/Images/Training/Modules/" + DateTime.Now.Date.Year + DateTime.Now.Date.Month + DateTime.Now.Date.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + "_" + ImageName.Replace(" ", "_");
                    file1.SaveAs(HttpContext.Server.MapPath(createModule.Image));

                }



            Training_Property Modules_PRU = new Training_Property();
            Modules_PRU.lang = "ru";
            Modules_PRU.Titile = newModule.NameRU;
            createModule.Training_Property.Add(Modules_PRU);

            if (newModule.NameKZ != null)
            {
                Training_Property Modules_PKZ = new Training_Property();
                Modules_PKZ.lang = "kz";
                Modules_PKZ.Titile = newModule.NameKZ;
                createModule.Training_Property.Add(Modules_PKZ);
            }




            if (newModule.NameUZ != null)
            {
                Training_Property Modules_PUZ = new Training_Property();
                Modules_PUZ.lang = "uz";
                Modules_PUZ.Titile = newModule.NameUZ;
                createModule.Training_Property.Add(Modules_PUZ);
            }

            if (newModule.NameKR != null)
            {
                Training_Property Modules_PKR = new Training_Property();
                Modules_PKR.lang = "kr";
                Modules_PKR.Titile = newModule.NameKR;
                createModule.Training_Property.Add(Modules_PKR);
            }

            if (newModule.NameEN != null)
            {
                Training_Property Modules_PEN = new Training_Property();
                Modules_PEN.lang = "en";
                Modules_PEN.Titile = newModule.NameEN;
                createModule.Training_Property.Add(Modules_PEN);
            }


            if (newModule.NameTR != null)
            {
                Training_Property Modules_PTR = new Training_Property();
                Modules_PTR.lang = "tr";
                Modules_PTR.Titile = newModule.NameTR;
                createModule.Training_Property.Add(Modules_PTR);
            }







            createModule.Enable = newModule.publish;

                createModule.XId = newModule.XId;

                db.Trainings.Add(createModule);
                await db.SaveChangesAsync();
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

            Training Training = db.Trainings.Find(Id);

            ViewBag.language = language;

            TrainingCs ModuleCs = new TrainingCs();
            ModuleCs.ModuleId = Id;
           

            Training_Property Modules_PKZ = Training.Training_Property.Where(e => e.lang == "kz").FirstOrDefault();
            if (Modules_PKZ != null)
            {
                ModuleCs.NameKZ = Modules_PKZ.Titile;
            }



            Training_Property Modules_PRU = Training.Training_Property.Where(e => e.lang == "ru").FirstOrDefault();
            if (Modules_PRU != null)
            {
                ModuleCs.NameRU = Modules_PRU.Titile;
            }

            Training_Property Modules_PUZ = Training.Training_Property.Where(e => e.lang == "uz").FirstOrDefault();
            if (Modules_PUZ != null)
            {
                ModuleCs.NameUZ = Modules_PUZ.Titile;
            }

            Training_Property Modules_PKR = Training.Training_Property.Where(e => e.lang == "kr").FirstOrDefault();
            if (Modules_PKR != null)
            {
                ModuleCs.NameKR = Modules_PKR.Titile;
            }

            Training_Property Modules_PEN = Training.Training_Property.Where(e => e.lang == "en").FirstOrDefault();
            if (Modules_PEN != null)
            {
                ModuleCs.NameEN = Modules_PEN.Titile;
            }

            Training_Property Modules_PTR = Training.Training_Property.Where(e => e.lang == "tr").FirstOrDefault();
            if (Modules_PTR != null)
            {
                ModuleCs.NameTR = Modules_PTR.Titile;
            }

            ModuleCs.Image = Training.Image;
            ModuleCs.publish = Training.Enable;
            ModuleCs.XId = Training.XId;

            return View(ModuleCs);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult editModule(TrainingCs newModule, string language)
        {
            if (language == null) { language = "ru"; }
            ViewBag.language = language;

            string Msg = "error";

            Training editModule = db.Trainings.Find(newModule.ModuleId);

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
                    editModule.Image = "~/Images/Training/Modules/" + DateTime.Now.Date.Year + DateTime.Now.Date.Month + DateTime.Now.Date.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + "_" + ImageName.Replace(" ", "_");
                    newModule.Image = editModule.Image;
                    file1.SaveAs(HttpContext.Server.MapPath(editModule.Image));

                }

                Training_Property Modules_PKZ = editModule.Training_Property.Where(e => e.lang == "kz").FirstOrDefault();
                if (Modules_PKZ != null)
                {
                    Modules_PKZ.Titile = newModule.NameKZ;
                }
                else
                {

                    Training_Property newModules_PKZ = new Training_Property();
                    newModules_PKZ.lang = "kz";
                    newModules_PKZ.Titile = newModule.NameKZ;
                    editModule.Training_Property.Add(newModules_PKZ);

                }


                Training_Property Modules_PRU = editModule.Training_Property.Where(e => e.lang == "ru").FirstOrDefault();
                if (Modules_PRU != null)
                {
                    Modules_PRU.Titile = newModule.NameRU;
                }

                Training_Property Modules_PUZ = editModule.Training_Property.Where(e => e.lang == "uz").FirstOrDefault();
                if (Modules_PUZ != null)
                {
                    Modules_PUZ.Titile = newModule.NameUZ;
                }
                else
                {

                    Training_Property newModules_PUZ = new Training_Property();
                    newModules_PUZ.lang = "uz";
                    newModules_PUZ.Titile = newModule.NameUZ;
                    editModule.Training_Property.Add(newModules_PUZ);

                }

                Training_Property Modules_PKR = editModule.Training_Property.Where(e => e.lang == "kr").FirstOrDefault();
                if (Modules_PKR != null)
                {
                    Modules_PKR.Titile = newModule.NameKR;
                }
                else
                {

                    Training_Property newModules_PKR = new Training_Property();
                    newModules_PKR.lang = "kr";
                    newModules_PKR.Titile = newModule.NameKR;
                    editModule.Training_Property.Add(newModules_PKR);

                }

                Training_Property Modules_PEN = editModule.Training_Property.Where(e => e.lang == "en").FirstOrDefault();
                if (Modules_PEN != null)
                {
                    Modules_PEN.Titile = newModule.NameEN;
                }
                else
                {

                    Training_Property newModules_PEN = new Training_Property();
                    newModules_PEN.lang = "en";
                    newModules_PEN.Titile = newModule.NameEN;
                    editModule.Training_Property.Add(newModules_PEN);

                }


                Training_Property Modules_PTR = editModule.Training_Property.Where(e => e.lang == "tr").FirstOrDefault();
                if (Modules_PTR != null)
                {
                    Modules_PTR.Titile = newModule.NameTR;
                }
                else
                {

                    Training_Property newModules_PTR = new Training_Property();
                    newModules_PTR.lang = "tr";
                    newModules_PTR.Titile = newModule.NameTR;
                    editModule.Training_Property.Add(newModules_PTR);

                }
                


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

            Training Training = db.Trainings.Find(Id);


            try
            {


                if (Training.Training_VideoL.Count > 0)
                {
                    foreach (var videols in Training.Training_VideoL.ToList())
                    {

                 

                        Training_VideoLEM VideoLEMObj = db.Training_VideoLEM.Where(e => e.VideoXId == videols.XId).First();
                        db.Training_VideoLEM.Remove(VideoLEMObj);


                        Training_VideoXL VideoXLObj = db.Training_VideoXL.Where(e => e.XId == videols.XId).First();
                        db.Training_VideoXL.Remove(VideoXLObj);


                    }


                }


              
                db.Training_Property.RemoveRange(Training.Training_Property);
                db.Trainings.Remove(Training);
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

            var VideoLessons = db.Training_VideoL.Where(e => e.language == language).ToList();

            if (MId != null && MId != 0)
            {
                VideoLessons = VideoLessons.Where(e => e.TrId == MId).ToList();
            }

            


            var Training = db.Training_Property.Where(e => e.lang == language).ToList();

            ViewBag.MId = MId;
            ViewBag.Moduls = Training;
            ViewBag.language = language;
            return View(VideoLessons);
        }


        public ActionResult createvlessons(string language)
        {

            ViewBag.language = language;

            var Trainings = db.Training_Property.Where(e => e.lang == "ru").OrderBy(e => e.Training.XId).ToList();

            var VideoLessons = db.Training_VideoL.Where(e => e.language == "ru").ToList();

            ViewBag.VideoLessons = VideoLessons;

            return View(Trainings);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult createvlessons(Training_VideoL videols, int minute, string language, string XId, bool? publish)
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

            Training_VideoL video = db.Training_VideoL.Where(e => e.language == language && e.XId == videols.XId).FirstOrDefault();

            if (video == null)
            {

                try
                {

                    if (file1.FileName != "" && file1 != null)
                    {

                        string ImageName = file1.FileName;
                        videols.Iconimg = "~/Images/Training/" + language + "/" + DateTime.Now.Date.Year + DateTime.Now.Date.Month + DateTime.Now.Date.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + "_" + ImageName.Replace(" ", "_");
                        file1.SaveAs(HttpContext.Server.MapPath(videols.Iconimg));

                    }

                    if (file2.FileName != "" && file2 != null)
                    {

                        string ImageName = file2.FileName;
                        videols.Iconimg2 = "~/Images/Training/" + language + "/" + DateTime.Now.Date.Year + DateTime.Now.Date.Month + DateTime.Now.Date.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + "_" + ImageName.Replace(" ", "_");
                        file2.SaveAs(HttpContext.Server.MapPath(videols.Iconimg2));

                    }

                    Training_VideoXL videoXL = db.Training_VideoXL.Where(e => e.XId == videols.XId).FirstOrDefault();

                    if (videoXL == null)
                    {
                        Training_VideoXL newVideoXL = new Training_VideoXL();
                        newVideoXL.XId = videols.XId;
                        newVideoXL.TrId = videols.TrId;
                        newVideoXL.Enable = (bool)publish;
                        db.Training_VideoXL.Add(newVideoXL);



                        Training_VideoLEM newVideLem = new Training_VideoLEM();
                        newVideLem.Eye = 0;
                        newVideLem.Likes = 0;
                        newVideLem.minute = minute;
                        newVideLem.VideoXId = videols.XId;
                        db.Training_VideoLEM.Add(newVideLem);
                    }

                    videols.Date = DateTime.Now.AddHours(5);

                    db.Training_VideoL.Add(videols);
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

            var VideoLessons = db.Training_VideoL.Where(e => e.language == "ru").ToList();
            var Modules = db.Training_Property.Where(e => e.lang == "ru").ToList();
            ViewBag.VideoLessons = VideoLessons;
            ViewBag.AddVideoMsg = Msg;

            return View(Modules);
        }



        //: EDITVLESSONS -------------------------------------------------------------------------
        public ActionResult editvlessons(int Id, string language)
        {

            Training_VideoL videols = db.Training_VideoL.Find(Id);

            if (videols != null)
            {
                if (language == null) { language = "ru"; }
                ViewBag.language = language;


                var Modules = db.Training_Property.Where(e => e.lang == "ru").OrderBy(e => e.Training.XId).ToList();
                ViewBag.Modules = Modules;

                return View(videols);

            }

            return RedirectToAction("VideoLessons", new { language = language });





        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult editvlessons(Training_VideoL videols, int minute, string language, bool? publish)
        {

            string Msg = "error";
            Training_VideoL oldVideoL = db.Training_VideoL.Find(videols.Id);
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
                    oldVideoL.Iconimg = "~/Images/Training/" + language + "/" + DateTime.Now.Date.Year + DateTime.Now.Date.Month + DateTime.Now.Date.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + "_" + ImageName.Replace(" ", "_");
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
                    oldVideoL.Iconimg2 = "~/Images/Training/" + language + "/" + DateTime.Now.Date.Year + DateTime.Now.Date.Month + DateTime.Now.Date.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + "_" + ImageName.Replace(" ", "_");
                    file2.SaveAs(HttpContext.Server.MapPath(oldVideoL.Iconimg2));
                }




                oldVideoL.Link = videols.Link;
                oldVideoL.Name = videols.Name;
                oldVideoL.Training_VideoXL.Enable = (bool)publish;


                Training_VideoLEM videolem = oldVideoL.Training_VideoXL.Training_VideoLEM.FirstOrDefault();
                videolem.minute = minute;

                db.SaveChanges();
                Msg = "success";

            }
            catch { }

            var Training = db.Training_Property.Where(e => e.lang == "ru").ToList();
            ViewBag.Modules = Training;

            ViewBag.Message = Msg;

            return View(oldVideoL);

        }



        //: REMOVELESSONS -------------------------------------------------------------------------
        [HttpPost]
        public ActionResult removevlessons(int Id)
        {

            string Msg = "error";

            Training_VideoL videols = db.Training_VideoL.Find(Id);

            try
            {

                if (videols != null)
                {
                    if (videols.language == "ru")
                    {
                        Training_VideoXL VideoXl = db.Training_VideoXL.Find(videols.XId);
                        if (VideoXl.Training_VideoL.Where(e => e.language != "ru").Count() == 0)
                        {



                          

                            db.Training_VideoL.Remove(videols);

                            Training_VideoLEM VideoLEMObj = db.Training_VideoLEM.Where(e => e.VideoXId == videols.XId).First();

                            db.Training_VideoLEM.Remove(VideoLEMObj);

                            Training_VideoXL  VideoXLObj = db.Training_VideoXL.Where(e => e.XId == videols.XId).First();

                            db.Training_VideoXL.Remove(VideoXLObj);

                            db.SaveChanges();

                            Msg = "success";

                        }

                    }
                    else
                    {
                        
                        db.Training_VideoL.Remove(videols);

                        db.SaveChanges();

                        Msg = "success";

                    }
                }




            }
            catch { }




            return Json(Msg);

        }

        //: GIVE VIDEOLESSONS FOR SELECT MODULE
        public JsonResult VideLsForModule(int Id)
        {



            Training Training = db.Trainings.Find(Id);

            var VideoLessons = Training.Training_VideoL.Where(e => e.language == "ru").ToList();

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