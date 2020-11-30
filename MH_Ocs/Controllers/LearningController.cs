using MH_Ocs.Models;
using NReco.ImageGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Flurl;
using Flurl.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Data.Entity;

namespace MH_Ocs.Controllers
{

    [Authorize]
    public class LearningController : Controller
    {
        private Entities db = new Entities();

        public string GetUserName()
        {

            return User.Identity.Name;
        }

        // GET: MODULES-------------------------------------------------------------------------------------------------
        public async Task<ActionResult> Modules(string language)
        {

            if (language == null) { language = "ru"; }
            string UserName = GetUserName();
            try
            {

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);


                JVLO jvlo = await db.JVLOes.FirstOrDefaultAsync(e => e.UserName == UserName);

                var videolessons = await db.VideoXLs.Where(e => e.XId < jvlo.X).ToListAsync();




                UserProgress UserProgress = await UserGet.Progress(UserName, userinfo.LevelId, language);


                /* Delete 01.12.2020 */
                foreach (var video in videolessons)
                {
                    LessonVideoTime look = userinfo.LessonVideoTimes.FirstOrDefault(e => e.LessonXId == video.XId);
                    if (look == null)
                    {

                        jvlo.X = video.XId;
                        await db.SaveChangesAsync();
                        break;

                    }


                }
                /*-------------------------*/


                string GetCerURL = null;

                var userCert = userinfo.Users_Certificates.ToList();

                bool createcert = false;



                if (userCert.Where(e => e.TypeCert != 3).Count() > 0)
                {

                    createcert = true;
                }
                else if (userCert.Where(e => e.TypeCert == 3).Count() > 0)
                {

                    Users_Certificates Users_Certificate = userCert.Where(e => e.TypeCert == 3).FirstOrDefault();

                    if (Users_Certificate != null)
                    {
                        if (Users_Certificate.PublicCert == true)
                        {
                            GetCerURL = Users_Certificate.CertificateURL;
                        }
                        else
                        {
                            double LastVXId = UserProgress.EnableVideoXLs.Where(e => e.MId == 4).Max(e => e.XId);
                            LessonVideoTime LessonVideoTime = userinfo.LessonVideoTimes.FirstOrDefault(e => e.LessonXId == LastVXId);
                            if (LessonVideoTime != null)
                            {
                                if (LessonVideoTime.Status == true)
                                {
                                    Users_Certificate.PublicCert = true;
                                    GetCerURL = Users_Certificate.CertificateURL;

                                    User_SendCompleteTraining user_completeTR = await db.User_SendCompleteTraining.FirstOrDefaultAsync(e => e.UserName == UserName);

                                    if (user_completeTR == null)
                                    {
                                        bool sts = await markTrainingComplete(UserName);

                                        if (sts)
                                        {
                                            User_SendCompleteTraining newuser_completeTR = new User_SendCompleteTraining();
                                            newuser_completeTR.UserName = UserName;
                                            newuser_completeTR.SendCertificate = true;
                                            db.User_SendCompleteTraining.Add(newuser_completeTR);
                                        }

                                    }

                                    await db.SaveChangesAsync();
                                }
                            }
                        }


                    }
                }

                if (userCert.Count > 0 && createcert == true)
                {
                    bool certpublic = false;


                    foreach (var item in userCert)
                    {
                        if (item.PublicCert)
                        {
                            certpublic = true;
                        }

                    }

                    if (certpublic)
                    {

                        User_SendCompleteTraining user_completeTR = await db.User_SendCompleteTraining.FirstOrDefaultAsync(e => e.UserName == UserName);

                        if (user_completeTR == null)
                        {
                            bool sts = await markTrainingComplete(UserName);

                            if (sts)
                            {
                                User_SendCompleteTraining newuser_completeTR = new User_SendCompleteTraining();
                                newuser_completeTR.UserName = UserName;
                                newuser_completeTR.SendCertificate = true;
                                db.User_SendCompleteTraining.Add(newuser_completeTR);
                            }

                        }
                        GetCerURL = await MakeCertificate(UserName, true, "ru", true);
                    }
                    else
                    {

                        await MakeCertificate(UserName, false, "ru", true);
                    }

                    db.Users_Certificates.RemoveRange(userCert);
                    await db.SaveChangesAsync();

                }


                // Get User Modules
                var Modules = await db.Moduls.Where(e => e.Enable == true).OrderBy(e => e.XId).ToListAsync();
                var Modules_Property_Enable_List = new List<Modules_Property_Enable>();

                if (Modules.Count > 0)
                {
                    foreach (var Modul in Modules)
                    {

                        Modul_userLevel Modul_userLevelOBJ = Modul.Modul_userLevel.FirstOrDefault();
                        Modules_Property_Enable Modules_Property_EnableOBJ = new Modules_Property_Enable();
                        Modules_Property MP = Modul.Modules_Property.FirstOrDefault(e => e.lang == language);
                        if (MP == null)
                        {
                            MP = Modul.Modules_Property.FirstOrDefault(e => e.lang == "ru");
                        }

                        if (Modul_userLevelOBJ.LevelId <= userinfo.LevelId)
                        {
                            Modules_Property_EnableOBJ.Modul_Property_IN = MP;
                            Modules_Property_EnableOBJ.Enable = true;

                        }
                        else
                        {
                            Modules_Property_EnableOBJ.Modul_Property_IN = MP;
                            Modules_Property_EnableOBJ.Enable = false;
                        }

                        Modules_Property_Enable_List.Add(Modules_Property_EnableOBJ);

                    }

                }



                ViewBag.OUK = UserProgress.OUK;
                ViewBag.TBB = UserProgress.TBB;
                ViewBag.CertURL = GetCerURL;
                ViewBag.language = language;
                return View(Modules_Property_Enable_List);
            }
            catch { }

