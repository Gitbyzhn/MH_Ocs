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
    public class WebinarController : Controller
    {


        private Entities db = new Entities();

        public string GetUserName()
        {

            return User.Identity.Name;
        }



        // GET: VIDEOS---------------------------------------------------------------------
        public async  Task<ActionResult> Videos(string language)
        {


            try
            {

                string UserName = GetUserName();
                if (language == null) { language = "ru"; }


                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
                UserProgress UserProgress = await UserGet.Progress(UserName, userinfo.LevelId, language);


                ViewBag.OUK = UserProgress.OUK;
                ViewBag.TBB = UserProgress.TBB;
                ViewBag.language = language;

                List<Webinar_VideoL> VideoLessons = await db.Webinar_VideoL.Where(e => e.language == language).OrderByDescending(e=>e.XId).ToListAsync();

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




                Webinar_VideoL videols = await db.Webinar_VideoL.FindAsync(Id);




                if (videols != null && videols.Webinar_VideoXL.Enable == true)
                {





                    Webinar_LikeV lkv = userinfo.Webinar_LikeV.Where(e => e.VideoXId == videols.XId).FirstOrDefault();
                    if (lkv != null)
                    { ViewBag.lkv = 1; }


                    Webinar_VideoL vl = db.Webinar_VideoL.Where(e => e.XId < videols.XId && e.language == language).OrderByDescending(e => e.XId).FirstOrDefault();
                    Webinar_VideoL vn = db.Webinar_VideoL.Where(e => e.XId > videols.XId && e.language == language).FirstOrDefault();

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

                Webinar_VideoL videols = await db.Webinar_VideoL.FindAsync(id);
                Webinar_VideoLEM videoLEM = await db.Webinar_VideoLEM.FirstOrDefaultAsync(e => e.VideoXId == videols.XId);


                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);

                if (tf == 1)
                {
                    videoLEM.Likes += 1;
                    Webinar_LikeV lkv = new Webinar_LikeV();
                    lkv.UserId = userinfo.Id;
                    lkv.VideoXId = videols.XId;
                    db.Webinar_LikeV.Add(lkv);
                }
                else
                {
                    videoLEM.Likes -= 1;
                    Webinar_LikeV lkv = userinfo.Webinar_LikeV.Where(e => e.VideoXId == videols.XId).FirstOrDefault();
                    db.Webinar_LikeV.Remove(lkv);

                }

                await db.SaveChangesAsync();
            }
            catch { }
            return Json("");
        }


        public async Task<JsonResult> Addeye(int id)
        {
            try
            {


                string UserName = GetUserName();

                Webinar_VideoL videols = await db.Webinar_VideoL.FindAsync(id);

                Webinar_VideoLEM videoLEM = await db.Webinar_VideoLEM.FirstOrDefaultAsync(e => e.VideoXId == videols.XId);

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);


                Webinar_EyeV eye = userinfo.Webinar_EyeV.Where(e => e.VideoXId == videols.XId).FirstOrDefault();

                if (eye == null)
                {
                    videoLEM.Eye += 1;
                    Webinar_EyeV eyenew = new Webinar_EyeV();
                    eyenew.UserId = userinfo.Id;
                    eyenew.VideoXId = videols.XId;
                    db.Webinar_EyeV.Add(eyenew);
                    await db.SaveChangesAsync();
                }
            }
            catch { }
            return Json("");

        }
    }
}