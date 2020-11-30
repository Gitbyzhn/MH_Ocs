using MH_Ocs.Models;
using MH_Ocs.Models.APIClass;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MH_Ocs.API
{
    [Authorize]
    public class videolessonController : ApiController
    {

        private Entities db = new Entities();

        // POST: api/Default
        public async Task<IHttpActionResult> Post([FromBody]VideoLessonCs data)
        {



            ResponseVideoL response = new ResponseVideoL();
            UVideoLesson UVideoLesson = new UVideoLesson();

            try
            {

                string UserName = User.Identity.Name;

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);

                JVLO JV = await db.JVLOes.FirstOrDefaultAsync(e => e.UserName == UserName);
                

                if (data == null)
                {

                    return BadRequest("data null");
                }

                if (data.Id == null)
                {
                    
                    return BadRequest("data id null");
                }


                VideoL VideoL = await db.VideoLs.FindAsync(data.Id);

                if (VideoL == null)
                {
                    return BadRequest("VideoL null");

                }

                if (VideoL.Modul.Enable != true)
                {
                    return BadRequest("VideoL Disable");
                }

                if (VideoL.VideoXL.Enable != true)
                {
                    return BadRequest("VideoL Disable");
                }

                if (VideoL.Modul.Modul_userLevel.FirstOrDefault().LevelId > userinfo.LevelId)
                {

                    return BadRequest("User level not available");
                }

                if (VideoL.XId > JV.X)
                {
                    return BadRequest("User level not available");
                }



                //-----GET User ENABLE VIDEOXLs----------------------------------------
                var Moduls_userLevel = await db.Modul_userLevel.Where(e => e.LevelId <= userinfo.LevelId).ToListAsync();
                var Moduls = Moduls_userLevel.Select(e => e.Modul).Where(e => e.Enable == true).ToList();
                var EnableVideoXLs = new List<VideoXL>();
                foreach (var Module in Moduls)
                {
                    EnableVideoXLs.AddRange(Module.VideoXLs.Where(e => e.Enable == true).ToList());
                }
                //-----END------------------------------------------------------------



                var vxl = EnableVideoXLs.OrderBy(e => e.XId).ToList();


                VideoXL vlXID = vxl.Where(e => e.XId < VideoL.XId).OrderByDescending(e => e.XId).FirstOrDefault();
                VideoXL vnXID = vxl.Where(e => e.XId > VideoL.XId).OrderBy(e => e.XId).FirstOrDefault();

                VideoL vl = null;
                VideoL vn = null;

                if (vlXID != null)
                {
                    vl = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == vlXID.XId && e.language == VideoL.language);
                    if (vl == null && VideoL.language != "ru")
                        vl = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == vlXID.XId && e.language == "ru");
                }
                if (vnXID != null)
                {
                    vn = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == vnXID.XId && e.language == VideoL.language);
                    if (vn == null && VideoL.language != "ru")
                        vn = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == vnXID.XId && e.language == "ru");
                }
                
       


                int Like = 0;
                int View = 0;
                bool test = false;
                if (VideoL.Tests.Count > 0)
                {
                    test = true;
                    ValitO valito = userinfo.ValitOS.FirstOrDefault(e => e.VdeoLXId == VideoL.XId);
                    if (valito != null)
                    {
                        if (valito.KB > 74)
                        {
                            test = false;
                        }
                    }


                }


                bool FoolLook = false;

                LessonVideoTime look = userinfo.LessonVideoTimes.FirstOrDefault(e => e.LessonXId == VideoL.XId);
                if (look != null)
                {
                    if (look.Status)
                    {
                        FoolLook = true;
                    }
                }



                bool task = false;

                Models.Task Vtask = await db.Tasks.FirstOrDefaultAsync(e => e.LessonXId == VideoL.XId);

                if (Vtask != null)
                {

                    UserTaskCheck UserTaskCheck = userinfo.UserTaskChecks.FirstOrDefault(e => e.TaskId == Vtask.Id);
                    if (UserTaskCheck != null)
                    {
                        if (UserTaskCheck.Status == false)
                        {
                            task = true;
                        }
                    }
                    else if (FoolLook == true)
                    {

                        task = true;
                        UserTaskCheck newUserTaskCheck = new UserTaskCheck();
                        newUserTaskCheck.TaskId = Vtask.Id;
                        newUserTaskCheck.Status = false;
                        userinfo.UserTaskChecks.Add(newUserTaskCheck);
                        await db.SaveChangesAsync();
                    }


                }





                VideoLEM videLem = VideoL.VideoXL.VideoLEMs.FirstOrDefault();

                if (videLem != null)
                {
                    Like = videLem.Likes;
                    View = videLem.Eye;


                }

                bool certificate = false;

                double LastVXId = vxl.Where(e => e.MId == 4).Max(e => e.XId);

                if (VideoL.XId == LastVXId)
                {

                    if (FoolLook == false)
                    {
                        certificate = true;
                    }

                }



                bool liked = false;
                bool viewed = false;
                bool chosen = false;
                EyeV eye = userinfo.EyeVs.FirstOrDefault(e => e.VideoXId == VideoL.XId);
                if (eye != null)
                {
                    viewed = true;
                }
                LikeV like = userinfo.LikeVs.FirstOrDefault(e => e.VideoXId == VideoL.XId);
                if (like != null)
                {
                    liked = true;
                }
                Isbranni isb = userinfo.Isbrannis.FirstOrDefault(e => e.VideoLXId == VideoL.XId);
                if (isb != null)
                {
                    chosen = true;
                }


                if (vl != null)
                { UVideoLesson.previousId = vl.Id; }

                if (vn != null)
                { UVideoLesson.nextId = vn.Id; }




                UVideoLesson.Id = VideoL.Id;
                UVideoLesson.lang = VideoL.language;
                UVideoLesson.Name = VideoL.Name;
                UVideoLesson.Link = VideoL.Link;
                UVideoLesson.Like = Like;
                UVideoLesson.Views = View;
                UVideoLesson.Liked = liked;
                UVideoLesson.Viewed = viewed;
                UVideoLesson.Chosen = chosen;
                UVideoLesson.FoolLook = FoolLook;
                UVideoLesson.Test = test;
                UVideoLesson.Task = task;
                UVideoLesson.certificate = certificate;

                response.status = "ok";
                response.UVideoLesson = UVideoLesson;

                return Json(response);
            }

            catch(Exception ex) {

                return BadRequest(ex.Message);
            }

  
            

        }
    }
}