            string ActionName = "Modules";


            return RedirectToAction("ErrorLogOff", "Account", new { language = language, ActionName = ActionName });

        }


        // GET: VIDEOS---------------------------------------------------------------------
        public async Task<ActionResult> Videos(int Id, string language)
        {

            if (language == null) { language = "ru"; }
            string UserName = GetUserName();
            try
            {

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
                bool ModulNot = false;
                var Modules = await db.Moduls.FindAsync(Id);

                if (Modules == null || Modules.Enable == false)
                {
                    ModulNot = true;
                }
                else
                {

                    int ModulLevel = Modules.Modul_userLevel.FirstOrDefault().LevelId;
                    if (ModulLevel > userinfo.LevelId)
                    {
                        ModulNot = true;
                    }

                }
                if (ModulNot)
                {
                    return RedirectToAction("Noaccess", "Error");
                }

                bool EmptyLessons = false;
                //-!!!- User Progress If you create every action then you need take this Progress modul 
                UserProgress UserProgress = await UserGet.Progress(UserName, userinfo.LevelId, language);
                JVLO jv = UserProgress.JVLO;
                int NextV = UserProgress.NextV;
                var lessons = new List<VideoL>();
                int? DisVID = null;



                if (jv == null)
                {


                    EmptyLessons = true;
                }


                if (EmptyLessons == false)
                {
                    VideoL videols = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == jv.X && e.language == language);
                    if (videols == null)
                    {
                        videols = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == jv.X && e.language == "ru");
                    }

                    DisVID = videols.Id;


                    var VideoLsRu = Modules.VideoLs.Where(e => e.language == "ru" && e.VideoXL.Enable == true).ToList();

                    if (VideoLsRu.Count == 0)
                    {
                        EmptyLessons = true;
                    }



                    if (EmptyLessons == false)
                    {

                        foreach (var item in VideoLsRu)
                        {
                            VideoL videol = Modules.VideoLs.FirstOrDefault(e => e.language == language && e.XId == item.XId && e.VideoXL.Enable == true);
                            if (videol == null)
                            {
                                lessons.Add(item);
                            }
                            else
                            {
                                lessons.Add(videol);
                            }
                        }
                        lessons = lessons.OrderBy(e => e.XId).ToList();
                    }

                }







                Modules_Property Modules_Property = Modules.Modules_Property.FirstOrDefault(e => e.lang == language);
                if (Modules_Property == null)
                    Modules_Property = Modules.Modules_Property.FirstOrDefault(e => e.lang == "ru");



                ViewBag.OUK = UserProgress.OUK;
                ViewBag.TBB = UserProgress.TBB;
                ViewBag.ModulName = Modules_Property.Titile;
                ViewBag.JVX = jv.X;
                ViewBag.NextV = NextV;
                ViewBag.DisVID = DisVID;
                ViewBag.language = language;

