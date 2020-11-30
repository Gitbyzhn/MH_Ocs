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
    public class tasktendreamController : ApiController
    {

        private Entities db = new Entities();

        public async Task<IHttpActionResult> Get()
        {

            ResponseUTasktendreams response = new ResponseUTasktendreams();

            UTasktendream tasktendream = new UTasktendream();

            response.status = "filed";
            response.Tasktendream = tasktendream;

            try
            {

                string UserName = User.Identity.Name;

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);

                Task_First usertasktendream = userinfo.Task_First.FirstOrDefault();

                if (usertasktendream != null)
                {

                    tasktendream.First = usertasktendream.First;
                    tasktendream.Second = usertasktendream.Second;
                    tasktendream.Third = usertasktendream.Third;
                    tasktendream.Fourth = usertasktendream.Four;
                    tasktendream.Fifth = usertasktendream.Five;
                    tasktendream.Sixth = usertasktendream.Six;
                    tasktendream.Seventh = usertasktendream.Seven;
                    tasktendream.Eighth = usertasktendream.Eight;
                    tasktendream.Ninth = usertasktendream.Nine;
                    tasktendream.Tenth = usertasktendream.Ten;
                }

                response.status = "ok";
                response.Tasktendream = tasktendream;

                return Json(response);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }


        public async Task<IHttpActionResult> Post([FromBody] TasktendreamCs data)
        {


            //if (data.VideoLessonId == null)
            //{
            //    return BadRequest("VideoLessonId not specified");
            //}

            try
            {

                string UserName = User.Identity.Name;

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);

                Task_First tasktendream = userinfo.Task_First.FirstOrDefault();

                if (tasktendream != null)
                {
                    tasktendream.First = data.First;
                    tasktendream.Second = data.Second;
                    tasktendream.Third = data.Third;
                    tasktendream.Four = data.Fourth;
                    tasktendream.Five = data.Fifth;
                    tasktendream.Six = data.Sixth;
                    tasktendream.Seven = data.Seventh;
                    tasktendream.Eight = data.Eighth;
                    tasktendream.Nine = data.Ninth;
                    tasktendream.Ten = data.Tenth;

                }
                else
                {


                    Task_First usertasktendream = new Task_First();

                    usertasktendream.First = data.First;
                    usertasktendream.Second = data.Second;
                    usertasktendream.Third = data.Third;
                    usertasktendream.Four = data.Fourth;
                    usertasktendream.Five = data.Fifth;
                    usertasktendream.Six = data.Sixth;
                    usertasktendream.Seven = data.Seventh;
                    usertasktendream.Eight = data.Eighth;
                    usertasktendream.Nine = data.Ninth;
                    usertasktendream.Ten = data.Tenth;

                    userinfo.Task_First.Add(usertasktendream);
                }

                db.SaveChanges();

                UserTaskCheck ustskch = userinfo.UserTaskChecks.FirstOrDefault(e => e.TaskId == 34);
                UserTaskCheck UserTaskCheck = new UserTaskCheck();

                if (ustskch == null)
                {

                    UserTaskCheck.TaskId = 34;
                    UserTaskCheck.Status = false;

                }


                if (!string.IsNullOrEmpty(data.First) && !string.IsNullOrEmpty(data.Second) &&
                    !string.IsNullOrEmpty(data.Third) && !string.IsNullOrEmpty(data.Fourth) &&
                    !string.IsNullOrEmpty(data.Fifth) && !string.IsNullOrEmpty(data.Sixth) &&
                    !string.IsNullOrEmpty(data.Seventh) && !string.IsNullOrEmpty(data.Eighth)&&
                    !string.IsNullOrEmpty(data.Ninth) && !string.IsNullOrEmpty(data.Tenth))
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

                    VideoL VideoL = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == 3 && e.language == "ru");
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

                

              await db.SaveChangesAsync();
              return Ok();
            }
            catch
            {

                return BadRequest("filed");
            }

           

        }
    }
}
