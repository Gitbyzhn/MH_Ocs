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
    public class favoritesController : ApiController
    {

        private Entities db = new Entities();

        // POST: api/Default
        public async Task<IHttpActionResult> Post(string language)
        {



            ResponseVideoLs response = new ResponseVideoLs();

            var EnableVideoLs = new List<VideoL>();

            var UVideoLessons = new List<UVideoLessons>();


            try
            {

                if (language == null) { language = "ru"; }

                string UserName = User.Identity.Name;

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);



                JVLO JV = await db.JVLOes.FirstOrDefaultAsync(e => e.UserName == UserName);


                //-----GET User ENABLE VIDEOXLs----------------------------------------
                var Moduls_userLevel = await db.Modul_userLevel.Where(e => e.LevelId <= userinfo.LevelId).ToListAsync();
                var Moduls = Moduls_userLevel.Select(e => e.Modul).Where(e => e.Enable == true).ToList();
                var EnableVideoXLs = new List<VideoXL>();
                foreach (var Module in Moduls)
                {
                    EnableVideoXLs.AddRange(Module.VideoXLs.Where(e => e.Enable == true).ToList());
                }
                //-----END------------------------------------------------------------



                EnableVideoXLs = EnableVideoXLs.OrderBy(e => e.XId).ToList();


                foreach (var videoXL in EnableVideoXLs)
                {

                    EnableVideoLs.AddRange(videoXL.VideoLs);

                }
                
                List<VideoL> videoizb = new List<VideoL>();
                foreach (var izb in userinfo.Isbrannis.ToList())
                {
                    VideoL vid = EnableVideoLs.FirstOrDefault(e => e.language == language && e.XId == izb.VideoLXId);
                    if (vid == null)
                        vid = EnableVideoLs.FirstOrDefault(e => e.language == "ru" && e.XId == izb.VideoLXId);

                    if (vid != null)
                    {
                        videoizb.Add(vid);
                    }

                }

                foreach (var videoLs in videoizb)
                {



                    string Image = videoLs.Iconimg != null ? Url.Content(videoLs.Iconimg) : null;
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

                    UVideoLessons.Add(new UVideoLessons
                    {
                        Id = videoLs.Id,
                        MId = videoLs.Modul.XId,
                        lang = videoLs.language,
                        Name = videoLs.Name,
                        Image = Image,
                        Like = Like,
                        View = View,
                        minute = minute,
                        Enable = Enable

                    });


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