                return View(lessons);
            }
            catch { }

            string ActionName = "Videos";


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
                JVLO jv = UserProgress.JVLO;



                VideoL videolsForXid = await db.VideoLs.FindAsync(Id);

                VideoL videols = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == videolsForXid.XId && e.language == language);
                if (videols == null)
                {
                    videols = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == videolsForXid.XId && e.language == "ru");
                }

                if (videols != null && videols.VideoXL.Enable == true)
                {
                    int LevelId = db.Modul_userLevel.FirstOrDefault(e => e.ModuleId == videols.ModulId).LevelId;

                    if (userinfo.LevelId < LevelId)
                    {
                        return RedirectToAction("Noaccess", "Error");
                    }


                    if (videols.XId <= jv.X)
                    {

                        LikeV lkv = userinfo.LikeVs.FirstOrDefault(e => e.VideoXId == videols.XId);
                        if (lkv != null)
                        { ViewBag.lkv = 1; }


                        Isbranni izb = userinfo.Isbrannis.FirstOrDefault(e => e.VideoLXId == videols.XId);
                        if (izb != null)
                        { ViewBag.izb = 1; }


                        var vxl = UserProgress.EnableVideoXLs.ToList();
                        VideoXL vlXID = vxl.Where(e => e.XId < videols.XId).OrderByDescending(e => e.XId).FirstOrDefault();
                        VideoXL vnXID = vxl.Where(e => e.XId > videols.XId).OrderBy(e => e.XId).FirstOrDefault();


                        double LastVXId = UserProgress.EnableVideoXLs.Where(e => e.MId == 4).Max(e => e.XId);


                        VideoL vl = null;
                        VideoL vn = null;


                        if (vlXID != null)
                        {
                            vl = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == vlXID.XId && e.language == language);
                            if (vl == null && language != "ru")
                                vl = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == vlXID.XId && e.language == "ru");
                        }
                        if (vnXID != null)
                        {
                            vn = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == vnXID.XId && e.language == language);
                            if (vn == null && language != "ru")
                                vn = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == vnXID.XId && e.language == "ru");
                        }





                        bool VideoTest = false;
                        bool FulllookL = false;
                        string GetCerURL = null;
                        int CertStatus = 0;



                        int NextV = 0;


                        var test = videols.Tests.ToList();
                        Models.Task task = await db.Tasks.FirstOrDefaultAsync(e => e.LessonXId == videols.XId);




                        if (test.Count() > 0)
                        {
                            VideoTest = true;
                            NextV = 1;
                            ValitO vo = userinfo.ValitOS.FirstOrDefault(e => e.VdeoLXId == videols.XId);
                            if (vo != null)
                            {
                                if (vo.KB >= 75)
                                {
                                    NextV = 0;

                                }
                            }
                        }

                        if (NextV == 0 && task != null)
                        {
                            NextV = 2;
                            UserTaskCheck UserTaskCheck = userinfo.UserTaskChecks.FirstOrDefault(e => e.TaskId == task.Id);
                            if (UserTaskCheck != null)
                            {
                                if (UserTaskCheck.Status == true)
                                {
                                    NextV = 0;
                                }
                            }
                            else
                            {
                                UserTaskCheck newUserTaskCheck = new UserTaskCheck();
                                newUserTaskCheck.TaskId = task.Id;
                                newUserTaskCheck.Status = false;
                                userinfo.UserTaskChecks.Add(newUserTaskCheck);
                                await db.SaveChangesAsync();
                            }

                        }

                        LessonVideoTime LessonVideoTime = userinfo.LessonVideoTimes.FirstOrDefault(e => e.LessonXId == videols.XId);
                        if (LessonVideoTime != null)
                        {
                            if (LessonVideoTime.Status == true)
                            {
                                FulllookL = true;
                            }
                        }

                        if (vn != null && NextV == 0)
                        {

                            NextV = 3;
                        }




                        if (videols.XId == LastVXId)
                        {
                            Users_Certificates UserCert = userinfo.Users_Certificates.FirstOrDefault();
                            if (UserCert == null)
                            {

                                GetCerURL = await MakeCertificate(UserName, false, "ru", true);
                            }
                            else
                            {

                                if (UserCert.PublicCert == false)
                                {
                                    GetCerURL = UserCert.CertificateURL;
                                }

                            }

                            if (VideoTest == false)
                            {

                                if (FulllookL == false)
                                {

                                    CertStatus = 1;


                                }

                            }

                        }




                        if (vl != null)
                        { ViewBag.LastVId = vl.Id; }

                        if (vn != null)
                        { ViewBag.NextVId = vn.Id; }


                        ViewBag.OUK = UserProgress.OUK;
                        ViewBag.TBB = UserProgress.TBB;
                        ViewBag.NextV = NextV;
                        ViewBag.FullookL = FulllookL;
                        ViewBag.CertStatus = CertStatus;
                        ViewBag.CerURL = GetCerURL;
                        ViewBag.language = language;

                        return View(videols);
                    }

                }

                return RedirectToAction("Noaccess", "Error");
            }
            catch { }

            string ActionName = "Videolesson";


            return RedirectToAction("ErrorLogOff", "Account", new { language = language, ActionName = ActionName });

        }


        // GET: TASKS----------------------------------------------------------------------
        public async Task<ActionResult> Tasks(string language)
        {

            try
            {
                string UserName = GetUserName();
                if (language == null) { language = "ru"; }



                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);

                //-!!!- User Progress If you create every action then you need take this Progress modul 
                UserProgress UserProgress = await UserGet.Progress(UserName, userinfo.LevelId, language);


                UserTaskCheck UserTaskCheck = userinfo.UserTaskChecks.Where(e => e.TaskId == 34).FirstOrDefault();
                bool Task_First = false;


                if (UserTaskCheck != null)
                {

                    Task_First = true;
                }

                ViewBag.Task_First = Task_First;
                ViewBag.OUK = UserProgress.OUK;
                ViewBag.TBB = UserProgress.TBB;
                ViewBag.language = language;

                return View();
            }
            catch { }

            string ActionName = "Tasks";


            return RedirectToAction("ErrorLogOff", "Account", new { language = language, ActionName = ActionName });

        }


        public async Task<ActionResult> CTask(int Id, string language)
        {
            try
            {

                if (language == null) { language = "ru"; }
                string UserName = GetUserName();

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);



                if (Id == 34)
                {
                    return RedirectToAction("Task_ten_dreams", new { language = language });
                }

                return RedirectToAction("AllVideos", new { language = language });
            }
            catch { }

            string ActionName = "CTask";


            return RedirectToAction("ErrorLogOff", "Account", new { language = language, ActionName = ActionName });

        }
        public async Task<ActionResult> Task_ten_dreams(string language)
        {

            try
            {

                string UserName = GetUserName();
                if (language == null) { language = "ru"; }

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
                UserProgress UserProgress = await UserGet.Progress(UserName, userinfo.LevelId, language);
                JVLO jv = UserProgress.JVLO;

                Task_First task = userinfo.Task_First.FirstOrDefault();





                VideoL videols = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == 3 && e.language == language);
                if (videols == null)
                {
                    videols = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == 3 && e.language == "ru");
                }




                if (jv.X >= 3)
                {


                    var test = videols.Tests.ToList();

                    if (test.Count > 0)
                    {

                        ValitO vo = userinfo.ValitOS.Where(e => e.VdeoLXId == 3).FirstOrDefault();
                        if (vo != null)
                        {
                            if (vo.KB < 75)
                            {
                                return RedirectToAction("Test", "Learning", new { Id = videols.Id, language = language });
                            }

                        }
                    }

                    ViewBag.language = language;
                    ViewBag.OUK = UserProgress.OUK;
                    ViewBag.TBB = UserProgress.TBB;
                    return View(task);
                }
            }
            catch
            { }

            string ActionName = "Task_ten_dreams";


            return RedirectToAction("ErrorLogOff", "Account", new { language = language, ActionName = ActionName });

        }

        [HttpPost]
        public async Task<ActionResult> Task_ten_dreams(Task_First task, string language)
        {
            if (language == null) { language = "ru"; }


            string UserName = GetUserName();

            if (task != null)
            {

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
                Task_First tskobj = userinfo.Task_First.FirstOrDefault();
                if (tskobj == null)
                {
                    task.UserId = userinfo.Id;
                    db.Task_First.Add(task);

                }
                else
                {
                    tskobj.First = task.First;
                    tskobj.Second = task.Second;
                    tskobj.Third = task.Third;
                    tskobj.Four = task.Four;
                    tskobj.Five = task.Five;
                    tskobj.Six = task.Six;
                    tskobj.Seven = task.Seven;
                    tskobj.Eight = task.Eight;
                    tskobj.Nine = task.Nine;
                    tskobj.Ten = task.Ten;

                }

                db.SaveChanges();

                UserTaskCheck ustskch = userinfo.UserTaskChecks.FirstOrDefault(e => e.TaskId == 34);
                UserTaskCheck UserTaskCheck = new UserTaskCheck();

                if (ustskch == null)
                {

                    UserTaskCheck.TaskId = 34;
                    UserTaskCheck.Status = false;

                }



                if (!string.IsNullOrEmpty(task.First) && !string.IsNullOrEmpty(task.Second) &&
                !string.IsNullOrEmpty(task.Third) && !string.IsNullOrEmpty(task.Four) &&
                !string.IsNullOrEmpty(task.Five) && !string.IsNullOrEmpty(task.Six) &&
                !string.IsNullOrEmpty(task.Seven) && !string.IsNullOrEmpty(task.Eight) &&
                !string.IsNullOrEmpty(task.Nine) && !string.IsNullOrEmpty(task.Ten)
                )
                {

                    if (ustskch == null)
                    {
                        UserTaskCheck.Status = true;
                        userinfo.UserTaskChecks.Add(UserTaskCheck);
                    }
                    else
                    {

                        ustskch.Status = true;
                    }

                    VideoL VideoL = db.VideoLs.FirstOrDefault(e => e.XId == 3 && e.language == "ru");
                    var test = VideoL.Tests.ToList();

                    if (test.Count == 0)
                    {
                        JVLO jv = await db.JVLOes.FirstOrDefaultAsync(e => e.UserName == UserName);

                        var Moduls_userLevel = await db.Modul_userLevel.Where(e => e.LevelId <= userinfo.LevelId).ToListAsync();
                        var Moduls = Moduls_userLevel.Select(e => e.Modul).Where(e => e.Enable == true).ToList();
                        var EnableVideoXLs = new List<VideoXL>();
                        foreach (var Module in Moduls)
                        {
                            EnableVideoXLs.AddRange(Module.VideoXLs.Where(e => e.Enable == true).ToList());
                        }

                        VideoXL NextVideoXL = EnableVideoXLs.Where(e => e.XId > 3).OrderBy(e => e.XId).FirstOrDefault();



                        if (NextVideoXL != null)
                        {
                            if (NextVideoXL.XId > jv.X)
                            {
                                jv.X = NextVideoXL.XId;
                            }
                        }

                    }


                }





                db.SaveChanges();


            }

            return RedirectToAction("Task_ten_dreams", new { language = language });
        }


        // GET: TEST-----------------------------------------------------------------------
        public async Task<ActionResult> Test(int Id, string language)
        {

            try
            {


                string UserName = GetUserName();
                if (language == null) { language = "ru"; }


                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
                UserProgress UserProgress = await UserGet.Progress(UserName, userinfo.LevelId, language);
                JVLO jv = UserProgress.JVLO;


                bool error = false;

                VideoL videols = await db.VideoLs.FindAsync(Id);

                if (videols == null || videols.VideoXL.Enable == false)
                {
                    error = true;
                }
                else
                {
                    int LevelId = db.Modul_userLevel.FirstOrDefault(e => e.ModuleId == videols.ModulId).LevelId;
                    if (userinfo.LevelId < LevelId)
                        error = true;
                }

                if (error)
                    return RedirectToAction("Noaccess", "Error");




                if (videols.XId <= jv.X)
                {
                    bool FulllookL = false;
                    LessonVideoTime LessonVideoTime = userinfo.LessonVideoTimes.FirstOrDefault(e => e.LessonXId == videols.XId);
                    if (LessonVideoTime != null)
                    {
                        if (LessonVideoTime.Status == true)
                        {
                            FulllookL = true;
                        }
                    }

                    if (FulllookL)
                    {

                        int taskvideo = 0;



                        Models.Task tskv = db.Tasks.FirstOrDefault(e => e.LessonXId == videols.XId);
                        if (tskv != null)
                        {
                            taskvideo = 1;
                        }




                        int? NextId = null;

                        VideoXL NextVideoXL = UserProgress.EnableVideoXLs.FirstOrDefault(e => e.XId > videols.XId);
                        if (NextVideoXL != null)
                        {
                            VideoL NextVideo = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == NextVideoXL.XId && e.language == language);
                            if (NextVideo == null)
                            {
                                NextVideo = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == NextVideoXL.XId && e.language == "ru");
                            }
                            if (NextVideo != null)
                                NextId = NextVideo.Id;
                        }


                        var tst = videols.Tests.ToList();

                        if (tst.Count > 0)
                        {

                            Test gettest = tst.FirstOrDefault();

                            List<string> testOptions = new List<string>();

                            string answer = "";
                            Random random = new Random();
                            int idAnswer = random.Next(4);


                            if (gettest.Answer == 1)
                            {
                                answer = gettest.A;

                            }
                            else
                            {
                                testOptions.Add(gettest.A);
                            }

                            if (gettest.Answer == 2)
                            {
                                answer = gettest.B;
                            }
                            else
                            {
                                testOptions.Add(gettest.B);

                            }


                            if (gettest.Answer == 3)
                            {
                                answer = gettest.C;
                            }
                            else
                            {
                                testOptions.Add(gettest.C);
                            }


                            if (gettest.Answer == 4)
                            {
                                answer = gettest.D;
                            }
                            else
                            {
                                testOptions.Add(gettest.D);
                            }
                            if (gettest.Answer == 5)
                            {
                                answer = gettest.E;
                            }
                            else
                            {
                                testOptions.Add(gettest.E);
                            }





                            var RanDomTest = testOptions.OrderBy(a => Guid.NewGuid()).ToList();

                            RanDomTest.Insert(idAnswer, answer);

                            Test rntest = new Test();
                            rntest.Id = gettest.Id;
                            rntest.Question = gettest.Question;
                            rntest.LessonId = gettest.LessonId;
                            rntest.A = RanDomTest[0];
                            rntest.B = RanDomTest[1];
                            rntest.C = RanDomTest[2];
                            rntest.D = RanDomTest[3];
                            rntest.E = RanDomTest[4];
                            rntest.Answer = idAnswer + 1;



                            double LastVXId = UserProgress.EnableVideoXLs.Where(e => e.MId == 4).Max(e => e.XId);
                            int CertStatus = 0;
                            string GetCerURL = null;


                            if (videols.XId == LastVXId && LastVXId > 2)
                            {
                                Users_Certificates UserCert = userinfo.Users_Certificates.FirstOrDefault();
                                if (UserCert != null)
                                {
                                    if (UserCert.PublicCert != true)
                                    {
                                        CertStatus = 1;
                                        GetCerURL = UserCert.CertificateURL;

                                    }

                                }

                            }

                            ViewBag.OUK = UserProgress.OUK;
                            ViewBag.TBB = UserProgress.TBB;
                            ViewBag.VideoName = gettest.VideoL.Name;
                            ViewBag.IdModul = tst.FirstOrDefault().VideoL.ModulId;
                            ViewBag.CountTest = tst.Count;
                            ViewBag.Ids = NextId;
                            ViewBag.ztf = taskvideo;
                            ViewBag.FullookL = FulllookL;
                            ViewBag.CertStatus = CertStatus;
                            ViewBag.CerURL = GetCerURL;
                            ViewBag.language = language;


                            return View(rntest);

                        }




                    }
                }

            }
            catch
            {
                string ActionName = "Test";


                return RedirectToAction("ErrorLogOff", "Account", new { language = language, ActionName = ActionName });
            }




            return RedirectToAction("Noaccess", "Error");

        }


        public async Task<ActionResult> TestGet(int Id, int LessonId)
        {

            var Tests = await db.Tests.Where(e => e.LessonId == LessonId).ToListAsync();
            Test gettest = Tests.Where(e => e.Id > Id).FirstOrDefault();

            List<string> testOptions = new List<string>();

            string answer = "";
            Random random = new Random();
            int idAnswer = random.Next(4);


            if (gettest.Answer == 1)
            {
                answer = gettest.A;

            }
            else
            {
                testOptions.Add(gettest.A);
            }

            if (gettest.Answer == 2)
            {
                answer = gettest.B;
            }
            else
            {
                testOptions.Add(gettest.B);

            }


            if (gettest.Answer == 3)
            {
                answer = gettest.C;
            }
            else
            {
                testOptions.Add(gettest.C);
            }


            if (gettest.Answer == 4)
            {
                answer = gettest.D;
            }
            else
            {
                testOptions.Add(gettest.D);
            }
            if (gettest.Answer == 5)
            {
                answer = gettest.E;
            }
            else
            {
                testOptions.Add(gettest.E);
            }





            var RanDomTest = testOptions.OrderBy(a => Guid.NewGuid()).ToList();

            RanDomTest.Insert(idAnswer, answer);

            Test rntest = new Test();
            rntest.Id = gettest.Id;
            rntest.Question = gettest.Question;
            rntest.LessonId = gettest.LessonId;
            rntest.A = RanDomTest[0];
            rntest.B = RanDomTest[1];
            rntest.C = RanDomTest[2];
            rntest.D = RanDomTest[3];
            rntest.E = RanDomTest[4];
            rntest.Answer = idAnswer + 1;

            ViewBag.VideoName = gettest.VideoL.Name;
            return View(rntest);
        }


        // GET: USEFUL_SERVICES------------------------------------------------------------
        public async Task<ActionResult> Useful_services(string language)
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


                return View();
            }
            catch
            { }

            string ActionName = "Useful_services";


            return RedirectToAction("ErrorLogOff", "Account", new { language = language, ActionName = ActionName });


        }


        // GET: ALLVIDEOS------------------------------------------------------------------
        public async Task<ActionResult> AllVideos(string language)
        {
            try
            {


                string UserName = GetUserName();
                if (language == null) { language = "ru"; }


                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
                UserProgress UserProgress = await UserGet.Progress(UserName, userinfo.LevelId, language);
                JVLO jv = UserProgress.JVLO;


                VideoL videols = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == jv.X && e.language == language);
                if (videols == null)
                {
                    videols = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == jv.X && e.language == "ru");
                }

                int NextV = UserProgress.NextV;



                var EnableVideoLs = new List<VideoL>();

                foreach (var videoXL in UserProgress.EnableVideoXLs)
                {
                    VideoL addVideoL = videoXL.VideoLs.FirstOrDefault(e => e.language == language);
                    if (language != "ru" && addVideoL == null)
                    {
                        addVideoL = videoXL.VideoLs.FirstOrDefault(e => e.language == "ru");
                    }
                    if (addVideoL != null)
                    {

                        EnableVideoLs.Add(addVideoL);
                    }
                }






                ViewBag.OUK = UserProgress.OUK;
                ViewBag.TBB = UserProgress.TBB;
                ViewBag.language = language;
                ViewBag.JVX = jv.X;
                ViewBag.NextV = NextV;
                ViewBag.DisVId = videols.Id;

                EnableVideoLs = EnableVideoLs.OrderBy(e => e.XId).ToList();


                return View(EnableVideoLs);


            }
            catch
            { }

            string ActionName = "AllVideos";


            return RedirectToAction("ErrorLogOff", "Account", new { language = language, ActionName = ActionName });
        }


        public async Task<JsonResult> TestV(int id, int KB, string language)
        {

            try
            {

                string UserName = GetUserName();
                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
                VideoL videols = await db.VideoLs.FindAsync(id);

                if (language == null) { language = "kz"; }



                var Moduls_userLevel = await db.Modul_userLevel.Where(e => e.LevelId <= userinfo.LevelId).ToListAsync();
                var Moduls = Moduls_userLevel.Select(e => e.Modul).Where(e => e.Enable == true).ToList();
                var EnableVideoXLs = new List<VideoXL>();
                foreach (var Module in Moduls)
                {
                    EnableVideoXLs.AddRange(Module.VideoXLs.Where(e => e.Enable == true).ToList());
                }



                ValitO vo = userinfo.ValitOS.Where(e => e.VdeoLXId == videols.XId).FirstOrDefault();
                JVLO jv = await db.JVLOes.FirstOrDefaultAsync(e => e.UserName == UserName);


                Models.Task tsk = await db.Tasks.FirstOrDefaultAsync(e => e.LessonXId == videols.XId);

                VideoXL NextVideoXL = EnableVideoXLs.Where(e => e.XId > videols.XId).OrderBy(e => e.XId).FirstOrDefault();


                if (NextVideoXL != null)
                {

                    if (tsk != null)
                    {
                        UserTaskCheck usertskcheck = userinfo.UserTaskChecks.Where(e => e.TaskId == tsk.Id).FirstOrDefault();
                        if (usertskcheck != null)
                        {
                            if (usertskcheck.Status && KB > 74)
                            {
                                if (NextVideoXL.XId > jv.X)
                                {
                                    jv.X = NextVideoXL.XId;
                                }

                            }
                        }

                    }
                    else if (KB > 74)
                    {

                        if (NextVideoXL.XId > jv.X)
                        {
                            jv.X = NextVideoXL.XId;
                        }
                    }



                }




                if (vo != null)
                {
                    if (KB > vo.KB)
                    {
                        vo.KB = KB;
                    }

                }
                else if (vo == null)
                {
                    ValitO vonew = new ValitO();
                    vonew.KB = KB;
                    vonew.UserId = userinfo.Id;
                    vonew.VdeoLXId = videols.XId;
                    db.ValitOS.Add(vonew);

                }

                db.SaveChanges();

                int OB = 0;

                var valitos = userinfo.ValitOS.ToList();
                if (valitos.Count > 0)
                {
                    foreach (var item in userinfo.ValitOS.ToList())
                    {
                        OB += item.KB;
                    }

                    jv.TBB = OB / valitos.Count;
                    await db.SaveChangesAsync();
                }
            }
            catch { }

            return Json("");

        }

        public async Task<JsonResult> like(int id, int tf)
        {

            string result = "error";
            try
            {

                VideoL videols = await db.VideoLs.FindAsync(id);
                VideoLEM videolem = videols.VideoXL.VideoLEMs.FirstOrDefault();

                string UserName = GetUserName();

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
                if (tf == 1)
                {
                    videolem.Likes += 1;
                    LikeV lkv = new LikeV();
                    lkv.UserId = userinfo.Id;
                    lkv.VideoXId = videols.XId;
                    db.LikeVs.Add(lkv);
                }
                else
                {
                    videolem.Likes -= 1;
                    LikeV lkv = userinfo.LikeVs.Where(e => e.VideoXId == videols.XId).FirstOrDefault();
                    db.LikeVs.Remove(lkv);

                }
                await db.SaveChangesAsync();
                result = "success";
            }
            catch { }

            return Json(result);
        }


        public async Task<JsonResult> Addeye(int id)
        {

            string result = "error";
            try
            {

                VideoL videols = await db.VideoLs.FindAsync(id);

                VideoLEM videoLEM = await db.VideoLEMs.FirstOrDefaultAsync(e => e.VideoXId == videols.XId);


                string UserName = GetUserName();

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);


                EyeV eye = userinfo.EyeVs.FirstOrDefault(e => e.VideoXId == videols.XId);

                if (eye == null)
                {
                    videoLEM.Eye += 1;
                    EyeV eyenew = new EyeV();
                    eyenew.UserId = userinfo.Id;
                    eyenew.VideoXId = videols.XId;
                    db.EyeVs.Add(eyenew);
                    await db.SaveChangesAsync();
                }

                result = "success";
            }
            catch
            {

            }
            return Json(result);

        }
        public async Task<JsonResult> izb(int id, int tf)
        {
            string result = "error";
            try
            {
                VideoL video = await db.VideoLs.FindAsync(id);
                string UserName = GetUserName();

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);


                if (tf == 1)
                {

                    Isbranni izb = new Isbranni();
                    izb.UserId = userinfo.Id;
                    izb.VideoLXId = video.XId;
                    db.Isbrannis.Add(izb);
                }
                else
                {

                    Isbranni izb = userinfo.Isbrannis.Where(e => e.VideoLXId == video.XId).FirstOrDefault();
                    if (izb != null)
                    {
                        db.Isbrannis.Remove(izb);
                    }

                }

                await db.SaveChangesAsync();
                result = "success";
            }
            catch { }



            return Json(result);
        }
        
        [HttpPost]
        public async Task<JsonResult> lookVideoL(int id, int ts, int cert, string language)
        {


            string UserName = GetUserName();
            if (language == null) { language = "ru"; }

            UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);



            //-----GET User ENABLE VIDEOXLs----------------------------------------
            var Moduls_userLevel = await db.Modul_userLevel.Where(e => e.LevelId <= userinfo.LevelId).ToListAsync();
            var Moduls = Moduls_userLevel.Select(e => e.Modul).Where(e => e.Enable == true).ToList();
            var EnableVideoXLs = new List<VideoXL>();
            foreach (var Module in Moduls)
            {
                EnableVideoXLs.AddRange(Module.VideoXLs.Where(e => e.Enable == true).ToList());
            }
            //-----END------------------------------------------------------------





            string result = "error";
            VideoL videols = await db.VideoLs.FindAsync(id);

            if (videols != null)
            {



                LessonVideoTime LessonVideoTime = userinfo.LessonVideoTimes.FirstOrDefault(e => e.LessonXId == videols.XId);
                if (LessonVideoTime == null)
                {
                    LessonVideoTime LessonVideo = new LessonVideoTime();
                    LessonVideo.LessonXId = videols.XId;
                    LessonVideo.UserId = userinfo.Id;
                    LessonVideo.Status = true;
                    db.LessonVideoTimes.Add(LessonVideo);

                }
                else
                {
                    LessonVideoTime.Status = true;
                }

                JVLO jv = await db.JVLOes.FirstOrDefaultAsync(e => e.UserName == UserName);




                VideoXL NextVideoXL = EnableVideoXLs.Where(e => e.XId > videols.XId).OrderBy(e => e.XId).FirstOrDefault();


                if (NextVideoXL != null)
                {
                    if (ts == 0)
                    {
                        if (NextVideoXL.XId > jv.X)
                        {
                            jv.X = NextVideoXL.XId;


                        }

                    }


                }

                double LastVXId = EnableVideoXLs.Where(e => e.MId == 4).Max(e => e.XId);


                if (videols.XId == LastVXId && ts == 0)
                {


                    Users_Certificates certificate = userinfo.Users_Certificates.FirstOrDefault();
                    if (certificate != null)
                    {


                        certificate.PublicCert = true;
                        result = "cert";

                        User_SendCompleteTraining user_completeTR = await db.User_SendCompleteTraining.FirstOrDefaultAsync(e => e.UserName == UserName);

                        if (user_completeTR == null)
                        {
                            bool sts = await markTrainingComplete(UserName);

                            if (sts)
                            {
                                User_SendCompleteTraining newuser_completeTR = new User_SendCompleteTraining();
                                newuser_completeTR.UserName = UserName;
                                newuser_completeTR.SendCertificate = true;
                                db.User_SendCompleteTraining.Add(newuser_completeTR);
                                await db.SaveChangesAsync();
                            }

                        }


                    }

                }

                await db.SaveChangesAsync();

            }

            return Json(result);
        }
        
        [HttpPost]
        public async Task<ActionResult> certPublic()
        {

            string UserName = GetUserName();

            string result = "error";

            UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
            Users_Certificates certificate = userinfo.Users_Certificates.FirstOrDefault();
            if (certificate != null)
            {
                certificate.PublicCert = true;

                User_SendCompleteTraining user_completeTR = await db.User_SendCompleteTraining.FirstOrDefaultAsync(e => e.UserName == UserName);

                if (user_completeTR == null)
                {
                    bool sts = await markTrainingComplete(UserName);

                    if (sts)
                    {
                        User_SendCompleteTraining newuser_completeTR = new User_SendCompleteTraining();
                        newuser_completeTR.UserName = UserName;
                        newuser_completeTR.SendCertificate = true;
                        db.User_SendCompleteTraining.Add(newuser_completeTR);
                        await db.SaveChangesAsync();
                    }

                }
                result = "succes";
                await db.SaveChangesAsync();
            }



            return Json(result);
        }
        
        public static async Task<bool> markTrainingComplete(string UserName)
        {

            bool sts = false;
            try
            {

                string url = "https://my.marinehealth.asia/api/proxy/userManage/markTrainingCompleted?email=" + UserName;

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("AuthToken", "99acbcede1554b0da47a9be1c43b09A145F43DCDE848169BEA5697859DD16A0acb303dec7c6544d39690e6e078157b05");
                var response = await client.PostAsync(url, null);
                string result = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode.ToString() == "OK")
                {

                    sts = true;

                }




            }
            catch { }
            return sts;

        }

        public async Task<string> MakeCertificate(string UserName, bool certpub, string language, bool certnull)
        {

            string result = null;
            UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
            language = "ru";
            try
            {

                string Lname = "";
                string Fname = "";

                if (userinfo.Lname != null)
                {
                    Lname = userinfo.Lname;
                }

                if (userinfo.Fname != null)
                {
                    Fname = userinfo.Fname;
                }

                DateTime CurrentDate = DateTime.Now.AddHours(6);
                string Day = CurrentDate.Day.ToString();
                string Month = CurrentDate.Month.ToString();
                string Year = CurrentDate.Year.ToString();
                string Date = Day + "." + Month + "." + Year;
                string imageName = "";


                await System.Threading.Tasks.Task.Run(() =>
                {

                    int usernameLg = (Lname.Length + Fname.Length) / 2;

                    int fontSize = 140;
                    int alfa = 840 - usernameLg * 89;


                    if (usernameLg > 20)
                    {
                        fontSize = 120;
                        alfa = 840 - usernameLg * 80;
                    }



                    int leftPosition = 600 + alfa;


                    var htmlToImageConv = new HtmlToImageConverter();

                    //string html = "<html> <head><meta http-equiv=Content-Type content='text/html; charset=UTF-8'><style type='text/css'>@font-face{font-family: 'Font Name';src: url('http://academy.marinehealth.asia/Images/cert/10846.ttf') format('truetype');}body {font-family: 'Font Name', Fallback, sans-serif;}</style><script type = 'text/javascript' src = 'a8535914-f191-11e9-9d71-0cc47a792c0a_id_a8535914-f191-11e9-9d71-0cc47a792c0a_files/wz_jsgraphics.js' ></script></head><body ><div style = 'position:absolute;left:50%;margin-left:-1462px;top:0px;width:2924px;height:1995px;border-style:outset;overflow:hidden' > <div style = 'position:absolute;left:0px;top:0px' ><img src='http://academy.marinehealth.asia/Images/cert/cert-ru.jpg'></div><div style = 'position:absolute;right:120px;top:820px;color:#2b529f;font-size:" + fontsize + ";text-align:end'>" + Lname + " <br>" + Fname + " </div><div style='position: absolute; right: 150px; top: 1875px;color:#2b529f;font-size:50px;'>" + DateTime.Now.AddHours(6).ToString("dd.MM.yyyy") + "</div></body></html>";


                    string html = "<html> <head><meta http-equiv=Content-Type content='text/html; charset=UTF-8'><style type='text/css'></style><script type = 'text/javascript' src = 'a8535914-f191-11e9-9d71-0cc47a792c0a_id_a8535914-f191-11e9-9d71-0cc47a792c0a_files/wz_jsgraphics.js'></script></head><body style='font-family: Geneva, Arial, Helvetica, sans-serif'><div style = 'position:absolute;left:50%;margin-left:-1462px;top:0px;width:2924px;height:2075px;border-style:outset;overflow:hidden'> <div style = 'position:absolute;left:0px;top:0px' ><img src='http://academy.marinehealth.asia/Images/cert/certificate-v3.jpg'></div><div style='position:absolute;left:" + leftPosition + "px;top:1030px;color:#000;font-size:" + fontSize + "px;font-weight:900'>" + Lname + " " + Fname + "</div><div style='position: absolute; left:1350px;top:1770px;color:#717171;font-size:45px;font-weight:900'>" + DateTime.Now.AddHours(6).ToString("dd.MM.yyyy") + "</div></body></html>";

                    //string html = "<html><head><meta http-equiv=Content-Type content='text/html; charset=UTF-8'><meta name='viewport' content='width = device - width, minimum - scale = 0.1' ></head><body style='margin: 0px; background: #0e0e0e;font-family: Geneva, Arial, Helvetica, sans-serif;'><img style='-webkit-user-select: none;margin: auto;display:table' src='http://academy.marinehealth.asia/Images/cert/certificate-v3.jpg' width='1320' height='930'><div style='position:absolute;left:" + leftPosition + "px;top:480px;color:#000000;font-size:" + fontSize + "px'>Шарипжанов Айдар</div><div style='position:absolute; left:1020px; top:795px; color:#615f60d9; font-size:17px; font-weight:900'>04.11.2020</div></body></html>";


                    var img = htmlToImageConv.GenerateImage(html, ImageFormat.Jpeg);







                    imageName = "~/Images/UserCertificate/" + userinfo.Fname + "_" + "(certificate_" + userinfo.Id + ")" + ".jpg";

                    String path = HttpContext.Server.MapPath(imageName); //Path
                    System.IO.File.WriteAllBytes(path, img);
                });




                if (certnull)
                {
                    int UserCertCount = userinfo.Users_Certificates.Where(e => e.TypeCert == 3 && e.PublicCert == certpub).Count();
                    if (UserCertCount == 0)
                    {
                        Users_Certificates UserCerNew = new Users_Certificates();
                        UserCerNew.CertificateURL = imageName;
                        UserCerNew.PublicCert = certpub;
                        UserCerNew.UserId = userinfo.Id;
                        UserCerNew.TypeCert = 3;
                        UserCerNew.MakeDate = DateTime.Now.AddHours(6).Date;
                        db.Users_Certificates.Add(UserCerNew);
                        await db.SaveChangesAsync();
                    }
                }


                result = imageName;


            }
            catch
            { }
            return result;


        }
        
    }
}