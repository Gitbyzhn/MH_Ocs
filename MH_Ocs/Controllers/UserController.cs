using MH_Ocs.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MH_Ocs.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        // GET: User

        private Entities db = new Entities();
        public string GetUserName()
        {

            return User.Identity.Name;
        }
        public async Task<ActionResult> Initialize(string language)
        {


            try
            {

                string UserName = GetUserName();


                UT UserToken = await db.UTs.FirstOrDefaultAsync(e => e.UserName == UserName);
                if (UserToken != null)
                {

                    var rqstinfus = WebRequest.Create("https://my.marinehealth.asia/api/proxy/userprofile/get?includeSensitiveData=false");
                    var Httprqstinfus = (HttpWebRequest)rqstinfus;
                    Httprqstinfus.PreAuthenticate = true;

                    Httprqstinfus.Headers.Add("AuthToken", UserToken.Token);
                    Httprqstinfus.Accept = "application/json";
                    string responseiu;
                    WebResponse responseiuw = Httprqstinfus.GetResponse();
                    using (Stream stream = responseiuw.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            responseiu = reader.ReadToEnd();
                        }
                    }

                    MHUserInfo MHUserInfo = JsonConvert.DeserializeObject<MHUserInfo>(responseiu);


                    if (MHUserInfo.ImageFileName != null && MHUserInfo.Contacts.Count > 0)
                    {
                        MHUserInfo.ImageFileName = "https://my.marinehealth.asia/files/users/" + MHUserInfo.Contacts[0].UserId + "/images/" + MHUserInfo.ImageFileName;
                    }



                    UserInfo UserInfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);



                    if (UserInfo != null)
                    {
                        UserInfo.Lname = MHUserInfo.LastName;
                        UserInfo.Fname = MHUserInfo.FirstName;
                        UserInfo.Image = MHUserInfo.ImageFileName;
                        UserInfo.LevelId = MHUserInfo.LevelId == null ? 0 : MHUserInfo.LevelId;


                    }
                    else
                    {
                        UserInfo UserInfoNew = new UserInfo();
                        UserInfoNew.Lname = MHUserInfo.LastName;
                        UserInfoNew.Fname = MHUserInfo.FirstName;
                        UserInfoNew.Image = MHUserInfo.ImageFileName;
                        UserInfoNew.UserName = UserName;
                        UserInfoNew.LevelId = MHUserInfo.LevelId == null ? 0 : MHUserInfo.LevelId;
                        UserInfoNew.RegTime = DateTime.Now.AddHours(6);
                        db.UserInfoes.Add(UserInfoNew);


                        JVLO jv = await db.JVLOes.FirstOrDefaultAsync(e => e.UserName == UserName);
                        if (jv == null)
                        {
                            JVLO jvnew = new JVLO();
                            jvnew.OV = 1;
                            jvnew.TBB = 0;
                            jvnew.UserName = UserName;
                            jvnew.X = 1;
                            db.JVLOes.Add(jvnew);
                        }


                    }
                    
                    await db.SaveChangesAsync();

                    return RedirectToAction("Modules", "Learning", new { language = language });
                }


            }
            catch
            {

            }

            string ActionName = "Initialize";
            return RedirectToAction("ErrorLogOff", "Account", new { language = language, ActionName = ActionName });

        }
        
        public async Task<ActionResult> favorites(string language)
        {

            try
            {

                string UserName = GetUserName();
                if (language == null) { language = "ru"; }




                UserInfo UserInfoOBJ = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
                JVLO jv = await db.JVLOes.FirstOrDefaultAsync(e => e.UserName == UserName);

                UserProgress UserProgress = await UserGet.Progress(UserName, UserInfoOBJ.LevelId, language);


                var EnableVideoLs = new List<VideoL>();

                foreach (var videoXL in UserProgress.EnableVideoXLs)
                {
                    if (videoXL.XId <= jv.X)
                    {
                        EnableVideoLs.AddRange(videoXL.VideoLs);
                    }

                  
                }



                List<VideoL> videoizb = new List<VideoL>();
                foreach (var izb in UserInfoOBJ.Isbrannis.ToList())
                {
                    VideoL vid = EnableVideoLs.Where(e => e.language == language && e.XId == izb.VideoLXId).FirstOrDefault();
                    if (vid == null)
                        vid = EnableVideoLs.Where(e => e.language == "ru" && e.XId == izb.VideoLXId).FirstOrDefault();

                    if (vid != null)
                    {
                        videoizb.Add(vid);
                    }

                }


                ViewBag.OUK = UserProgress.OUK;
                ViewBag.TBB = UserProgress.TBB;
                ViewBag.language = language;


                return View(videoizb.ToList());
            }
            catch
            {

            }


            string ActionName = "favorites";


            return RedirectToAction("ErrorLogOff", "Account", new { language = language, ActionName = ActionName });


        }
        
        public async Task<ActionResult> certificates(string language)
        {
            try
            {
                string UserName = GetUserName();


                if (language == null) { language = "ru"; }
                ViewBag.language = language;


                UserInfo UserInfoOBJ = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);

                UserProgress UserProgress = await UserGet.Progress(UserName, UserInfoOBJ.LevelId, language);

                var user_certificates = UserInfoOBJ.Users_Certificates.ToList();



                ViewBag.language = language;
                ViewBag.OUK = UserProgress.OUK;
                ViewBag.TBB = UserProgress.TBB;
                return View(user_certificates);
            }
            catch
            {
              
            }


            string ActionName = "certificates";


            return RedirectToAction("ErrorLogOff", "Account", new { language = language, ActionName = ActionName });

        }

        
    }
}