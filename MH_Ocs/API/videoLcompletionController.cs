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
    public class videoLcompletionController : ApiController
    {

        private Entities db = new Entities();

        // POST: api/Default
        public async Task<IHttpActionResult> Post([FromBody]UVideoLcompletionCs data)
        {
            
            ResponseUVideoLcompletion response = new ResponseUVideoLcompletion();
            
            if (data == null)
            {
                return BadRequest("data null");
            }
            if (data.VideoId == null || (data.TestTotal == null && data.FoolLook == null))
            {

                return BadRequest("data null");
            }

            VideoL video = await db.VideoLs.FindAsync(data.VideoId);

            if (video == null)
            {

                return BadRequest("video null");
            }

            try
            {

                string UserName = User.Identity.Name;

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);

                JVLO JV = await db.JVLOes.FirstOrDefaultAsync(e => e.UserName == UserName);

                bool LookVideo = true;
                bool Task = true;
                bool Test = true;

                LessonVideoTime look = userinfo.LessonVideoTimes.FirstOrDefault(e => e.LessonXId == video.XId);

                if (data.FoolLook == true)
                {


                    if (look == null)
                    {

                        LessonVideoTime newlook = new LessonVideoTime();
                        newlook.LessonXId = video.XId;
                        newlook.Status = true;
                        userinfo.LessonVideoTimes.Add(newlook);
                        await db.SaveChangesAsync();
                    }
                    else if (look.Status == false)
                    {

                        look.Status = true;
                        await db.SaveChangesAsync();
                    }

                    response.msgfoollook = "ok";

                }

                if (look != null)
                {
                    if (look.Status == false)
                    {
                        LookVideo = false;
                    }

                }


                Models.Task task = await db.Tasks.FirstOrDefaultAsync(e => e.LessonXId == video.XId);
                if (task != null)
                {
                    UserTaskCheck UserTaskCheck = userinfo.UserTaskChecks.FirstOrDefault(e => e.TaskId == task.Id);
                    if (UserTaskCheck != null)
                    {
                        if (UserTaskCheck.Status == false)
                        {
                            Task = false;
                        }
                    }
                }




                var test = video.Tests.ToList();


                if (test.Count() > 0)
                {
                    if (Task == false)
                    {
                        response.msgtesttotal = "Task not completed";
                        Test = false;
                    }

                    if (LookVideo == false)
                    {
                        response.msgtesttotal = "Video not fully viewed";
                        Test = false;
                    }



                }


                if (test.Count() > 0 && Test == true)
                {

                    ValitO vo = userinfo.ValitOS.FirstOrDefault(e => e.VdeoLXId == video.XId);

                    int KB = vo != null ? vo.KB : 0;


                    if (data.TestTotal != null)
                    {

                        if (data.TestTotal > 0)
                        {
                            if (vo == null)
                            {
                                KB = (int)data.TestTotal;

                                ValitO newvo = new ValitO();
                                newvo.VdeoLXId = video.XId;
                                newvo.KB = KB;
                                userinfo.ValitOS.Add(newvo);

                                await db.SaveChangesAsync();
                            }
                            else if (vo.KB < data.TestTotal)
                            {
                                KB = (int)data.TestTotal;
                                vo.KB = KB;
                                await db.SaveChangesAsync();
                            }

                            response.msgtesttotal = "ok";

                        }

                    }

                    if (KB < 75)
                    {
                        Test = false;
                    }


                }



                if (LookVideo == true && Task == true && Test == true)
                {
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


                    VideoXL NextVideoXL = EnableVideoXLs.Where(e => e.XId > video.XId).OrderBy(e => e.XId).FirstOrDefault();

                  
                    VideoXL vnXID = vxl.Where(e => e.XId > video.XId).OrderBy(e => e.XId).FirstOrDefault();

        
                    VideoL vn = null;

                    
                    if (vnXID != null)
                    {
                        vn = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == vnXID.XId && e.language == video.language);
                        if (vn == null && video.language != "ru")
                            vn = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == vnXID.XId && e.language == "ru");
                    }

                    if (vn != null)
                    { response.nextId = vn.Id; }

                    if (NextVideoXL != null)
                    {

                        if (NextVideoXL.XId > JV.X)
                        {

                            JV.X = NextVideoXL.XId;
                            await db.SaveChangesAsync();

                        }
                    }


                }
                response.status = "ok";
                return Json(response);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);

            }
            
        }

    }
}
