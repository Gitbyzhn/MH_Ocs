using MH_Ocs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using MH_Ocs.Models.APIClass;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Data.Entity;

namespace MH_Ocs.API
{
    [Authorize]
    public class videolessonsController : ApiController
    {

        private Entities db = new Entities();

        // POST: api/Default
        public async  Task<IHttpActionResult> Post([FromBody]VideoLessonsCs data)
        {



            ResponseVideoLs response = new ResponseVideoLs();

            var EnableVideoLs = new List<VideoL>();

            var UVideoLessons = new List<UVideoLessons>();


            try
            {

                string language = null;
                int? MId = null;

                if (data != null)
                {
                    language = data.language;
                    MId = data.MId;

                }


                if (language == null) {language = "ru"; }

                string UserName = User.Identity.Name;

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);

               

                JVLO JV = await db.JVLOes.FirstOrDefaultAsync(e => e.UserName == UserName);

                var EnableVideoXLs = new List<VideoXL>();



                var Moduls_userLevel = await db.Modul_userLevel.Where(e => e.LevelId <= userinfo.LevelId).ToListAsync();



                var Moduls = Moduls_userLevel.Select(e => e.Modul).Where(e => e.Enable == true).ToList();

                if (MId != null)
                {

                    Moduls = Moduls.Where(e => e.XId == MId).ToList();
                }


                foreach (var Module in Moduls)
                {
                    EnableVideoXLs.AddRange(Module.VideoXLs.Where(e => e.Enable == true).ToList());
                }



                if (EnableVideoXLs.Count > 0)
                {

                    EnableVideoXLs = EnableVideoXLs.OrderBy(e => e.XId).ToList();


                   

                    foreach (var videoXL in EnableVideoXLs)
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

                    foreach (var videoLs in EnableVideoLs)
                    {
                           
                 

                        string Image = videoLs.Iconimg!=null?Url.Content(videoLs.Iconimg):null;
                        bool Enable = true;
                        int Like = 0;
                        int View = 0;
                        int minute = 0;

                        VideoLEM videLem = videoLs.VideoXL.VideoLEMs.FirstOrDefault();

                        if (videLem != null)
                        {
                            Like = videLem.Likes;
                            View = videLem.Eye;
                            minute = videLem.minute;

                        }


                        if (JV.X < videoLs.XId)
                        {

                            Image = videoLs.Iconimg != null ? Url.Content(videoLs.Iconimg2) : null;
                            Enable = false;
                        }


                        string Date = null;
                        string Time = null;
                        if (videoLs.Date != null)
                        {
                            Date = videoLs.Date.Value.ToString("MM/dd/yyyy");
                            Time = videoLs.Date.Value.ToString("HH:mm:ss");
                        }

                        UVideoLessons.Add(new UVideoLessons {
                            Id = videoLs.Id,
                            MId = videoLs.Modul.XId,
                            lang = videoLs.language,
                            Name = videoLs.Name,
                            Image = Image,
                            Like = Like,
                            View = View,
                            Date = Date,
                            Time = Time,
                            minute = minute,
                            Enable = Enable
                            
                        });


                    }

                    

                }

                response.status = "ok";
                response.UVideoLessons = UVideoLessons;
                return Json(response);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

     

                

        }

    }
}
