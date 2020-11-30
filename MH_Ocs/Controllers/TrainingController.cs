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
    [Authorize]
    public class TrainingController : Controller
    {

        private Entities db = new Entities();

        public string GetUserName()
        {

            return User.Identity.Name;
        }



        // GET: MODULES--------------------------------------------------------------------
        public async Task<ActionResult> Modules(string language)
        {

            try
            {

                string UserName = GetUserName();
                if (language == null) { language = "ru"; }


                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
                UserProgress UserProgress = await UserGet.Progress(UserName, userinfo.LevelId, language);

                // Get User Modules
                var Trainings = await db.Trainings.Where(e => e.Enable == true).OrderBy(e => e.XId).ToListAsync();

                var trainings_properties = new List<Training_Property>();


                foreach (var training in Trainings)
                {

                    trainings_properties.Add(training.Training_Property.FirstOrDefault());
                }


                ViewBag.OUK = UserProgress.OUK;
                ViewBag.TBB = UserProgress.TBB;
                ViewBag.language = language;
                return View(trainings_properties);
            }
            catch { }

            string ActionName = "Trainig-Modules";
            return RedirectToAction("ErrorLogOff", "Account", new { language = language, ActionName = ActionName });

        }


        // GET: VIDEOS---------------------------------------------------------------------
        public async Task<ActionResult> Videos(int Id, string language)
        {


            try
            {

                string UserName = GetUserName();
                if (language == null) { language = "ru"; }


                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
                UserProgress UserProgress = await UserGet.Progress(UserName, userinfo.LevelId, language);

                var Trainings = await db.Trainings.FindAsync(Id);

                if (Trainings == null || Trainings.Enable == false)
                {
                    return RedirectToAction("Noaccess", "Error");
                }

                Training_Property Training_Property = Trainings.Training_Property.FirstOrDefault(e => e.lang == language);

                ViewBag.OUK = UserProgress.OUK;
                ViewBag.TBB = UserProgress.TBB;
                ViewBag.ModulName = Training_Property.Titile;
                ViewBag.language = language;

                List<Training_VideoXL> VideoXLessons = Trainings.Training_VideoXL.Where(e => e.Enable == true).OrderByDescending(e=>e.XId).ToList();
                List<Training_VideoL> VideoLessons = VideoXLessons.Select(e => e.Training_VideoL.FirstOrDefault()).ToList();
                return View(VideoLessons);
            }
            catch { }

            string ActionName = "Trainig-Videos";


            return RedirectToAction("ErrorLogOff", "Account", new { language = language, ActionName = ActionName });

        }


        // GET: VIDEOLESSON----------------------------------------------------------------
        public async Task<ActionResult> Videolesson(int Id, string language)
        {

            try
            {
                string UserName = GetUserName();
                if (language == null) { language = "ru"; }

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
                UserProgress UserProgress = await UserGet.Progress(UserName, userinfo.LevelId, language);


                Training_VideoL videols = await db.Training_VideoL.FindAsync(Id);


                if (videols != null && videols.Training_VideoXL.Enable == true)
                {

                    Training_LikeV lkv = userinfo.Training_LikeV.Where(e => e.VideoXId == videols.XId).FirstOrDefault();
                    if (lkv != null)
                    { ViewBag.lkv = 1; }


                    Training_VideoL vl = db.Training_VideoL.Where(e => e.XId < videols.XId && e.language == language).OrderByDescending(e => e.XId).FirstOrDefault();
                    Training_VideoL vn = db.Training_VideoL.Where(e => e.XId > videols.XId && e.language == language).FirstOrDefault();

                    int? vlId = null;

                    int? vnId = null;



                    if (vl != null)
                    {
                        vlId = vl.Id;
                    }

                    if (vn != null)
                    {
                        vnId = vn.Id;
                    }


                    ViewBag.LastVId = vlId;
                    ViewBag.NextVId = vnId;
                    ViewBag.OUK = UserProgress.OUK;
                    ViewBag.TBB = UserProgress.TBB;
                    ViewBag.language = language;

                    return View(videols);


                }

                return RedirectToAction("Noaccess", "Error");
            }
            catch { }

            string ActionName = "Videolesson";


            return RedirectToAction("ErrorLogOff", "Account", new { language = language, ActionName = ActionName });

        }



        public async Task<JsonResult> like(int id, int tf)
        {

            try
            {

                string UserName = GetUserName();

                Training_VideoL videols = await db.Training_VideoL.FindAsync(id);
                Training_VideoLEM videoLEM = await db.Training_VideoLEM.FirstOrDefaultAsync(e => e.VideoXId == videols.XId);


                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);

                if (tf == 1)
                {
                    videoLEM.Likes += 1;
                    Training_LikeV lkv = new Training_LikeV();
                    lkv.UserId = userinfo.Id;
                    lkv.VideoXId = videols.XId;
                    db.Training_LikeV.Add(lkv);
                }
                else
                {
                    videoLEM.Likes -= 1;
                    Training_LikeV lkv = userinfo.Training_LikeV.Where(e => e.VideoXId == videols.XId).FirstOrDefault();
                    db.Training_LikeV.Remove(lkv);

                }
                await db.SaveChangesAsync();


            }
            catch { }

            return Json("");
        }


        public async Task<JsonResult> Addeye(int id)
        {

            string result = "error";
            try
            {
                string UserName = GetUserName();

                Training_VideoL videols = await db.Training_VideoL.FindAsync(id);

                Training_VideoLEM videoLEM = await db.Training_VideoLEM.FirstOrDefaultAsync(e => e.VideoXId == videols.XId);

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);


                Training_EyeV eye = userinfo.Training_EyeV.Where(e => e.VideoXId == videols.XId).FirstOrDefault();

                if (eye == null)
                {
                    videoLEM.Eye += 1;
                    Training_EyeV eyenew = new Training_EyeV();
                    eyenew.UserId = userinfo.Id;
                    eyenew.VideoXId = videols.XId;
                    db.Training_EyeV.Add(eyenew);
                    await db.SaveChangesAsync();
                }
                result = "success";
            }
            catch { }

            return Json(result);

        }





    }
}